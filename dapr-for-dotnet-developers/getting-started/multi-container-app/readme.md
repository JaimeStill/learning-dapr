# Multi Container Dapr App

https://learn.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/getting-started#build-a-multi-container-dapr-application

> Note that this example was all built using the CLI and VS Code. It will differ slightly from what is presented in the linked instructions.

To run:

```bash
multi-container-app> docker-compose build
multi-container-app> docker-compose up
```

Relevant Files:

* [`Dapr.AspNetCore`](./MyFrontEnd/MyFrontEnd.csproj#L10) - Package registration
* [`Program.cs`](./MyFrontEnd/Program.cs#L4) - `.AddDaprClient()`
* [`WeatherForecast.cs`](./MyFrontEnd/WeatherForecast.cs)
* [`Index.cshtml.cs`](./MyFrontEnd/Pages/Index.cshtml.cs)
* [`Index.cshtml`](./MyFrontEnd/Pages/Index.cshtml)
* [MyFrontEnd - `Dockerfile`](./MyFrontEnd/Dockerfile)
* [MyBackEnd - `Dockerfile`](./MyBackEnd/Dockerfile)
* [`.dockerignore`](./.dockerignore)
* [`docker-compose.yml`](./docker-compose.yml)

To use Dapr building blocks from inside a containerized application, you'll need to add the Dapr sidecards containers to your Compose file:

[`docker-compose.yml`](./docker-compose.yml)

```yaml
version: '3.4'

services:
  myfrontend:
    image: ${DOCKER_REGISTRY-}myfrontend
    build:
      context: .
      dockerfile: MyFrontEnd/Dockerfile
    ports:
      - "51000:50001"
      - "5100:80"

  myfrontend-dapr:
    image: "daprio/daprd:latest"
    command: ["./daprd", "-app-id", "MyFrontEnd", "-app-port", "80"]
    depends_on:
      - myfrontend
    network_mode: "service:myfrontend"
  
  mybackend:
    image: ${DOCKER_REGISTRY-}mybackend
    build:
      context: .
      dockerfile: MyBackEnd/Dockerfile
    ports:
      - "52000:50001"
      - "5200:80"

  mybackend-dapr:
    image: "daprio/daprd:latest"
    command: ["./daprd", "-app-id", "MyBackEnd", "-app-port", "80"]
    depends_on:
      - mybackend
    network_mode: "service:mybackend"
```

In the updated file, the `myfrontend-dapr` and `mybackend-dapr` sidecards for `myfrontend` and `mybackend` have been added.

* The sidecars use the `daprio/daprd:latest` container image. The use of the `latest` tag isn't recommended for production scenarios. Instead, use a specific version number.

* Each service defined in the Compose file has its own network namespace for network isolation purposes. The sidecars use `network_mode: "service:..."` to ensure they run in the same network namespace as the application. Doing so allows the sidecar and the application to communicate using `localhost`.

* The ports on which the Dapr sidecars are listening for gRPC communication (by default, 50001) must be exposed to allow the sidecars to communicate with each other.

* The Web App and API HTTP ports must be specified, or they will default to :80 by default.