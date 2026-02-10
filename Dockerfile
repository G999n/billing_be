# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the project file first (Preserving the subfolder structure is KEY here)
COPY ["billing_be/billing_be.csproj", "billing_be/"]

# Restore dependencies
RUN dotnet restore "billing_be/billing_be.csproj"

# Copy the rest of the code
COPY . .

# Build and Publish
WORKDIR "/src/billing_be"
RUN dotnet publish "billing_be.csproj" -c Release -o /app/publish

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

# Cloud Run requirement: Listen on port 8080
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "billing_be.dll"]
