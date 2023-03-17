# Resiliency

**Configure policies and monitor app and sidecar health**  

https://docs.dapr.io/concepts/resiliency-concept/

Distributed applications are commonly comprised of many microservices, with dozens - sometimes hundreds - of instances scaling across underlying infrastructure. As these distributed solutions grow in size and complexity, the potential for system failures inevitably increases. Service instances can fail or become unresponsive due to any number of issues, including hardware failures, unexpected throughput, or application lifecycle events, such as scaling out and application restarts. Designing and implementing a self-healing solution with the ability to detect, mitigate, and respond to failure is critical.

## Resiliency Policies

![resiliency-policies](https://docs.dapr.io/images/resiliency_diagram.png)

Dapr provides a capability for defining and applying fault tolerance resiliency policies to your application. You can define policies for following resiliency patterns:

* Timeouts
* Retries / back-offs
* Circuit breakers

These policies can be applied to any Dapr API calls when calling components with a [resiliency spec](https://docs.dapr.io/operations/resiliency/resiliency-overview/).

## App Health Checks

![app-health-checks](https://docs.dapr.io/images/observability-app-health.webp)

Applications can become unresponsive for a variety of reasons. For example, they are too busy to accept new work, could have crashed, or be in a deadlock state. Sometimes the condition can be transitory or persistent.

Dapr provides a capability for monitoring app health through probes that check the health of your application and react to status changes. When an unhealthy app is detected, Dapr stops accepting new work on behalf of the application.

Read more on how to apply [app health checks](https://docs.dapr.io/developing-applications/building-blocks/observability/app-health/) to your application.

## Sidecar Health Chekcs

![sidecar-health-checks](https://docs.dapr.io/images/sidecar-health.png)

Dapr provides a way to determine its health using an [HTTP `/healthz` endpoint](https://docs.dapr.io/reference/api/health_api/). With this endpoint, the *daprd* process, or sidecar, can be:

* Probed for its health
* Determined for readiness and liveness

Read more about how to apply [dapr health checks](https://docs.dapr.io/developing-applications/building-blocks/observability/sidecar-health/) to your application.