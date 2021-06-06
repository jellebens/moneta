FROM node:16-alpine3.11 as build
WORKDIR /app

ENV PATH /app/node_modules/.bin:$PATH

COPY Moneta.Frontend.Web/package.json ./
COPY Moneta.Frontend.Web/package-lock.json ./
RUN npm install --silent

COPY Moneta.Frontend.Web/ ./
RUN touch .env

RUN yarn build

FROM nginx:latest
COPY --from=build /app/build /usr/share/nginx/html

COPY Moneta.Frontend.Web/nginx/default.conf /etc/nginx/conf.d/default.conf
COPY Moneta.Frontend.Web/nginx/nginx.conf /etc/nginx/nginx.conf

RUN chown -R nobody:nogroup /usr/share/nginx/html && chmod -R 755 /usr/share/nginx/html && \
        chown -R nobody:nogroup /var/cache/nginx && \
        chown -R nobody:nogroup /var/log/nginx && \
        chown -R nobody:nogroup /etc/nginx/conf.d

RUN touch /var/run/nginx.pid && \
        chown -R nobody:nogroup /var/run/nginx.pid

EXPOSE 8080

CMD ["nginx", "-g", "daemon off;"]