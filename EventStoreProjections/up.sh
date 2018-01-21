#!/bin/bash

docker-compose down
docker-compose up -d

sleep 5

./EventStore/deploy.sh