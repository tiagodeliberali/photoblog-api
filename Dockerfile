FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Photoblog.Api.csproj ./
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
RUN dotnet build Photoblog.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Photoblog.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Photoblog.Api.dll"]