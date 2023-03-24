# Dapr Sentry Control Plane Service Overview

**Overview of the Dapr sentry service**

https://docs.dapr.io/concepts/dapr-services/sentry/

The Dapr Sentry service manages mTLS between services and acts as a certificate authority. It generates mTLS certificates and distributes them to any running sidecars. This allows sidecars to communicate with encrypted, mTLS traffic. For more information read the [sidecar-to-sidecar communication overview](https://docs.dapr.io/concepts/security-concept/#sidecar-to-sidecar-communication).

## Self-hosted Mode

The Sentry service Docker container is not started automatically as part of [`dapr init`](https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-with-docker/). However, it can be executed manually by following the instructions for setting up [mutual TLS](https://docs.dapr.io/operations/security/mtls/#self-hosted).

It can also be run manually as a process if you are running in [slim-init mode](https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-no-docker/).

![mTLS-sentry-selfhosted](https://docs.dapr.io/images/security-mTLS-sentry-selfhosted.png)

## Kubernetes Mode

The sentry service is deployed as part of `dapr init -k`, or via the Dapr Helm charts. For more information on running Dapr on Kubernetes, visit the [Kubernetes hosting page](https://docs.dapr.io/operations/hosting/kubernetes/).

![mTLS-sentry-kubernetes](https://docs.dapr.io/images/security-mTLS-sentry-kubernetes.png)