# Dapr Sidecar (daprd) Overview

**Overview of the Dapr sidecar process**

https://docs.dapr.io/concepts/dapr-services/sidecar/

Dapr uses a [sidecar pattern](https://docs.dapr.io/concepts/overview/#sidecar-architecture), meaning the Dapr APIs are run and exposed on a separate process, the Dapr sidecar, running alongside your application. The Dapr sidecar process is named `daprd` and is launched in different ways depending on the hosting environment.

The Dapr sidecar exposes:

* [Building block APIs](https://docs.dapr.io/concepts/building-blocks-concept/) used by your application business logic
* A [metadata API](https://docs.dapr.io/reference/api/metadata_api/) for discoverability of capabilities and to set attributes
* A [health API](https://docs.dapr.io/developing-applications/building-blocks/observability/sidecar-health/) to determine health status and sidecar readiness and liveness

The Dapr sidecar will reach readiness state once the application is accessible on its configured port. The application cannot access the Dapr components during application start up / initialization.

![sidecar-apis](https://docs.dapr.io/images/overview-sidecar-apis.png)

The sidecar APIs are called from your application over local http or gRPC endpoints.

![sidecar-model](https://docs.dapr.io/images/overview-sidecar-model.png)

## Self-hosted with `dapr run`

When Dapr is installed in [self-hosted mode](https://docs.dapr.io/operations/hosting/self-hosted/), the `daprd` binary is downloaded and placed under the user home directory (`$HOME/.dapr/bin` for Linux/macOS or `$env:userprofile\.dapr\bin` for Windows).

In self-hosted mode, running the Dapr CLI [`run` command](https://docs.dapr.io/reference/cli/dapr-run/) launches the `daprd` executable with the provided application executable. This is the recommended way of running the Dapr sidecar when working locally in scenarios such as development and testing.

You can find various arguments that the CLI exposes to configure the sidecar in the [Dapr run command reference](https://docs.dapr.io/reference/cli/dapr-run/).

## Kubernetes with `dapr-sidecar-injector`

On [Kubernetes](https://docs.dapr.io/operations/hosting/kubernetes/), the Dapr control plane includes the [dapr-sidecar-injector service](https://docs.dapr.io/operations/hosting/kubernetes/kubernetes-overview/), which watches for new pods with the `dapr.io/enabled` annotation and injects a container with the `daprd` process within the pod. In this case, sidecar arguments can be passed through annotations as outlined in the **Kubernetes annotations** column in [this table](https://docs.dapr.io/reference/arguments-annotations-overview/).

## Running the Sidecar Directly

In most cases you do not need to run `daprd` explicity, as the sidecar is either launced by the [CLI](https://docs.dapr.io/reference/cli/cli-overview/) (self-hosted mode) or by the dapr-sidecar-injector service (Kubernetes). For advanced use cases (debugging, scripted deployments, etc.) the `daprd` proces can be launced directly.

For a detailed list of all available arguments, run `daprd --help` or see this [table](https://docs.dapr.io/reference/arguments-annotations-overview/) which outlines how the `daprd` arguments relate to the CLI arguments and Kubernetes annotations.

### Examples

1. Start a sidecar alongside an application by sepcifying its unique ID.

    > `--app-id` is a required field, and cannot contain dots.

    ```bash
    daprd --app-id myapp
    ```

2. Specify the port your application is listening to.

    ```bash
    daprd --app-id myapp --app-port 5000
    ```

3. If you are using several custom resources and want to specify the location of the resource definition files, use the `--resources-path` argument:

    ```bash
    daprd --app-id myapp --resources-path <PATH-TO-RESOURCES-FILES>
    ```

4. Enable collection of Prometheus metrics while running your app.

    ```bash
    daprd --app-id myapp --enable-metrics
    ```

5. Listen to IPv4 and IPv6 loopback only.

    ```bash
    daprd --app-id myapp --dapr-listen-addresses '127.0.0.1,[::1]'
    ```