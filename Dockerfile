#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.WebApi/EOM.TSHotelManagement.WebApi.csproj", "TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.WebApi/"]
COPY ["TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Shared/EOM.TSHotelManagement.Shared.csproj", "TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Shared/"]
COPY ["TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Application/EOM.TSHotelManagement.Application.csproj", "TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Application/"]
COPY ["TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Common.Contract/EOM.TSHotelManagement.Common.Contract.csproj", "TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Common.Contract/"]
COPY ["TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Common.Migration/EOM.TSHotelManagement.Common.Migration.csproj", "TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Common.Migration/"]
COPY ["TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Common.Core/EOM.TSHotelManagement.Common.Core.csproj", "TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Common.Core/"]
COPY ["TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Common.Util/EOM.TSHotelManagement.Common.Util.csproj", "TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.Common.Util/"]
COPY ["TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.EntityFramework/EOM.TSHotelManagement.EntityFramework.csproj", "TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.EntityFramework/"]
RUN dotnet restore "./TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.WebApi/EOM.TSHotelManagement.WebApi.csproj"
COPY . .
WORKDIR "/src/TopskyHotelManagerSystem-WebApi/EOM.TSHotelManagement.WebApi"
RUN dotnet build "./EOM.TSHotelManagement.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EOM.TSHotelManagement.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EOM.TSHotelManagement.WebApi.dll"]