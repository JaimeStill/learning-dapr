# Initialize Dapr in Your Local Environment

**Fetch the Dapr sidecar binaries and install them locally using `dapr init`**

https://docs.dapr.io/getting-started/install-dapr-selfhost/

> See [disconnected-notes](../../disconnected-notes/) to capture / review details regarding initializing Dapr in an environment with no internet.

Now that you've [installed the Dapr CLI](./install-dapr-cli.md), use the CLI to initialize Dapr on your local machine.

Dapr runs a sidecar alongside your application. In self-hosted mode, this means it is a process on your local machine. By initializing Dapr, you:

* Fetch and install the Dapr sidecar binaries locally.
* Create a development environment that streamlines application development with Dapr.

Dapr initialization includes:

1. Running a **Redis container instance** to be used as a local state store and message broker.
2. Running a **Zipkin container instance** for observability.
3. Creating a **default components folder** with component definitions for the above.
4. Running a **Dapr placement service container instance** for local actor support.

## Initialize Dapr

1. Open an elevated terminal
    * On Windows, open a terminal as administrator.    
    * On Linux / MacOS, ensure you have `sudo` permissions.

2. Run the init CLI command:

    ```bash
    dapr init
    ```

3. Verify Dapr version

    ```bash
    dapr --version
    ```

    **Output:**

    ```
    CLI version: 1.10.0
    Runtime version: 1.10.4
    ```

4. Verify containers are running

    ```bash
    docker ps
    ```

    **Output**

    > Simplified output for brevity

    IMAGE | COMMAND | PORTS | NAMES
    ------|---------|-------|------
    **daprio/dapr:1.10.4** | *"./placement"* | `0.0.0.0:6050->50005/tcp` | `dapr_placement`
    **openzipkin/zipkin** | *"start-zipkin"* | `9410/tcp, 0.0.0.0:9411->9411/tcp` | `dapr_zipkin`
    **redis:6** | *"docker-entrypoint.sh redis-server"* | `0.0.0.0:6379->6379/tcp` | `dapr_redis`

5. Verify components directory has been initialized

    On `dapr init`, the CLI also creates a default components folder that contains several YAML files with definitions for a state store, Pub/sub, and Zipkin. The Dapr sidecar will read these components and use:

    * The Redis container for state management and messaging.
    * The Zipkin container for collecting traces.

    Verify by opening your components directory:

    * Windows: `$env:userprofile\.dapr`
    * Linux/MacOS: `~/.dapr`

    **Contents**

    ```
    bin components config.yaml
    ```