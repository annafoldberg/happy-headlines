# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0@sha256:478b9038d187e5b5c29bfa8173ded5d29e864b5ad06102a12106380ee01e2e49 AS build
WORKDIR /src

# Copy and restore project file for caching
COPY ./ProfanityService/ProfanityService.csproj ./ProfanityService/
RUN dotnet restore ./ProfanityService/ProfanityService.csproj

# Copy remaining relevant source
COPY ./ProfanityService/ ./ProfanityService/

# Build and publish release
WORKDIR /src/ProfanityService
RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0@sha256:a04d1c1d2d26119049494057d80ea6cda25bbd8aef7c444a1fc1ef874fd3955b
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "ProfanityService.dll"]