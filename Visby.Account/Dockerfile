FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Visby.Account.csproj", "Visby.Account/"]
RUN dotnet restore "Visby.Account/Visby.Account.csproj"
COPY . Visby.Account/
WORKDIR "/src/Visby.Account"
RUN dotnet build "Visby.Account.csproj" -c Release -o /app/build -v m

FROM build AS publish
RUN dotnet publish "Visby.Account.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Visby.Account.dll"]
