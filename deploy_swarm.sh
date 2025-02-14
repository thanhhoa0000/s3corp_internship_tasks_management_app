#!/bin/bash

CERT_PASSWORD=verylOngandWeAkpAasw0rd
TASKS_CERT_PATH=./certs/.aspnet/https/Tasks.API.pfx
USERS_CERT_PATH=./certs/.aspnet/https/Users.API.pfx
AUTHEN_CERT_PATH=./certs/.aspnet/https/Authen.API.pfx

STACK_NAME=${1:-hoathstack}

echo -e "\nBuilding Docker images..."
docker-compose build

if ! docker info 2>/dev/null | grep -q "Swarm: active"; then
    echo -e "\nInitializing Docker Swarm..."
    docker swarm init
else
    echo -e "\nDocker Swarm already initialized."
fi

echo -e "\nRemoving old secrets..."
docker secret rm aes_key aes_iv 2>/dev/null || true

echo -e "\nCreating new Docker secrets..."
docker secret create aes_key aes_key.bin
docker secret create aes_iv aes_iv.bin
echo "$CERT_PASSWORD" | docker secret create cert_password -

docker secret create tasks_cert $TASKS_CERT_PATH
docker secret create users_cert $USERS_CERT_PATH
docker secret create authen_cert $AUTHEN_CERT_PATH

echo -e "\nDeploying stack: $STACK_NAME..."
docker stack deploy -c docker-compose.yml -c docker-compose.override.yml "$STACK_NAME"

echo -e "\nDeployment Complete!"
