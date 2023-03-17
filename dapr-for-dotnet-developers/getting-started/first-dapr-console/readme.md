# First Dapr Console

The Dapr CLI `run` command starts the application. It invokes the underlying Dapr runtime and enables both the application and Dapr sidecar to run together. If you omit the `app-id`, Dapr will generate a unique name for the application. The final segment of the command, `dotnet run`, instructs the Dapr runtime to run the .NET application.

> Care must be taken to always pass an explicit `app-id` parameter when consuming the state management building block. The block uses the application id as a *prefix* for its state key for each key/value pair. If the application id changes, you can no longer access the previously stored state.

```bash
dapr run --app-id DaprCounter dotnet run
```

If you stop and restart the application, you'll see that the counter doesn't reset. Instead, it continues from the previously saved state. The Dapr building block makes the application stateful.

> It's important to understand your sample application communicates with a pre-configured state component, but has no direct dependency on it. Dapr abstracts away the dependency. The underlying state store component can be changed with a simple configuration update.

When you first initialized Dapr for the local environment, it automatically provisioned a Redis container. Dapr then configured the Redis container as the default state store component with a component configuration file, entitled `statestore.yaml`:

> This file can be found at `$env:userprofile\.dapr\components\statestore.yaml`

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: statestore
spec:
  type: state.redis
  version: v1
  metadata:
  - name: redisHost
    value: localhost:6379
  - name: redisPassword
    value: ""
  - name: actorStateStore
    value: "true"
```

A Dapr sidecar can consume any Dapr component configured in your application. But, what if you had an architectural justification to limit the accessibility of a component? How could you restrict the Redis component to Dapr sidecars running only in a production environment?

To do so, you could define a `namespace` for the production environment. You might name it `production`. In self-hosted mode, you specify the namespace of a Dapr sidecar by setting the `NAMESPACE` environment variable. When configured, the Dapr sidecar will only load the componetns that match the namespace. For Kubernetes deployments, the Kubernetes namespace determines the components that are loaded. The following sample shows the Redis component placed in a `production` namespace. Note the `namespace` declaration in the `metadata` element:

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: statestore
  namespace: production
spec:
  type: state.redis
  version: v1
  metadata:
  - name: redisHost
    value: localhost:6379
  - name: redisPassword
    value: ""
  - name: actorStateStore
    value: "true"
```

> A namespaced component is only accessible to applications running in the same namespace. If your Dapr application fails to load a component, make sure that the application namespace matches the component namespace. This can be especially tricky in self-hosted mode where the application namespace is stored in a `NAMESPACE` environment variable.

If needed, you could further restrict a component to a particular application. Within the `production` namespace, you may want to limit access of the Redis cache to only the `DaprCounter` application. You do so by specifying `scopes` in the component configuration. The following example shows how to restrict access to the Redis `statestore` component to the application `DaprCounter` in the `production` namespace:

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: statestore
  namespace: production
spec:
  type: state.redis
  version: v1
  metadata:
  - name: redisHost
    value: localhost:6379
  - name: redisPassword
    value: ""
  - name: actorStateStore
    value: "true"
  scopes:
  - DaprCounter
```