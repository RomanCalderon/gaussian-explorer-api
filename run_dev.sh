#!/bin/bash

docker compose \
    -f docker-compose.yml \
    -f docker-compose.override.yml \
    up -d --build

echo "Starting development environment..."

echo "Container status:"
docker compose ps

echo "Container logs:"
docker compose logs -f