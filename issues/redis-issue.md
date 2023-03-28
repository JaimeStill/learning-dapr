# Repro Dapr Redis Source Issue

## What is the area?

/area runtime

## What version of Dapr?
CLI version: 1.10.0
Runtime Version: 1.10.4

## Expected Behavior

When initializing Dapr via `dapr --init` and interfacing with the Redis store component, Dapr should be interfacing with the instance initialized in the Docker container.

## Actual Behavior

If Redis is already installed and running somewhere else on the machine, Dapr will interface with the previously installed instance of Redis vs. the Docker container initialized by Dapr.

## Steps to Reproduce the Problem

On my Windows 11 machine, I already had Redis installed in WSL prior to installing and initializing Dapr. These steps will assume no artifacts of Dapr previously exist

### Install Redis in WSL Ubuntu

```bash
sudo snap install redis
```

### Install the Dapr CLI

> From an Administrative terminal

```PowerShell
pwsh -Command "iwr -useb https://raw.githubusercontent.com/dapr/cli/master/install/install.ps1 | iex"
```

### Initialize Dapr and start a Dapr app

> From an Administrative terminal

```PowerShell
dapr init

dapr run --app-id myapp --dapr-http-port 3500
```

### Store and Retrieve Data in Dapr State Store

```PowerShell
# POST data
Invoke-RestMethod -Method Post -ContentType 'application/json' -Body '[{"key": "name", "value": "Bruce Wayne"}]' -Uri 'http://localhost:3500/v1.0/state/statestore'

# GET data
# Output: Bruce Wayne
Invoke-RestMethod -Uri 'http://localhost:3500/v1.0/state/statestore/name'
```

### Connect to dapr_redis Container and Check Data

> The following output should print (empty array)
> for all executed redis commands

```bash
docker exec -it dapr_redis redis-cli

127.0.0.1:6370>keys *
(empty array)

127.0.0.1:6370>hgetall "myapp||name"
(empty array)
```

### Connect to WSL Redis Instance and Check Data

**Output**  

```bash
# connect to WSL
wsl

# make sure redis-cli is installed
sudo apt install redis-tools

# connect to redis
~$ redis-cli

# check for keys
127.0.0.1:6379> keys *
1) "myapp||name"

# check value
127.0.0.1:6379> hgetall "myapp||name"
1) "data"
2) "\"Bruce Wayne\""
3) "version"
4) "1"
```

### Remediate Problem

I honestly had forgotten that I installed Redis in WSL at an earlier point and spent a long time trying to figure out what I had done wrong. Nothing about the scenario made sense until I remembered that I had installed it. Data was able to be stored and retrieved via the Dapr REST methods, but I could not find any trace of it in the **darp_redis**.

Once I understood what was happening, I was able to get everything working after the following steps:

1. Remove all Redis artifacts from Ubuntu

    ```bash
    # connect to WSL Ubuntu
    wsl

    # remove redis-tools
    sudo apt remove redis-tools

    # exit WSL
    exit

    # restart WSL
    wsl --shutdown Ubuntu

    # connect again
    wsl

    # remove redis
    sudo snap remove redis --purge

    # exit wsl
    exit
    ```

2. Purge / Clean Docker for Desktop and Restart.

3. From an administrative command prompt:

    ```PowerShell
    # uninstall Dapr
    dapr uninstall

    # reinit Dapr
    dapr init
    ```

4. Start the Dapr app

    ```PowerShell
    dapr run --app-id myapp --dapr-http-port 3500
    ```

5. Store and Retrieve State Store Data

    ```PowerShell
     Invoke-RestMethod -Method Post -ContentType 'application/json' -Body '[{"key": "name", "value": "Bruce Wayne"}]' -Uri 'http://localhost:3500/v1.0/state/statestore'

    Invoke-RestMethod -Uri 'http://localhost:3500/v1.0/state/statestore/name'
    ```

    **Output**

    ```
    Bruce Wayne
    ```

6. Connect to **dapr_redis** and verify data:

    ```
    > docker exec -it dapr_redis redis-cli

    127.0.0.1:6379> keys *
    1) "myapp||name"

    127.0.0.1:6379> hgetall "myapp||name"
    1) "data"
    2) "\"Bruce Wayne\""
    3) "version"
    4) "1"
    ```