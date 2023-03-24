# Install the Dapr CLI

**Install the Dapr CLI as the main tool for running Dapr-related tasks**  

https://docs.dapr.io/getting-started/install-dapr-cli/

You'll use the Dapr CLI as the main tool for various Dapr-related tasks. You can use it to:

* Run an application with a Dapr sidecar
* Review sidecar logs
* List running services
* Run the Dapr dashboard

## Step 1: Install the Dapr CLI

**Linux**  

```bash
wget -q https://raw.githubusercontent.com/dapr/cli/maters/install/install.sh -O - | /bin/bash
```

**Windows**  

```PowerShell
# iwr = Invoke-WebRequest
# -useb = -UseBasicParsing
# iex = Invoke-Expression
pwsh -Command "iwr -useb https://raw.githubusercontent.com/dapr/cli/master/install/install.ps1 | iex"
```

**MacOS** 

```bash
curl -fsSL https://raw.githubusercontent.com/dapr/cli/master/install/install.sh | /bin/bash
```

**Binaries**  

You can manually download and install the binaries for each Dapr CLI release:

1. Download the desired Dapr CLI from the latest [Dapr Release](https://github.com/dapr/cli/releases)
2. Unpack it (e.g. dapr_linux_amd64.tar.gz, dapr_windows_amd64.zip).
3. Move it to the desired location.
    * For Linux/MacOS, we recommend `/usr/local/bin`.
    * For Windows, create a directory and add this to your System PATH:
        * Create a directory called `C:\dapr`.
        * Add your newly created directory to your System PATH by editing your system environment variable.

## Step 2: Verify the Installation

Verify the CLI is installed by restarting your terminal / command prompt and running the following:

```bash
dapr
```

**Output:**

```bash
         __
    ____/ /___ _____  _____
   / __  / __ '/ __ \/ ___/
  / /_/ / /_/ / /_/ / /
  \__,_/\__,_/ .___/_/
              /_/

===============================
Distributed Application Runtime

Usage:
  dapr [command]

Available Commands:
  completion     Generates shell completion scripts
  components     List all Dapr components. Supported platforms: Kubernetes
  configurations List all Dapr configurations. Supported platforms: Kubernetes
  dashboard      Start Dapr dashboard. Supported platforms: Kubernetes and self-hosted
  help           Help about any command
  init           Install Dapr on supported hosting platforms. Supported platforms: Kubernetes and self-hosted
  invoke         Invoke a method on a given Dapr application. Supported platforms: Self-hosted
  list           List all Dapr instances. Supported platforms: Kubernetes and self-hosted
  logs           Get Dapr sidecar logs for an application. Supported platforms: Kubernetes
  mtls           Check if mTLS is enabled. Supported platforms: Kubernetes
  publish        Publish a pub-sub event. Supported platforms: Self-hosted
  run            Run Dapr and (optionally) your application side by side. Supported platforms: Self-hosted
  status         Show the health status of Dapr services. Supported platforms: Kubernetes
  stop           Stop Dapr instances and their associated apps. . Supported platforms: Self-hosted
  uninstall      Uninstall Dapr runtime. Supported platforms: Kubernetes and self-hosted
  upgrade        Upgrades a Dapr control plane installation in a cluster. Supported platforms: Kubernetes
  version        Print the Dapr runtime and CLI version

Flags:
  -h, --help      help for dapr
  -v, --version   version for dapr

Use "dapr [command] --help" for more information about a command.
```