# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files and restore dependencies
COPY ["projects/backend/src/*/*.csproj", "./"]
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore "Api/Api.csproj"

# Copy the rest of the application code
COPY projects/backend/src/ .

# Build the application
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# Stage 4: Production - Run the application
FROM base AS production
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Api.dll"]

# Stage 5: Development - Run the application in watch mode
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
WORKDIR /src

# Copy all project files and restore dependencies
COPY ["projects/backend/src/*/*.csproj", "./"]
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore "Api/Api.csproj"

# Copy the rest of the application code
COPY projects/backend/src/ .

# Expose port 80 for development
EXPOSE 80

# Set the working directory to the API project
WORKDIR "/src/Api"

ENV ASPNETCORE_URLS=http://+:80

# Run the application in watch mode
CMD ["dotnet", "watch", "run", "--no-restore"]