#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["micro/NPlatform.GrpcServe/NPlatform.GrpcServe/NPlatform.GrpcServe.csproj", "micro/NPlatform.GrpcServe/NPlatform.GrpcServe/"]
RUN dotnet restore "micro/NPlatform.GrpcServe/NPlatform.GrpcServe/NPlatform.GrpcServe.csproj"
COPY . .
WORKDIR "/src/micro/NPlatform.GrpcServe/NPlatform.GrpcServe"
RUN dotnet build "NPlatform.GrpcServe.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NPlatform.GrpcServe.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NPlatform.GrpcServe.dll"]