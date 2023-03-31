# Quickstart: Workflow

**Get started with the Dapr Workflow building block**  

https://docs.dapr.io/getting-started/quickstarts/workflow-quickstart/

> The workflow building block is currently in **alpha.**

Let's take a look at the Dapr [Workflow building block](https://docs.dapr.io/developing-applications/building-blocks/workflow/). In this quickstart, you'll create a simple console application to demonstrate Dapr's workflow programming model and the workflow management APIs.

The `order-processor` console app starts and manages the lifecycle of the `OrderProcessingWorkflow` workflow that stores and retrieves data in a state store. The workflow consists of four workflow activities, or tasks:

* `NotifyActivity`: Utilizes a logger to print out messages throughout the workflow.
* `ReserveInventoryActivity`: Checks the state store to ensure that there is enough inventory for the purchase.
* `ProcessPaymentActivity`: Processes and authorizes the payment.
* `UpdateInventoryActivity`: Removes the requested items from the state store and updates the store with the new remaining inventory valeu.

In this guide, you'll:

* Run the `order-processor` application.
* Start the workflow and watch the workflow activities / tasks execute.
* Review the workflow logic and the workflow activities and how they're represented in the code.

![workflow](https://docs.dapr.io/images/workflow-quickstart-overview.png)

## Step 1: Pre-requisites

For this example, you will need:

* [Dapr CLI and initialized environment](https://docs.dapr.io/getting-started)
* [.NET SDK or .NET 7 SDK installed](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Step 2: Set up the environment

[Quickstarts repo](https://github.com/dapr/quickstarts/) was cloned and the `workflow/csharp/sdk/` directory was copied to [`src/`](./src/) local to this readme.

## Step 3: Run the order processor app

In a terminal window, navigate to [`src/order-processor/`](./src/order-processor/) and run the following:

```bash
dotnet restore
dotnet build

dapr run --app-id order-processor dotnet run
```

This starts the `order-processor` app with unique workflow ID and runs the workflow activities.

**Output** 

```
== APP == Starting workflow 6d2abcc9 purchasing 10 Cars

== APP == info: Microsoft.DurableTask.Client.Grpc.GrpcDurableTaskClient[40]
== APP ==       Scheduling new OrderProcessingWorkflow orchestration with instance ID '6d2abcc9' and 47 bytes of input data.
== APP == info: WorkflowConsoleApp.Activities.NotifyActivity[0]
== APP ==       Received order 6d2abcc9 for 10 Cars at $15000
== APP == info: WorkflowConsoleApp.Activities.ReserveInventoryActivity[0]
== APP ==       Reserving inventory for order 6d2abcc9 of 10 Cars
== APP == info: WorkflowConsoleApp.Activities.ReserveInventoryActivity[0]
== APP ==       There are: 100, Cars available for purchase

== APP == Your workflow has started. Here is the status of the workflow: Dapr.Workflow.WorkflowState

== APP == info: WorkflowConsoleApp.Activities.ProcessPaymentActivity[0]
== APP ==       Processing payment: 6d2abcc9 for 10 Cars at $15000
== APP == info: WorkflowConsoleApp.Activities.ProcessPaymentActivity[0]
== APP ==       Payment for request ID '6d2abcc9' processed successfully
== APP == info: WorkflowConsoleApp.Activities.UpdateInventoryActivity[0]
== APP ==       Checking Inventory for: Order# 6d2abcc9 for 10 Cars
== APP == info: WorkflowConsoleApp.Activities.UpdateInventoryActivity[0]
== APP ==       There are now: 90 Cars left in stock
== APP == info: WorkflowConsoleApp.Activities.NotifyActivity[0]
== APP ==       Order 6d2abcc9 has completed!

== APP == Workflow Status: Completed
```

## Step 4: View in Zipkin

If you have Zipkin configured for Dapr locally on your machine, you can view the workflow trace spans in the Zipkin web UI at http://localhost:9411/zipkin/.

![image](https://user-images.githubusercontent.com/14102723/229196306-c7de8d8d-4de8-47de-a3a4-4292cb70090c.png)

## How It Works

When you ran `dapr run --app-id order-processor dotnet run`:

1. A unique order ID for the workflow is generated (in the above example, `6d2abcc9`) and the workflow is scheduled.
2. The `NotifyActivity` workflow activity sends a notification saying an order for 10 cars has been received.
3. The `ReserveInventoryActivity` workflow activity checks the inventory data, determines if you can supply the ordered item, and responds with the number of cars in stock.
4. Your workflow starts and notifies you of its status.
5. The `ProcessPaymentActivity` workflow activity begins processing payment for order `6d2abcc9` and confirms if successful.
6. The `UpdateInventoryActivity` workflow activity updates the inventory with the current available cars after the order has been processed.
7. The `NotifyActivity` workflow activity sends a notification saying that order `6d2abcc9` has completed.
8. The workflow terminates as completed.

### `order-processor`

In the application's [**Program.cs**](./src/order-processor/Program.cs):

* The unique workflow ID is generated
* The workflow is scheduled
* The workflow status is retrieved
* The workflow and the workflow activities it invokes are registered

```cs
using Dapr.Client;
using Dapr.Workflow;
//...

{
    services.AddDaprWorkflow(options =>
    {
        // Note that it's also possible to register a lambda function as the workflow
        // or activity implementation instead of a class.
        options.RegisterWorkflow<OrderProcessingWorkflow>();

        // These are the activities that get invoked by the workflow(s).
        options.RegisterActivity<NotifyActivity>();
        options.RegisterActivity<ReserveInventoryActivity>();
        optinos.RegisterActivity<ProcessPaymentActivity>();
        options.RegisterActivity<UpdateInventoryActivity>();
    });
};

//...

// Generate a unique ID for the workflow
string orderId = Guid.NewGuid().ToString()[..8];
string itemToPurchase = "Cars";
int amountToPurchase = 10;

//...

// Start the workflow
Console.WriteLine($"Starting workflow {orderId} purchasing {amountToPurchase} {itemToPurchase}");

await workflowClient.ScheduleNewWorkflowAsync(
    name: nameof(OrderProcessingWorkflow),
    instanceId: orderid,
    input: orderInfo
);

//...

WorkflowState state = await workflowClient.GetWorkflowStateAsync(
    instanceId: orderId,
    getInputsAndOutputs: true
);

Console.WriteLine($"Your workflow has started. Here is the status of the workflow: {state}");

//...

Console.Writeline($"Workflow Status: {state.RuntimeStatus}");
```

In [`OrderProcessingWorkflow.cs`](./src/order-processor/Workflows/OrderProcessingWorkflow.cs), the workflow is defined as a class with all of its associated tasks (determined by workflow activities):

```cs
using Dapr.Workflow;
//...

class OrderProcessingWorkflow : Workflow<OrderPayload, OrderResult>
{
    public override async Task<OrderResult> RunAsync(WorkflowContext context, OrderPayload order)
    {
        string orderId = context.InstanceId;

        // Notify the user than an order has come through
        await context.CallActivityAsync(
            nameof(NotifyActivity),
            new Notification($"Received order {orderId} for {order.Quantity} {order.Name} at ${order.TotalCost}")
        );

        string requestId = context.InstanceId;

        // Determine if there is enough fo the item available for purchase by checking the inventory
        InventoryResult result = await context.CallActivityAsync<InventoryResult>(
            nameof(ReserveInventoryActivity),
            new InventoryRequest(RequestId: orderId, order.Name, order.Quantity)
        );

        // If there is insufficient inventory, fail and let the user know
        if (!result.Success)
        {
            // End the workflow here since we don't have sufficient inventory
            await context.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Insufficient inventory for {order.Name}")
            );

            return new OrderResult(Processed: false);
        }

        // There is enough inventory available so the user can purchase the item(s). Process their payment
        await context.CallActivityAsync(
            nameof(ProcessPaymentActivity),
            new PaymentRequest(RequestId: orderId, order.Name, order.Quantity, order.TotalCost));
        );

        try
        {
            // There is enough inventory available so the user can purcase the item(s). Process their payment
            await context.CallActivityAsync(
                nameof(UpdateInventoryActivity),
                new PaymetnRequest(RequestId: orderId, order.Name, order.Quantity, order.TotalCost)
            );
        }
        catch (TaskFailedException)
        {
            // Let them know their payment failed
            await context.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Order {orderId} Failed! You are now getting a refund")
            );

            return new OrderResult(Processed: false);
        }

        // Let them know their payment was processed
        await context.CallActivityAsync(
            nameof(NotifyActivity),
            new Notifiaction($"Order {orderId} has completed!")
        );

        // End the workflow with a success result
        return new OrderResult(Processed: true);
    }
}
```

The [`Activities`](./src/order-processor/Activities/) directory holds the four workflow activities used by the workflow, defined in the following files:

* [`NotifyActivity.cs`](./src/order-processor/Activities/NotifyActivity.cs)
* [`ReserveInventoryActivity.cs`](./src/order-processor/Activities/ReserveInventoryActivity.cs)
* [`ProcessPaymentActivity`](./src/order-processor/Activities/ProcessPaymentActivity.cs)
* [`UpdateInventoryActivity.cs`](./src/order-processor/Activities/UpdateInventoryActivity.cs)