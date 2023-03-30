# Quickstart: Secrets Management

**Get started with Dapr's Secrets Management building block**  

https://docs.dapr.io/getting-started/quickstarts/secrets-quickstart/

Dapr provides a dedicated secrets API that allows developers to retrieve secrets from a secrets store. In this quickstart, you:

1. Run a microservice with a secret store component.
2. Retrieve secrets using the Dapr secrets API in the application code.

![secrets](https://docs.dapr.io/images/secretsmanagement-quickstart/secrets-mgmt-quickstart.png)

## Pre-requisites

For this example, you will need:

* [Dapr CLI and initialized environment](https://docs.dapr.io/getting-started)
* [.NET SDK or .NET 7 SDK installed](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Step 1: Set up the environment

[Quickstarts repo](https://github.com/dapr/quickstarts/) was cloned and the `secrets_management/csharp/sdk/` directory was copied to [`src/dotnet/sdk/`](./src/dotnet/sdk/) local to this readme. Additionally, `bindings/components` was copied to [`src/components`](./src/components/).

## Step 2: Retrieve the secret

In a terminal window, navigate to `src/dotnet/sdk/order-processor` and execute the following:

```bash
dotnet restore
dotnet build

dapr run --app-id order-processor --resources-path ../../../components/ -- dotnet run
```

**Output**  

```
== APP == Fetched Secret: [secret, YourPasskeyHere]
```

## How It Works

### `order-processor` service

Notice how the `order-processor` service in [**Program.cs**](./src/dotnet/sdk/order-processor/Program.cs#L4) points to:

* The `DAPR_SECRET_STORE` defined in the [`local-secret-store.yaml`](./src/components/local-secret-store.yaml) component.
* The secret defined in [`secrets.json`](./src/dotnet/sdk/order-processor/secrets.json).

```cs
const string DAPR_SECRET_STORE = "localsecretstore";
const string SECRET_NAME = "secret";
var client = new DaprClientBuilder().Build();

var secret = await client.GetSecretAsync(DAPR_SECRET_STORE, SECRET_NAME);
var secretValue = string.Join(", ", secret);
Console.WriteLine($"Fetched Secret: {secretValue}");
```

### `local-secret-store.yaml` component

`DAPR_SECRET_STORE` is defined in the [`local-secret-store.yaml`](./src/components/local-secret-store.yaml) component file:

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: localsecretstore
  namespace: default
spec:
  type: secretstores.local.file
  version: v1
  metadata:
  - name: secretsFile
    value: secrets.json
  - name: nestedSeparator
    value: ":"
```

In the YAML file:

* `metadata/name` is how your application references the component (called `DAPR_SECRET_NAME` in the code sample).
* `spec/metadata` defines the connection to the secret used by the component.

### `secrets.json` file

`SECRET_NAME` is defined in the [`secrets.json`](./src/dotnet/sdk/order-processor/secrets.json) file:

```
{
    "secret": "YourPasskeyHere"
}
```