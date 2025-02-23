# Stage 1: Build the Angular project and api-client package
FROM node:22 AS build

# Set working directory
WORKDIR /workspace

# Copy pnpm workspace files
COPY projects/pnpm-workspace.yaml .
COPY projects/package.json .
COPY projects/pnpm-lock.yaml .

# Install pnpm globally
RUN npm install -g pnpm

# Install dependencies for the workspace
RUN pnpm install --frozen-lockfile

# Copy the entire workspace (including frontend and api-client)
COPY projects/ ./projects

# Build the api-client package
WORKDIR /workspace/projects/api-client
RUN pnpm run build

# Build the Angular project
WORKDIR /workspace/projects/frontend
RUN pnpm run build

# Stage 2: Serve the Angular app using Nginx
FROM nginx:alpine AS production

# Copy the built Angular app from the build stage
COPY --from=build /workspace/projects/frontend/dist/frontend /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]

# Stage 3: Development - Serve the Angular app with live reload
FROM node:22 AS development

# Set working directory
WORKDIR /workspace

# Copy pnpm workspace files
COPY projects/pnpm-workspace.yaml .
COPY projects/package.json .
COPY projects/pnpm-lock.yaml .

# Install pnpm globally
RUN npm install -g pnpm

# Install dependencies for the workspace
RUN pnpm install --frozen-lockfile

# Copy the entire workspace (including frontend and api-client)
COPY projects/ ./projects

# Expose port 4200 for Angular development server
EXPOSE 4200

# Start the Angular development server
WORKDIR /workspace/projects/frontend
CMD ["pnpm", "start"]