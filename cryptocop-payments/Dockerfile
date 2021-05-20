FROM node:latest
LABEL maintainer="Arnthor Dadi Jonasson"

COPY . /app
WORKDIR /app

RUN npm install

ENTRYPOINT [ "npm", "run", "build" ]