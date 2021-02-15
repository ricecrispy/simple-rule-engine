docker rm -f jsiuapp
docker build -t jsiucontainer .
docker run --name jsiuapp jsiucontainer