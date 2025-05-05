FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["IO.Swagger.sln", "./"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Service/Service.csproj", "Service/"]
COPY ["src/IO.Swagger/IO.Swagger.csproj", "src/IO.Swagger/"]

RUN dotnet restore IO.Swagger.sln

COPY . .

WORKDIR /src/src/IO.Swagger
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "IO.Swagger.dll"]
