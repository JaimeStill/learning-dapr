# Dapr

https://docs.dapr.io/concepts/overview/

![dapr-overview](https://docs.dapr.io/images/overview.png)

Dapr (Distributed Application Runtime) is a portable, event-drive runtime that makes it easy for any developer to build resilient, stateless and stateful applications that run on the cloud and edge and embraces the diversity of languages and developer frameworks.

It's hard to become a distributed systems expert, nor should you have to. Developers want to focus on business logic, while leaning on the platforms to imbue their applications with scale, resiliency, maintainability, elasticity and other attributes of cloud-native architectures.

This is where Dapr comes in. Dapr codifies the *best practices* for building microservice applications into open, independent APIs called building blocks, that enable you to build portable applications with the language and framework of your choice. Each building block is completely independent and you can use one, some, or all of them in your application.

Dapr is platform agnostic, meaning you can run your applications locally, on any Kubernetes cluster, on virtual or physical machines and in other hosting environments that Dapr integrates with. This enables you to build microservice applications that can run on the cloud and edge.

## Building Blocks

![dapr-building-blocks](https://docs.dapr.io/images/building_blocks.png)

Dapr provides best practices for common capabilities when building microservice applications that developers can use in a standard way, and deploy to any environment. It does this by providing distributed building blocks.

Each of these building blocks is independent, meaning that you can use one, some, or all of them in your application. The following building blocks are available:

Building Block | Description
---------------|------------
[Service-to-service invocation](https://docs.dapr.io/developing-applications/building-blocks/service-invocation/service-invocation-overview/) | Resilient service-to-service invocation enables method calls, including retries, on remote services, wherever they are located in the supported hosting environment.
[State management](https://docs.dapr.io/developing-applications/building-blocks/state-management/state-management-overview/) | With state management for storing and querying key/value pairs, long-running, highly available, stateful services can be easily written alongside stateless services in your application. The state store is pluggable and examples include AWS DynamoDB, Azure CosmosDB, Azure SQL Server, GCP Firebase, PostgreSQL, or Redis, among others.
[Publish and subscribe](https://docs.dapr.io/developing-applications/building-blocks/pubsub/pubsub-overview/) | Publishing events and subscribing to topics between services enables event-driven architectures to simplify horizontal scalability and make them resilient to failure. Dapr provides at-least-once message delivery guarantee, message TTL, consumer groups, and other advanced features.
[Resource bindings](https://docs.dapr.io/developing-applications/building-blocks/bindings/bindings-overview/) | Resource bindings with triggers builds further on event-driven architectures for scale and resiliency by receiving and sending events to and from any external source such as databases, queues, file systems, etc.
[Actors](https://docs.dapr.io/developing-applications/building-blocks/actors/actors-overview/) | A pattern for stateful and stateless objects that mkes concurrency simple, with method and state encapsulation. Dapr provides many capabilities in its actor runtime, including concurrency, state, and life-cycle management for actor activation / deactivation, and timers and reminders to wake up actors.
[Observability](https://docs.dapr.io/concepts/observability-concept/) | Dapr emits metrics, logs, and traces to debug and monitor both Dapr and user applications. Dapr supports distributed tracing to easily diagnose and serve inter-service calls in production using the W3C Trace Context standard and Open Telemetry to send to different monitoring tools.
[Secrets](https://docs.dapr.io/developing-applications/building-blocks/secrets/secrets-overview/) | The secrets management API integrates with public cloud and local secret stores to retrieve the secrets for use in application code.
[Configuration](https://docs.dapr.io/developing-applications/building-blocks/configuration/configuration-api-overview/) | The configuration API enables you to retrieve and subscribe to application configuration items from configuration stores.
[Distributed lock](https://docs.dapr.io/developing-applications/building-blocks/distributed-lock/distributed-lock-api-overview/) | The distributed lock API enables your application to acquire a lock for any resource that gives it exclusive access until either the lock is released by the application, or a lease timeout occurs.
[Workflows](https://docs.dapr.io/developing-applications/building-blocks/workflow/workflow-overview/) | Workflows makes orchestrating the logic required for messaging, state management, and failure handling across various microservices easier for developers. They enable you to create long running, fault-tolerant, and stateful applications.

## Sidecar Architecture

![sidecar-model](https://docs.dapr.io/images/overview-sidecar-model.png)

Dapr exposes its HTTP and gRPC APIs as a sidecar architecture, either as a container or as a process, not requiring the application code to include any Dapr runtime code. This makes integration with Dapr easy from other runtimes, as well as providing separation of the application logic for improved supportability.