FROM node:16-alpine3.11 as build
WORKDIR /app

ENV PATH /app/node_modules/.bin:$PATH

COPY package.json ./
COPY package-lock.json ./
RUN npm install --silent

COPY . ./

RUN yarn build

FROM nginx:latest
COPY --from=build /app/build /usr/share/nginx/html

COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
ENV REDIRECT_URL=http://localhost/home
CMD ["nginx", "-g", "daemon off;"]