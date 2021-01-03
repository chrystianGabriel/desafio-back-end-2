FROM  mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /src

COPY ./PlaylistRecommender.Api/*.csproj ./PlaylistRecommender.Api/
COPY ./PlaylistRecommender.Domain/*.csproj ./PlaylistRecommender.Domain/
RUN dotnet restore PlaylistRecommender.Api/



COPY ./PlaylistRecommender.Api/ ./PlaylistRecommender.Api/
COPY ./PlaylistRecommender.Domain/ ./PlaylistRecommender.Domain/
WORKDIR /src/PlaylistRecommender.Api
RUN dotnet build -c release --no-restore --no-cache /restore

FROM build-env AS publish
RUN dotnet publish --no-restore -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY ./*settings*.json ./
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PlaylistRecommender.Api.dll"]
