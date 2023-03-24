# Dapr Init Tracing

Reverse engineer what the `dapr init` command is doing so that it can be replicated in an offline environment

* [init](https://github.com/dapr/cli/blob/master/cmd/init.go) command
* [standalone](https://github.com/dapr/cli/blob/master/pkg/standalone/standalone.go) pkg
* [utils](https://github.com/dapr/cli/blob/master/utils/utils.go)

Self-hosted configuration is stored at `$env:userprofile\.dapr`.

Installed images can be cached using the steps in [docker-images.md](./docker-images.md).