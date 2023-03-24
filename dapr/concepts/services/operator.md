# Dapr Operator Control Plane Service Overview

**Overview of the Dapr operator service**

https://docs.dapr.io/concepts/dapr-services/operator/

When running Dapr in [Kubernetes mode](https://docs.dapr.io/operations/hosting/kubernetes/), a pod running the Dapr Operator service manages [Dapr component](https://docs.dapr.io/operations/components/) updates and provides Kubernetes services endpoints for Dapr.

## Running the Operator Service

The operator service is deployed as part of `dapr init -k`, or via the Dapr Helm charts. for more information on running Dapr on Kubernetes, vist the [Kubernetes hosting page](https://docs.dapr.io/operations/hosting/kubernetes/).

## Additional Configuration Options

The operator service includes additional configuration options.

### Injector Watchdog

The operator service includes an *injector watchdog* feature which periodically polls all pods running in your Kubernetes cluster and confirms that the Dapr sidecar is injected in those which have the `dapr.io/enabled=true` annotation. It is primarily meant to address situations where the [Injector service](https://docs.dapr.io/concepts/dapr-services/sidecar-injector/) did not successfully inject the sidecar (this `daprd` container) into pods.

Thet injector watchdog can be useful in a few situations, including:

* Recovering from a Kubernetes cluster completely stopped. When a cluster is completely stopped and then restarted (including in the case ofa  total cluster failure), pods are restarted in a random order. If your application is restarted before the Dapr control plane (specifically the Injector service) is ready, the Dapr sidecar may not be injected into your application's pods, causing your application to behave unexectedly.

* Addressing potential random failures with the sidecar injector, such as transient failures within the Injector service.

If the watchdog detects a pod that does not have a sidecar when it should have one, it deletes it. Kubernetes will then re-create the pod, invoking the Dapr sidecar injector again.

The injector watchdog feature is **disabled by default**.

You can enable it by passing the `--watch-interval` flag to the `operator` command, which can take one of the following values:

* `--watch-interval=0` - disables the injector watchdog (default values if the flag is omitted).
* `--watch-interval=<interval>` - the injector watchdog is enabled and polls all pods at the given interval; the value for the interval is a string that includes the unit. For example: `--watchdog-interval=10s` (every 10 seconds) or `--watch-interval=2m` (every 2 minutes).
* `--watch-interval=once` - the injector watchdog runs only once when the operator service is started.

If you're using Helm, you can configure the injector watchdog with the [`dapr_operator.watchInterval` option](https://github.com/dapr/dapr/blob/master/charts/dapr/README.md#dapr-operator-options), which has the same values as the command line flags.

> The injector watchdog is safe to use when the operator service is running in HA (High Availability) mode with more than one replica. In this case, Kubernetes automatically elects a "leader" instance which is the only one that runs the injector watchdog service.

> However, whin in HA mode, if you configure the injector watchdog to run "once", the watchdog polling is actually started every time an instance of the operator service is electetd as leader. This means that, should the leader of the operator service crash and a new leader be elected, that would trigger the injector watchdog again.

Watch [this video](https://youtu.be/ecFvpp24lpo) for an overview of the injector watchdog.

Kubernetes / Helm commands from above video simulating use case for injector watchdog:

1. Get all pods in a namespace:

    ```bash
    # note that -n is a global option
    kubectl get pods -n daprdemo
    ```

2. Scale down a pod:

    ```bash
    kubectl scale deployment --replicas=0 -n daprdemo dapr-sidecar-injector
    ```

3. Deploy an app to a pod in a namespace:

    ```bash
    kubectl apply -f ../testapp/app.yaml -n daprdemo
    ```

4. Get details of a kubernetes pod:

    ```bash
    # show details such as annotations, deployed containers, and eventsS
    kubectl describe pod -n daprdemo nginx
    ```

5. Scale a pod back up:

    ```bash
    # re-enable the sidecar injector
    kubectl scale deployment --replicas=1 -n daprdemo dapr-sidecar-injector
    ```

6. Use Helm to enable the Dapr operator service injector watchdog via `dapr_operator.watchInterval`:

    ```bash
    helm upgrade dapr ./charts/dapr \
        --install --wait --timeout 5m0s \
        --namespace=${DAPR_NAMESPACE} \
        --set-string global.tag=${DAPR_TAG}-linux-amd64 \
        --set-string global.registry=${DAPR_REGISTRY} \
        --set dapr_operator.logLevel=debug \
        --set dapr_sidecar_injector.sidecarImagePullPolicy=Always \
        --set global.imagePullPolicy=Always \
        --set dapr_operator.watchInterval=2m
    ```

The `nginx` pod should now have two containers running, the `nginx` container and the `daprd` sidecar container. You can inspect the logs for the dapr operator to see that the `nginx` pod was deleted so it could be recreated by Kubernetes with the `daprd` container added via the sidecar injector service.

```bash
kubectl logs -n daprdemo -l app=dapr-operator --tail=-1 | grep nginx

# verify two containers in the nginx pod
kubectl describe pod -n daprdemo nginx
```