# Quickstart: Input & Output Bindings

**Get started with Dapr's Binding building block**  

https://docs.dapr.io/getting-started/quickstarts/bindings-quickstart/

Let's take a look at Dapr's [Bindings building block](https://docs.dapr.io/developing-applications/building-blocks/bindings/). Using bindings, you can:

* Trigger your app with events coming in from external systems.
* Interface with external systems.

In this Quickstart, you will schedule a batch script to run every 10 seconds using an input [Cron](https://docs.dapr.io/reference/components-reference/supported-bindings/cron/) binding. The script processes a JSON file and outputs data to a SQL database using the [PostgreSQL](https://docs.dapr.io/reference/components-reference/supported-bindings/postgres) Dapr binding.

![bindings](https://docs.dapr.io/images/bindings-quickstart/bindings-quickstart.png)

## Pre-requisites

For this example, you will need:

* [Dapr CLI and initialized environment](https://docs.dapr.io/getting-started)
* [.NET SDK or .NET 7 SDK installed](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Step 1: Set up the environment

[Quickstarts repo](https://github.com/dapr/quickstarts/) was cloned and the `bindings/csharp/sdk/` directory was copied to [`src/dotnet/sdk/`](./src/dotnet/sdk/) local to this readme. Additionally, `bindings/components` was copied to [`src/components`](./src/components/), `bindings/db` was copied to [`src/db`](./src/db/), and `bindings/orders.json` was copied to [`src/dotnet/sdk/batch/orders.json`](./src/dotnet/sdk/batch/orders.json).

Modified [`src/dotnet/sdk/batch.program.cs`](./src/dotnet/sdk/batch/program.cs#L36) as follows:

```cs
// initial
string jsonFile = File.ReadAllText("../../../orders.json");

// adjustment
string jsonFile = File.ReadAllText("./orders.json");
```

## Step 2: Run PostgreSQL Docker container locally

Run the [PostgreSQL instance](https://www.postgresql.org/) locally in a Docker container on your machine. The quickstart sample includes a [Docker Comopse file](./src/db/docker-compose.yml) to locally customize, build, run, and initialize the `postgres` container with a default `orders` table.

In a terminal window, navigate to [`src/db`](./src/db/) and run the following:

``` bash
docker compose up
```

In a separate terminal, verify that the container is running locally:

```bash
docker ps
```

**Output**

> Shortened for brevity

IMAGE | PORTS | NAMES
------|-------|------
samples/postgres | 0.0.0.0:5432->5432/tcp | postgres

## Step 3: Schedule a Cron job and write to the database

In a terminal window, navigate to [`src/dotnet/sdk/batch`](./src/dotnet/sdk/batch/) and run the following:

```bash
dotnet restore
dotnet build

dapr run --app-id batch-sdk --app-port 7002 --resources-path ../../compoennts -- dotnet run
```

The code inside the `process_batch` function is executed every 10 seconds (defined in [`binding-cron.yaml`](./src/components/binding-cron.yaml) in the `components` directory). The binding trigger looks for a route called via HTTP POST in your application by the Dapr sidecar.

[**Program.cs**](./src/dotnet/sdk/batch/program.cs#L33)

```cs
app.MapPost($"/{cronBindingName}", async () => {
    //...
})
```

The `batch-sdk` service uses the PostgreSQL output binding defined in the [`binding-postgres.yaml`](./src/components/binding-postgres.yaml) component to insert the `OrderId`, `Customer`, and `Price` records into the `orders` table.

[**Program.cs**](./src/dotnet/sdk/batch/program.cs#L36)

```cs
// ...
string jsonfile = File.ReadAllText("./orders.json");
var ordersArray = JsonSerializer.Deserialize<Orders>(jsonFile);
using var client = new DaprClientBuilder().Build();
foreach (Order ord in ordersArray?.orders ?? new Order[] {}) {
    var sqlText = $"insert into orders (orderid, customer, price) values ({ord.OrderId}, '{ord.Customer}', {ord.Price});";
    var command = new Dictionary<string, string>() {
        { "sql", sqlText }
    };
    // ...

    await client.InvokeBindingAsync(bindingName: sqlBindingName, operation: "exec", data: "", metadata: command);
}
```

## Step 4: View the output of the job

Notice, as specified above, the code invokes the output binding with the `OrderId`, `Customer`, and `Price` as a payload.

**Output**

```
== APP == Processing batch..
== APP == insert into orders (orderid, customer, price) values (1, 'John Smith', 100.32);
== APP == insert into orders (orderid, customer, price) values (2, 'Jane Bond', 15.4);
== APP == insert into orders (orderid, customer, price) values (3, 'Tony James', 35.56);
== APP == Finished processing batch
```

In a new terminal, verify the same data has been inserted into the database. Navigate to [`src/db`](./src/db/) and run the following:

> Be sure the terminal that exectued `docker compose up` in the step above is still up and running

```
# start interactive Postgres CLI
docker exec -i -t postgres psql --username postgres -p 5432 -h localhost --no-password

# change to the orders table
\c orders;

# select all rows
select * from orders;
```

**Output**

```
 orderid |  customer  | price
---------+------------+--------
       1 | John Smith | 100.32
       2 | Jane Bond  |   15.4
       3 | Tony James |  35.56
```

## How It Works

### Cron

When you execute the `dapr run` command and specify the component path, the Dapr sidecar:

* Initializes the Cron [binding building block](https://docs.dapr.io/developing-applications/building-blocks/bindings/)
* Calls the binding endpoint (`batch`) every 10 seconds

The Cron [`binding-cron.yaml`](./src/components/binding-cron.yaml) file included for this quickstart contains the following:

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: cron
  namespace: quickstarts
spec:
  type: bindings.cron
  version: v1
  metadata:
  - name: schedule
    value: "@every 10s" # valid cron schedule
```

**Note:** The `metadata` section of the `binding-cron.yaml` contains a [Cron expression](https://docs.dapr.io/reference/components-reference/supported-bindings/cron/) that specifies how often the binding is invoked.

### PostgreSQL

When you execute the `dapr run` command and specify the component path, the Dapr sidecar:
* Initiatest he PostgreSQL [binding building block](https://docs.dapr.io/reference/components-reference/supported-bindings/postgres/)
* Connects to PostgreSQL using the settings specified in the `binding-postgres.yaml` file

With the `binding-postgres.yaml` component, you can easily swap out the backend database [binding](https://docs.dapr.io/reference/components-reference/supported-bindings/) without making code changes.

The PostgreSQL [`binding-postgres.yaml`](./src/components/binding-postgres.yaml) file included for this quickstart contains the following:

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: sqldb
  namespace: quickstarts
spec:
  type: bindings.postgres
  version: v1
  metadata:
  - name: url # Required
    value: "user=postgres password=docker host=localhost port=5432 dbname=orders pool_min_conns=1 pool_max_conns=10"
```

In the YAML file:

* `spec/type` specifies that PostgreSQL is used for this binding.
* `spec/metadata` defines the connection to the PostgreSQL instance used by the component.