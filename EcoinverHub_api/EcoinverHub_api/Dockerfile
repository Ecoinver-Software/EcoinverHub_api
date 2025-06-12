FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN mkdir -p /ssl/certs /ssl/private
COPY ./publish ./
COPY ssl/server.pfx /ssl/server.pfx
EXPOSE 80 443
ENTRYPOINT ["dotnet", "EcoinverHub_api.dll"]