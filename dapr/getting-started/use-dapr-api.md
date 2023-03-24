# Use the Dapr API

**Run a Dapr sidecar and try out the state API**

https://docs.dapr.io/getting-started/get-started-api/

In this guide, you'll simulate an application by running the sidecar and calling the API directly. After running Dapr using the Dapr CLI, you'll:

* Save a state object
* Read/get the state object
* Delete the state object

## Pre-requisites

1. [Install](./install-dapr-cli.md) the Dapr CLI
2. Run [`dapr init`](./init-dapr-locally.md)

## Exercise

1. Run the Dapr sidecar

    The [`dapr run`](https://docs.dapr.io/reference/cli/dapr-run/) command launches an application, together with a sidecar.
    
    Launch a Dapr sidecar that will listen on port 3500 for a blank application named `myapp`:

    ```bash
    dapr run --app-id myapp --dapr-http-port 3500
    ```

    Since no custom component folder was defined with the above command, Dapr uses the default component definitions created during the [`dapr init` flow](https://docs.dapr.io/getting-started/install-dapr-selfhost/#step-5-verify-components-directory-has-been-initialized).

2. Save state

    Update the state with an object. The new state will look like this:

    ```json
    [
        {
            "key": "name",
            "value": "Bruce Wayne"
        }
    ]
    ```

    Notice, the objects contained in the state each a `key` assigned with the value `name`. You will use the key in the next step.

    **PowerShell**

    ```PowerShell
    Invoke-RestMethod -Method Post -ContentType 'application/json' -Body '[{"key": "name", "value": "Bruce Wayne"}]' -Uri 'http://localhost:3500/v1.0/state/statestore'
    ```

    **Bash**

    ```bash
    curl -X POST -H "Content-Type: application/json" -d '[{"key": "name", "value": "Bruce Wayne"}]' http://localhost:3500/v1.0/state/statestore
    ```

3. Get state

    Retrieve the object you just stored in the state by using the state management API with the key `name`. In the same terminal window, run the following command:

    **PowerShell**

    ```PowerShell
    Invoke-RestMethod -Uri 'http://localhost:3500/v1.0/state/statestore/name'
    ```

    **Bash**

    ```bash
    curl http://localhost:35000/v1.0/state/statestore/name
    ```

    **Output**

    ```
    Bruce Wayne
    ```

4. See how the state is stored in Redis

    Look in the Redis container and verify Dapr is using it as a state store. Use the Redis CLI with the following command:

    ```bash
    docker exec -it dapr_redis redis-cli
    ```

    List the Redis keys to see how Dapr created a key value pair with the app-id you provided to `dapr run` as the key's prefix:

    ```
    keys *
    ```

    > If this command outputs `(empty array)`, it means that you may have another instance of Redis running on port 6379.
    
    > In my case, I had redis installed in WSL, so Dapr was connecting to that instance vs. the instance running in the Docker container.
    
    > I had to run `dapr uninstall`, remove redis from WSL, then re-run `dapr init` for this section to work.

    **Output**:

    ```
    1) "myapp||name"
    ```

    View the state values by running:

    ```bash
    hgetall "myapp||name"
    ```

    **Output**:

    ```
    1) "data"
    2) "\"Bruce Wayne\""
    3) "version"
    4) "1"
    ```

    Exit the Redis CLI:

    ```bash
    exit
    ```

5. Delete state

    In the same terminal window, delete the `name` state object from the state store.

    **PowerShell**

    ```PowerShell
    Invoke-RestMethod -Method Delete -ContentType 'application/json' -Uri 'http://localhost:3500/v1.0/state/statestore/name'
    ```

    **Bash**

    ```bash
    curl -v -X DELETE -H "Content-Type: application/json" http://localhost:3500/v1.0/state/statestore/name
    ```