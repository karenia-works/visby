FROM alpine:3.7
RUN apk add --no-cache --virtual .fetch-deps \
    ca-certificates \
    openssl \
    tar
WORKDIR /usr/bin/
RUN wget "https://github.com/caddyserver/caddy/releases/download/v2.0.0-beta10/caddy2_beta10_linux_amd64" -O caddy
RUN chmod 777 /usr/bin/caddy
ENV PATH=$PATH:/usr/bin
RUN mkdir /app
WORKDIR /app
ENTRYPOINT [ "/usr/bin/caddy", "start" ,"--config", "caddy.json"]
