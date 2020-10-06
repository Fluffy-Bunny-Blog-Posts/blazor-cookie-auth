echo "Creating shared docker network"
docker network create -d bridge blazorcookieauth-network
docker-compose -f docker-compose.yml up



