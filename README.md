# Docker Build and Run

docker build --tag goblin-identity:1.0 .

docker run --network bridge --publish 8003:80 --env-file DockerEnv --detach --name goblin-identity goblin-identity:1.0

---

# Docker Remove

docker rm --force goblin-identity

---

# Network

docker network ls

docker network create -d bridge goblin

docker network inspect goblin

docker network rm goblin