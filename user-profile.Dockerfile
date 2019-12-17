FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Visby.UserProfile/Visby.UserProfile.csproj", "Visby.UserProfile/"]
RUN dotnet restore "Visby.UserProfile/Visby.UserProfile.csproj"
COPY . .
WORKDIR "/src/Visby.UserProfile"
RUN dotnet build "Visby.UserProfile.csproj" -c Release -o /app/build -v m

FROM build AS publish
RUN dotnet publish "Visby.UserProfile.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Visby.UserProfile.dll"]
