# Quickstart: Configuration

**Get started with Dapr's Configuration building block**

https://docs.dapr.io/getting-started/quickstarts/configuration-quickstart/

Let's take a look at Dapr's [Configuration building block](https://docs.dapr.io/developing-applications/building-blocks/configuration/configuration-api-overview/). A configuration item is often dynamic in nature and tightly coupled to the needs of the application that consumes it. Configuration items are key / value pairs containing configuration data, such as:

* App ids
* Partition keys
* Database names, etc.

In this quickstart, you'll run an `order-processor` microservice that utilizes the Configuration API. The service:

1. Gets configuration items from the configuration store.
2. Subscribes for configuration updates.

![configuration](https://docs.dapr.io/images/configuration-quickstart/configuration-quickstart-flow.png)

## Pre-requisites

For this example, you will need:

* [Dapr CLI and initialized environment](https://docs.dapr.io/getting-started)
* [.NET SDK or .NET 7 SDK installed](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Step 1: Set up the environment

[Quickstarts repo](https://github.com/dapr/quickstarts/) was cloned and the `configuration/csharp/sdk/` directory was copied to [`src/dotnet/sdk/`](./src/dotnet/sdk/) local to this readme. Additionally, `configuration/components` was copied to [`src/components`](./src/components/).

Once copied, open a new terminal and run the following command to set values for configuration items `orderId1` and `orderId2`:

```
docker exec dapr_redis redis-cli MSET orderId1 "101" orderId2 "102"
```

**Output**

```
OK
```

## Step 2: Run the `order-processor` service

In a terminal window, navigate to [`src/dotnet/sdk/order-processor`](./src/dotnet/sdk/order-processor/) and run the following:

```
dotnet restore
dotnet build

dapr run --app-id order-processor-http --resources-path ../../../components/ --app-port 7001 -- dotnet run
```

**Output**

```
== APP == Configuration for orderId1: {"Value":"101","Version":"","Metadata":{}}
== APP == Configuration for orderId2: {"Value":"102","Version":"","Metadata":{}}
== APP == App subscribed to config changes with subscription id: 615d1a6f-6535-4442-b195-0b7ff7205f05
== APP == App unsubscribed from config changes
```

## Step 3: Update Configuration Item Values

Once the app has unsubscribed in the above step, run the process again and update the configuration item values before the unsubscribe occurs.

Run the `order-processor` service again:

```
dapr run --app-id order-processor-http --resources-path ../../../components/ --app-port 7001 -- dotnet run
```

Change the `orderId1` and `orderId2` values using the following command:

```
docker exec dapr_redis redis-cli MSET orderId1 "103" orderId2 "104"
```

**Output**

```
OK
```

The app will output the initial configuration values, then output the updates to those configuration values:

```
== APP == Configuration for orderId1: {"Value":"101","Version":"","Metadata":{}}
== APP == Configuration for orderId2: {"Value":"102","Version":"","Metadata":{}}
== APP == App subscribed to config changes with subscription id: 3e579c6c-a614-4839-bbf6-57ebf571a5cb
== APP == Configuration update {"orderId1":{"Value":"103","Version":"","Metadata":{}}}   
== APP == Configuration update {"orderId2":{"Value":"104","Version":"","Metadata":{}}}
```

## How It Works

The `order-processor` service includes code for:

* Getting the configuration items from the config store
* Subscribing to configuration updates (which you made in the CLI earlier)
* Unsubscribing from configuration updates and exiting the app after 20 seconds of inactivity

Get configuration items:

[**Program.cs**](./src/dotnet/sdk/order-processor/Program.cs#L13)

```cs
// Get config from configuration store
GetConfigurationResponse config = await client.GetConfiguration(DAPR_CONFIGURATION_STORE, CONFIGURATION_ITEMS);
foreach (var item in config.Items)
{
    var cfg = System.Text.Json.JsonSerializer.Serialize(item.Value);
    Console.WriteLine($"Configuration for {item.Key}: {cfg}");
}
```

Subscribe to configuration updates:

[**Program.cs**](./src/dotnet/sdk/order-processor/Program.cs#L27)

```cs
// Subscribe to config updates
SubscribeConfigurationResopnse subscribe = await client.SubscribeConfiguration(DAPR_CONFIGURATION_STORE, CONFIGURATION_ITEMS);
```

Unsubscribe from configuration udpates and exit the application:

[**Program.cs**](./src/dotnet/sdk/order-processor/Program.cs#L49)

```cs
// Unsubscribe to config updates and exit the app
await client.UnsubscribeConfiguration(DAPR_CONFIGURATION_STORE, subscriptionId);
Console.WriteLine("App unsubscribed from config changes");
Enviornment.Exit(0);
```