# Application and Control Plan Configuration

**Change the behavior of Dapr application sidecars or globally on Dapr control plan system services**

https://docs.dapr.io/concepts/configuration-concept/

Dapr configurations are settings and policies that enable you to change both the behavior of individual Dapr applications, or the global behavior of the dapr control plane system services. For example, you can set an ACL policy on the application sidecar configuration which indicates which methods can be called from another application, or on the Dapr control plane configuration you can change the certificate renewal period for all certificates that are deployed to application sidecar instances.

Configurations are defined and deployed as a YAML file. An application configuration example is shown below, which demonstrates an example fo setting a tracing endpoint for where to send the metrics information, capturing all the sample traces.

```yaml
apiVersion: dapr.io/v1alpha1
kind: Configuration
metadata:
    name: daprConfig
    namespace: default
spec:
    tracing:
        samplingRate: "1"
        zipkin:
            endponitAddress: "http://localhost:9411/api/v2/spans"
```

This configuration configures tracing for metrics recording. It can be loaded in local self-hosted mode by editing the default configuration file called `config.yaml` file in your `.dapr` directory (`$env:userprofile\.dapr`), or by applying it to your Kubernetes cluster with kubectl/helm.

Here is an example of the Dapr control plane configuration called `daprsystem` in the `dapr-system` namespace:

```yaml
apiVersion: dapr.io/v1alpha1
kind: Configuration
metadata:
    name: daprsystem
    namespace: dapr-system
spec:
    mtls:
        enabled: true
        workloadCertTTL: "24h"
        allowedClockSkew: "15m"
```

Visit [overview of Dapr configuration options](https://docs.dapr.io/operations/configuration/configuration-overview/) for a list of the configuration options.