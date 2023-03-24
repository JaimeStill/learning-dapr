# Dapr Terminology and Definitions

**Definitions for common terms and acronyms in the Dapr documentation**

https://docs.dapr.io/concepts/terminology/

This page details all of the common terms you may come across in the Dapr docs.

Term | Definition | More Information
-----|------------|-----------------
App/Application | A running service/binary, usually one that you as the user create and run | 
Building block | An API that Dapr provides to users to help in the creation of microservices and applications | [Dapr building blocks](https://docs.dapr.io/concepts/building-blocks-concept/)
Component | Modular types of functionality that are used either individually or with a collection of other components, by a Dapr building block | [Dapr components](https://docs.dapr.io/concepts/components-concept/)
Configuration | A YAML file declaring all of the settings for Dapr sidecars or the Dapr control plane. This is where you can configure contorl plane mTLS settings, or the tracing and middleware settings for an application instance. | [Dapr configuration](https://docs.dapr.io/concepts/configuration-concept/)
Dapr | Distributed Application Runtime | [Dapr overview](https://docs.dapr.io/concepts/overview/)
Dapr control plane | A collection of services that are part of a Dapr installation on a hosting platform such as a Kubernetes cluster. This allows Dapr-enabled applications to run on the platform and handles Dapr capabilities such as actor placement, Dapr sidecar injection, or certificate issuance / rollover. | [Self-hosted overview](https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-overview/) - [Kubernetes Overview](https://docs.dapr.io/operations/hosting/kubernetes/kubernetes-overview/)
Self-hosted | Windows/macOS/Linux machine(s) where you can run your applications with Dapr. Dapr provides the capability to run on machines in "self-hosted" mode. | [Self-hosted mode](https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-overview/)
Service | A running application or binary. This can refer to your application or to a Dapr application | 
Sidecar | A program that run alongside your application as a separate process or container | [Sidecar pattern](https://docs.microsoft.com/azure/architecture/patterns/sidecar)