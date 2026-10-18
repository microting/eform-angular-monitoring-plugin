FROM node:22-bookworm-slim as node-env

WORKDIR /app
ARG SENTRY_AUTH_TOKEN
ARG DISABLE_SENTRY
ENV PATH=/app/node_modules/.bin:$PATH
COPY eform-angular-frontend/eform-client ./
RUN apt-get update
RUN apt-get -y -q install ca-certificates
RUN corepack enable
RUN yarn install
RUN yarn build
RUN if [ -n "$SENTRY_AUTH_TOKEN" ] && [ "$DISABLE_SENTRY" != "true" ]; then yarn sentrysourcemap; else echo "Sentry sourcemap upload skipped (DISABLE_SENTRY=$DISABLE_SENTRY)"; fi

FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build-env
WORKDIR /app
ARG GITVERSION
ARG PLUGINVERSION

# Copy csproj and restore as distinct layers
COPY eform-angular-frontend/eFormAPI/eFormAPI.Web ./eFormAPI.Web
COPY eform-angular-monitoring-plugin/eFormAPI/Plugins/Monitoring.Pn ./Monitoring.Pn
RUN dotnet publish eFormAPI.Web -o eFormAPI.Web/out /p:Version=$GITVERSION --runtime linux-x64 --configuration Release
RUN dotnet publish Monitoring.Pn -o Monitoring.Pn/out /p:Version=$PLUGINVERSION --runtime linux-x64 --configuration Release

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble
WORKDIR /app
ARG DISABLE_SENTRY
ENV DISABLE_SENTRY=${DISABLE_SENTRY}
COPY --from=build-env /app/eFormAPI.Web/out .
RUN mkdir -p ./Plugins/Monitoring.Pn
COPY --from=build-env /app/Monitoring.Pn/out ./Plugins/Monitoring.Pn
COPY --from=node-env /app/dist/browser wwwroot
RUN rm connection.json; exit 0

ENV DEBIAN_FRONTEND=noninteractive
ENV Logging__Console__FormatterName=

RUN mkdir -p /usr/share/man/man1 && \
	apt-get update && \
	apt-get install -y -q --no-install-recommends \
		libxml2 \
		libgdiplus \
		libc6-dev \
		libreoffice-writer \
		libreoffice-core \
		fonts-liberation \
		fonts-dejavu-core \
		fonts-freefont-ttf \
		hyphen-en-us \
		openjdk-17-jre-headless && \
	apt-get -y -q remove libreoffice-gnome || true && \
	apt-get clean && \
	apt-get -y autoremove && \
	rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

RUN mkdir -p /opt/libreoffice && chown -R $APP_UID:$APP_UID /app /opt/libreoffice
ENV HOME=/opt/libreoffice
USER $APP_UID

EXPOSE 5000 5001

ENTRYPOINT ["dotnet", "eFormAPI.Web.dll"]
