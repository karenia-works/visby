FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Visby.Professors/Visby.Professors.csproj", "Visby.Professors/"]
RUN dotnet restore "Visby.Professors/Visby.Professors.csproj"
COPY . .
WORKDIR "/src/Visby.Professors"
RUN dotnet build "Visby.Professors.csproj" -c Release -o /app/build -v m

FROM build AS publish
RUN dotnet publish "Visby.Professors.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Visby.Professors.dll"]
