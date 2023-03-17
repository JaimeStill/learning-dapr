# Getting Started

https://learn.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/getting-started

This document will guide you through preparing your local development environment and building two Dapr .NET applications.

## Install Dapr to Local Environment

Once this section is complete, you can build and run Dapr aplpications in [self-hosted mode](https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-overview/).

1. [Install the Dapr CLI](). It enables you to launch, run, and manage Dapr instances. It also provides debugging support.

    ```PowerShell
    # install the latest Windows Dapr CLI
    # to $env:SystemDrive\dapr and add
    # the directory to the User PATH
    pwsh -Command "iwr -useb https://raw.githubusercontent.com/dapr/cli/master/install/install.ps1 | iex"
    ```

    When the command completes, you should see the following messages:

    ```
    Dapr CLI is installed successfully.
    To get started with Dapr, please visit https://docs.dapr.io/getting-started/ .
    Ensure that Docker Desktop is set to Linux containers mode when you run Dapr in self hosted mode.
    ```

2. Install [Docker Desktop](https://docs.docker.com/get-docker/). If you're running on Windows, make sure that **Docker Desktop for Windows** is configured to use Linux containers.

3. [Initialize Dapr](https://docs.dapr.io/getting-started/install-dapr-selfhost/). This step sets up your development environment by installing the latest Dapr binaries and container images.

    1. Open an elevated terminal

    2. Run the init CLI command:

        ```bash
        dapr init
        ```

    3. Verify Dapr version:

        ```bash
        dapr --version
        ```

        Output:

        ```
        CLI version: 1.10.0
        Runtime version: 1.10.4
        ```

    4. Verify containers are running:

        ```bash
        docker ps
        ```

        Output:

        > Only showing select columns for simplicity
        
        IMAGE | COMMAND | PORTS | NAMES
        ------|---------|-------|-------
        daprio/dapr:1.10.4 | "./placement" | 0.0.0:6050->50005/tcp | dapr_placement
        openzipkin/zipkin | "start-zipkin" | 9410/tcp, 0.0.0:9411->9411/tcp | dapr_zipkin
        redis:6 | "docker-entrypoint.s..." | 0.0.0:6379->6379/tcp | dapr_redis

    5. Verify components directory has been initialized:

        ```PowerShell
        ls $env:userprofile\.dapr\
        ```

        Output:
        
        Mode | LastWriteTime | Length | Name
        -----|---------------|--------|-----
        d---- | 3/17/2023 09:30 | | bin
        d---- | 3/17/2023 09:30 | | components
        -a--- | 3/17/2023 09:30 | 187 | config.yaml

4. Install the [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0).