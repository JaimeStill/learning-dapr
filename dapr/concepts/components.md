# Components

**Modular functionality used by building blocks and applications**

https://docs.dapr.io/concepts/components-concept/

Dapr uses a modular design where functionality is delivered as a component. Each component has an interface definition. All of the components are interchangeable so that you can swap out one component with the same interface for another.

You can contribute implementations and extend Dapr's component interfaces via:

* The [components-contrib repository](https://github.com/dapr/components-contrib)

* [Pluggable components](https://docs.dapr.io/concepts/components-concept/#built-in-and-pluggable-components)

A building block can use any combination of components. For example, the [actors](https://docs.dapr.io/developing-applications/building-blocks/actors/actors-overview/) and the [state management](https://docs.dapr.io/developing-applications/building-blocks/state-management/state-management-overview/) building blocks both use [state components](https://github.com/dapr/components-contrib/tree/master/state).

As another example, the [pub/sub](https://docs.dapr.io/developing-applications/building-blocks/pubsub/pubsub-overview/) building block uses [pub/sub components](https://github.com/dapr/components-contrib/tree/master/pubsub).

You can get a list of current components available in the hosting environmetn using the `dapr components` CLI command.

## Component Specification

Each component has a specification (or spec) that it conforms to. Components are configured at design-time with a YAML file which is stored in either:

* A `components/local` folder within your solution, or

* Globally in the `.dapr` folder created when invoking `dapr init`. (`$env:userprofile\.dapr`)

These YAML files adhere to the generic [Dapr component schema](https://docs.dapr.io/operations/components/component-schema/), but each is specific to the comopnent specification.

It is important to understand that the component spec values, particularly the spec `metadata`, can change between components of the same component type, for example between different state stores, and that some design-time spec values can be overridden at runtime when making requests to a component's API. As a result, it is strongly recommended to review a [component's specs](https://docs.dapr.io/reference/components-reference/), paying particular attention to the sample payloads for requests to set the metadata used to interact with the component.

![dapr-components](https://docs.dapr.io/images/concepts-components.png)

## Available Component Types

* **State stores** - Data stores (databases, files, memory) that store key-value pairs as part of the [state management](https://docs.dapr.io/developing-applications/building-blocks/state-management/state-management-overview/) building block.
    * [list](https://docs.dapr.io/reference/components-reference/supported-state-stores/) and [implementations](https://github.com/dapr/components-contrib/tree/master/state)

* **Name resolution** - Used with the [service invocation](https://docs.dapr.io/developing-applications/building-blocks/service-invocation/service-invocation-overview/) building block to integrate with the hosting environment and provide service-to-service discovery. For example, the Kubernetes name resolution component integrates with the Kubernetes DNS service, self-hosted uses mDNS and clusters of VMs can use the Consul name resolution component.
    * [list](https://docs.dapr.io/reference/components-reference/supported-name-resolution/) and [implementations](https://github.com/dapr/components-contrib/tree/master/nameresolution).

* **Pub/sub brokers** - Pub/sub broker components are message brokers that can pass messages to / from services as part of the [publish & subscribe](https://docs.dapr.io/developing-applications/building-blocks/pubsub/pubsub-overview/) building block.
    * [list](https://docs.dapr.io/reference/components-reference/supported-pubsub/) and [implementations](https://github.com/dapr/components-contrib/tree/master/pubsub)

* **Bindings** - External resources can connect to Dapr in order to trigger a method on an application or be called from an application as part of the [bindings](https://docs.dapr.io/developing-applications/building-blocks/bindings/bindings-overview/) building block.
    * [list](https://docs.dapr.io/reference/components-reference/supported-bindings/) and [implementations](https://github.com/dapr/components-contrib/tree/master/bindings)

* **Secret stores** - A [secret](https://docs.dapr.io/developing-applications/building-blocks/secrets/secrets-overview/) is any piece of private information that you want to guard against unwanted access. Secret stores are used to store secrets that can be retrieved and used in applications.
    * [list](https://docs.dapr.io/reference/components-reference/supported-secret-stores/) and [implementations](https://github.com/dapr/components-contrib/tree/master/secretstores)

* **Configuration stores** - Configuration stores are used to save application data, which can then be read by application instances on startup or notified of when changes occur. This allows for dynamic configuration.
    * [list](https://docs.dapr.io/reference/components-reference/supported-configuration-stores/) and [implementations](https://github.com/dapr/components-contrib/tree/master/configuration)

* **Locks** - Lock components are used as a distributed lock to provide mutually exclusive access to a resource such as a queue or database.
    * [list](https://docs.dapr.io/reference/components-reference/supported-locks/) and [implementations](https://github.com/dapr/components-contrib/tree/master/lock)

* **Workflows** - A [workflow](https://docs.dapr.io/developing-applications/building-blocks/workflow/workflow-overview/) is custom application logic that defines a reliable business process or data flow. Workflow components are workflow runtimes (or engines) that run the business logic written for that workflow and store their state in a state store.

* **Middleware** - Dapr allows custom [middleware]() to be plugged into the HTTP request processing pipeline. Middleware can perform additional actions on an HTTP request (such as authentication, encryption, and message transformation) before the request is routed to the user code, or the response is returned to the client. The middleware components are used with the [service invocation](https://docs.dapr.io/developing-applications/building-blocks/service-invocation/service-invocation-overview/) building block.
    * [list](https://docs.dapr.io/reference/components-reference/supported-middleware/) and [implementations](https://github.com/dapr/components-contrib/tree/master/middleware)