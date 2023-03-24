# Backup and Restore Docker Images

## List Available Images

```bash
# list docker images
docker images
```

**Output:**

> Only relevant table columns display for brevity

Repository | Tag
-----------|----
daprio/daprd | latest
daprio/dapr | 1.10.4
redis | 6
openzipkin/zipkin | latest

## Save the Image

```bash
docker save <repository>:<tag> -o <name>-<tag>.tar

#example
docker save daprio/dapr:1.10.4 -o dapr-1.10.4.tar
```

## Load the Saved Image

```bash
docker load -i <name>-<tag>.tar

# example
docker load -i dapr-1.10.4.tar
```

## Create and Run the Image

```bash
docker create daprio/dapr:1.10.4

docker run -d ... daprio/dapr:1.10.4
```