# Dapr Sidecar Injector Control Plane Service Overview

**Overview of the Dapr sidecar injector process**  

https://docs.dapr.io/concepts/dapr-services/sidecar-injector/

When running Dapr in [Kubernetes mode](https://docs.dapr.io/operations/hosting/kubernetes/), a pod is created running the Dapr Sidecar Injector service, which looks for pods initialized with the [Dapr annotations](https://docs.dapr.io/reference/arguments-annotations-overview/), and then creates another container in that pod for the [daprd service](https://docs.dapr.io/concepts/dapr-services/sidecar/).

## Running the Sidecar Injector

The sidecar injector service is deployed as part of `dapr init -k`, or via the Dapr Helm charts. For more information on running Dapr on Kubernetes, visit the [Kubernetes hosting page](https://docs.dapr.io/operations/hosting/kubernetes/).