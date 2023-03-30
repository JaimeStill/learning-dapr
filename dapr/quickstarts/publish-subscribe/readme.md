# Quickstart: Publish and Subscribe

**Get started with Dapr's Publish and Subscribe building block**  

https://docs.dapr.io/getting-started/quickstarts/pubsub-quickstart/

Let's take a look at Dapr's [Publish and Subscribe (Pub/Sub) building block](https://docs.dapr.io/developing-applications/building-blocks/pubsub/). In this Quickstart, you will run a publisher microservice and a subscriber microservice to demonstrate how Dapr enables a pub/sub pattern.

1. Using a publisher service, developers can repeatedly publish messages to a topic.
2. [A pub/sub component](https://docs.dapr.io/concepts/components-concept/#pubsub-brokers) queues or brokers those messages. Our example below uses Redis, but you can use RabbitMQ, Kafka, etc.
3. The subscriber to that topic pulls messages from the queue and processes them.

![pubsub](https://docs.dapr.io/images/pubsub-quickstart/pubsub-diagram.png)

## Step 1: Pre-requisites

For this example, you will need:

* [Dapr CLI and initialized environment](https://docs.dapr.io/getting-started)
* [.NET SDK or .NET 7 SDK installed](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Step 2: Set up the environment

[Quickstarts repo](https://github.com/dapr/quickstarts/) was cloned and the `pub_sub/csharp/sdk/` directory was copied to [`src/dotnet/sdk/`](./src/dotnet/sdk/) local to this readme. Additionally, `pub_sub/components` was copied to [`src/components`](./src/components/).

## Step 3: Subscribe to topics

In a terminal window, navigate to [`src/dotnet/sdk/order-processor/`](./src/dotnet/sdk/order-processor/) and run the following:

```bash
dotnet restore
dotnet build

dapr run --app-id order-processor --resources-path ../../components --app-port 7002 -- dotnet run
```

In the `order-processor` subscriber, we're subscribing to the Redis instance called `orderpubsub` [(as defined in the `pubsub.yaml` component)](./src/components/pubsub.yaml) and the topic `orders`. This enables your app code to talk to the Redis comopnent instance through the Dapr sidecar.

[**Program.cs**](./src/dotnet/sdk/order-processor/Program.cs#L17)

```cs
// Dapr subscription in [Topic] routes orders topic to this route
app.MapPost("/orders", [Topic("orderpubsub", "orders")] (Order order) => {
    Console.WriteLine($"Subscriber received : {order}");
    return Results.Ok(order);
});
```

## Step 4: Publish a Topic

In a new terminal window, navigate to [`src/dotnet/sdk/checkout/`](./src/dotnet/sdk/checkout/) and run the following:

```bash
dotnet restore
dotnet build

dapr run --app-id checkout --resources-path ../../components -- dotnet run
```

In the `checkout` publisher, we're publishing the `orderId` message to the Redis instance called `orderpubsub` [(as defined in the `pubsub.yaml` component)](./src/components/pubsub.yaml) and topic `orders`. As soon as the service starts, it publishes in a loop:

[**Program.cs**](./src/dotnet/sdk/checkout/Program.cs#L8)

```cs
using var client = new DaprClientBuilder().Build();
await client.PublishEventAsync("orderpubsub", "orders", order);
Console.WriteLine($"Published data : {order}");
```

## Step 5: View the Pub/Sub Outputs

Notice, as specified in the code above, the publisher pushes orders to the Dapr sidecar while the subscriber receives it.

**Publisher Output**

```
== APP == Published data: Order { OrderId = 1 }
== APP == Published data: Order { OrderId = 2 }
== APP == Published data: Order { OrderId = 3 }
== APP == Published data: Order { OrderId = 4 }
== APP == Published data: Order { OrderId = 5 }
== APP == Published data: Order { OrderId = 6 }
== APP == Published data: Order { OrderId = 7 }
== APP == Published data: Order { OrderId = 8 }
== APP == Published data: Order { OrderId = 9 }
== APP == Published data: Order { OrderId = 10 }
```

**Subscriber Output**  

```
== APP == Subscriber received : Order { OrderId = 1 }       
== APP == Subscriber received : Order { OrderId = 2 }       
== APP == Subscriber received : Order { OrderId = 3 }       
== APP == Subscriber received : Order { OrderId = 4 }       
== APP == Subscriber received : Order { OrderId = 5 }       
== APP == Subscriber received : Order { OrderId = 6 }       
== APP == Subscriber received : Order { OrderId = 7 }       
== APP == Subscriber received : Order { OrderId = 8 }       
== APP == Subscriber received : Order { OrderId = 9 }       
== APP == Subscriber received : Order { OrderId = 10 }
```

## How It Works

When you run `dapr.init`, Dapr creates a default Redis `pubsub.yaml` and runs a Redis container on your lcoal machine, located:

* On Windows, under `$env:userprofile\.dapr\components\pubsub.yaml`
* On Linux/MacOS, under `~/.dapr/components/pubsub.yaml`

With the `pubsub.yaml` component, you can easily swap out underlying components wihtout application code changes.

The Redis [`pubsub.yaml`](./src/components/pubsub.yaml) file included for this quickstart contains:

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: orderpubsub
spec:
  type: pubsub.redis
  version: v1
  metadata:
  - name: redisHost
    value: localhost:6379
  - name: redisPassword
    value: ""
```

In the YAML file:

* `metadata/name` is how your application talks to the component
* `spec/metadata` defines the connection to the instance of the component