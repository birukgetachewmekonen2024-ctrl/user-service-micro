FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UserManagement.Service/UserManagement.Service.csproj", "src/UserManagement.Service/"]
RUN dotnet restore "src/UserManagement.Service/UserManagement.Service.csproj"
COPY . .
WORKDIR "/src/src/UserManagement.Service"
RUN dotnet build "UserManagement.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserManagement.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagement.Service.dll"]