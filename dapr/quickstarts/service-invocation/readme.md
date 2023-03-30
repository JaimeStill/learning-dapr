# Quickstart: Service Invocation

**Get started with Dapr's Service Invocation building block**  

https://docs.dapr.io/getting-started/quickstarts/serviceinvocation-quickstart/

With [Dapr's Service Invocation building block](https://docs.dapr.io/developing-applications/building-blocks/service-invocation), your application can communicate reliably and securely with other applications.

![service-invocation](https://docs.dapr.io/images/serviceinvocation-quickstart/service-invocation-overview.png)

Dapr offers several methods for service invocation, which you can choose depending on your scenario. For this Quickstart, you'll enable the checkout service to invoke a method using HTTP proxy in the order-processor service. Learn more about Dapr's methods for service invocation in the [overview article](https://docs.dapr.io/developing-applications/building-blocks/service-invocation/service-invocation-overview/).

## Step 1: Pre-requisites

For this example, you will need:

* [Dapr CLI and initialized environment](https://docs.dapr.io/getting-started)
* [.NET SDK or .NET 7 SDK installed](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Step 2: Set up the environment

[Quickstarts repo](https://github.com/dapr/quickstarts/) was cloned and the `service_invocation/csharp/http` directory was copied to [`src/dotnet/`](./src/dotnet/) local to this readme.

## Step 3: Run `order-processor` service

Change directory to [`./src/dotnet/order-processor`](./src/dotnet/order-processor/) and run:

```bash
dotnet restore
dotnet build

dapr run --app-port 7001 --app-id order-processor --app-protocol http --dapr-http-port 3501 -- dotnet run
```

See working code from the order processor's [**Program.cs**](./src/dotnet/order-processor/Program.cs#L10).

## Step 4: Run `checkout` service

In a new terminal window, change directory to [`./src/dotnet/checkout`](./src/dotnet/checkout) and run:

```bash
dotnet restore
dotnet build

dapr run --app-id checkout --app-protocol http --dapr-http-port 3500 -- dotnet run
```

In the [**Program.cs**](./src/dotnet/checkout/Program.cs#L7) file for the `checkout` service, you'll notice there's no need to rewrite your app code to use Dapr's service invocation. You can enable service invocation simply by adding the `dapr-app-id` header, which specifies the ID of the target service:

```cs
client.DefaultRequestHeaders.Add("dapr-app-id", "order-processor");
```

**Output**

```
== APP == Order passed: Order { OrderId = 1 }
== APP == Order passed: Order { OrderId = 2 }
== APP == Order passed: Order { OrderId = 3 }
== APP == Order passed: Order { OrderId = 4 }
== APP == Order passed: Order { OrderId = 5 }
== APP == Order passed: Order { OrderId = 6 }
== APP == Order passed: Order { OrderId = 7 }
== APP == Order passed: Order { OrderId = 8 }
== APP == Order passed: Order { OrderId = 9 }
== APP == Order passed: Order { OrderId = 10 }
== APP == Order passed: Order { OrderId = 11 }
== APP == Order passed: Order { OrderId = 12 }
== APP == Order passed: Order { OrderId = 13 }
== APP == Order passed: Order { OrderId = 14 }
== APP == Order passed: Order { OrderId = 15 }
== APP == Order passed: Order { OrderId = 16 }
== APP == Order passed: Order { OrderId = 17 }
== APP == Order passed: Order { OrderId = 18 }
== APP == Order passed: Order { OrderId = 19 }
== APP == Order passed: Order { OrderId = 20 }
```

In the running `order-processor` service, you should see the following output:

**Output**

```
== APP == Order received : Order { orderId = 1 }
== APP == Order received : Order { orderId = 2 }
== APP == Order received : Order { orderId = 3 }
== APP == Order received : Order { orderId = 4 }
== APP == Order received : Order { orderId = 5 }
== APP == Order received : Order { orderId = 6 }
== APP == Order received : Order { orderId = 7 }
== APP == Order received : Order { orderId = 8 }
== APP == Order received : Order { orderId = 9 }
== APP == Order received : Order { orderId = 10 }
== APP == Order received : Order { orderId = 11 }
== APP == Order received : Order { orderId = 12 }
== APP == Order received : Order { orderId = 13 }
== APP == Order received : Order { orderId = 14 }
== APP == Order received : Order { orderId = 15 }
== APP == Order received : Order { orderId = 16 }
== APP == Order received : Order { orderId = 17 }
== APP == Order received : Order { orderId = 18 }
== APP == Order received : Order { orderId = 19 }
== APP == Order received : Order { orderId = 20 }
```

## Step 5: Use with Multi-App Run

> The following only works in Linux / MacOS, but can be run in Windows via WSL. You will need to install Dapr CLI in WSL, then run `dapr uninstall` followed by `dapr init` to initialize the runtime for WSL as well. This will not affect your Windows Dapr initialization.

You can run the Dapr applications in this quickstart with the [Multi-App Run template](). Instead of running two separate `dapr run` commands for the `order-processor` and `checkout` applications, run the following command from the root of [`./src/dotnet`](./src/dotnet/):

```bash
dapr run -f .
```

This will run the services defined in [`./src/dotnet/dapr.yaml`](./src/dotnet/dapr.yaml):

```yaml
version: 1
apps:
  - appDirPath: ./order-processor/
    appID: order-processor
    appPort: 7001
    command: ["dotnet", "run"]
  - appID: checkout
    appDirPath: ./checkout/
    command: ["dotnet", "run"]
```