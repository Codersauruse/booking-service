FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build


WORKDIR /src

# Copy the project file and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET 8 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app



# Copy the published application from the build stage
COPY --from=build /app/publish .
# Set environment to Development so Swagger is enabled
ENV ASPNETCORE_ENVIRONMENT=Development
# Expose the port your application will run on
EXPOSE 8083

# Set the entry point for the container
ENTRYPOINT ["dotnet", "booking-service.dll","--environmnt=Development"]