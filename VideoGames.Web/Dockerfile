# build
FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build -- --configuration production

# run
FROM nginx:alpine
COPY --from=build /app/dist/VideoGames.Web/browser/ /usr/share/nginx/html
EXPOSE 80
