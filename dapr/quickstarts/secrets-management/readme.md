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

[Quickstarts repo](https://github.com/dapr/quickstarts/) was cloned and the `secrets_managemetn/csharp/sdk/` directory was copied to [`src/dotnet/sdk/`](./src/dotnet/sdk/) local to this readme. Additionally, `bindings/components` was copied to [`src/components`](./src/components/).