# Dapr Placement Control Plane Service Overview

**Overview of the Dapr placement service**

https://docs.dapr.io/concepts/dapr-services/placement/

The Dapr Placement service is used to calculate and distribute hash tables for the location of [Dapr actors](https://docs.dapr.io/developing-applications/building-blocks/actors/) running in [self-hosted mode](https://docs.dapr.io/operations/hosting/self-hosted/) or on [Kubernetes](https://docs.dapr.io/operations/hosting/kubernetes/). This hash table maps actor IDs to pods or processes so a Dapr application can communicate with the actor. Anytime a Dapr applicatino activates a Dapr actor, the placement updates the hash tables with the latest actor locations.

## Self-hosted Mode

The placement service Docker container is started automatically as part of [`dapr init`](https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-with-docker/). It can also be run manually as a process if you are running in [slim-init mode](https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-no-docker/).

## Kubernetes Mode

The placement service is deployed as part of `dapr init -k`, or via the Dapr Helm charts. For more information on running Dapr on Kubernetes, visit the [Kubernetes hosting page](https://docs.dapr.io/operations/hosting/kubernetes/).