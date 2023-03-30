# Quickstart: State Management

**Get started with Dapr's State Management building block**  

https://docs.dapr.io/getting-started/quickstarts/statemanagement-quickstart/

Let's take a look at Dapr's [State Management building block](https://docs.dapr.io/developing-applications/building-blocks/state-management/). In this Quickstart, you will save, get, and delete state using a Redis state store, but you can swap this out for any one of the [supported state stores](https://docs.dapr.io/reference/components-reference/supported-state-stores/).

![state-management](https://docs.dapr.io/images/state-management-quickstart.png)

## Pre-requisites

For this example, you will need:

* [Dapr CLI and initialized environment](https://docs.dapr.io/getting-started)
* [.NET SDK or .NET 7 SDK installed](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Step 1: Set up the environment

[Quickstarts repo](https://github.com/dapr/quickstarts/) was cloned and the `state_management/csharp/sdk/` directory was copied to [`src/dotnet/sdk/`](./src/dotnet/sdk/) local to this readme. Additionally, `state_management/resources` was copied to [`src/resources`](./src/resources/).

## Step 2: Manipulate Service State

In a terminal window, navigate to [`src/dotnet/sdk/order-processor`](./src/dotnet/sdk/order-processor/) and run the following:

```bash
dotnet restore
dotnet build

dapr run --app-id order-processor --resources-path ../../../resources/ -- dotnet run
```

The `order-processor` service writes, reads, and deletes an `orderId` key/value pair to the `statestore` instance [defined in the `statestore.yaml` component](./src/resources/statestore.yaml). As soon as the service starts, it performs a loop. See [**Program.cs**](./src/dotnet/sdk/order-processor/Program.cs).

```cs
var client = new DaprClientBuilder().Build();

// Save state into the state store
await client.SaveStateAsync(DAPR_STORE_NAME, orderId.ToString(), order.ToString());
Console.WriteLine($"Saving Order: {order}");

// Get state from teh state store
var result = await client.GetStateAsync<strign>(DAPR_STORE_NAME, orderId.ToString());
Console.WriteLine($"Getting Order: {result}");

// Delete state from the state store
await client.DeleteStateAsync(DAPR_STORE_NAME, orderId.ToString());
Console.WriteLine($"Deleting Order: {order}");
```

**Output**  

```
== APP == Saving Order: Order { orderId = 1 }
== APP == Getting Order: Order { orderId = 1 }
== APP == Deleting Order: Order { orderId = 1 }
== APP == Saving Order: Order { orderId = 2 }
== APP == Getting Order: Order { orderId = 2 }
== APP == Deleting Order: Order { orderId = 2 }
== APP == Saving Order: Order { orderId = 3 }
== APP == Getting Order: Order { orderId = 3 }
== APP == Deleting Order: Order { orderId = 3 }
== APP == Saving Order: Order { orderId = 4 }
== APP == Getting Order: Order { orderId = 4 }
== APP == Deleting Order: Order { orderId = 4 }
== APP == Saving Order: Order { orderId = 5 }
== APP == Getting Order: Order { orderId = 5 }
== APP == Deleting Order: Order { orderId = 5 }
```

## How It Works

When you run `dapr init`, Dapr creates a default Redis `statestore.yaml` and runs a Redis container on your local machine, located:

* On Windows, under `$env:userprofile\.dapr\components\statestore.yaml`
* On Linux/MacOS, under `~/.dapr/components/statestore.yaml`

With the `statestore.yaml` component, you can easily swap out the [state store]() without making code changes.

The Redis [`statestore.yaml`](./src/resources/statestore.yaml) file included for this quickstart contains the following:

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: statestore
spec:
  type: state.redis
  version: v1
  metadata:
  - name: redisHost
    value: localhost:6379
  - name: redisPassword
    value: ""
  - name: actorStateStore
    value: "true"
```

In the YAML file:

* `metadata/name` is how your application talks to the component (called `DAPR_STORE_NAME` in the code sample).
* `spec/metadata` defines the connection to the Redis instance used by the component.