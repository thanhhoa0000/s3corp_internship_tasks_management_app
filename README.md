# [S3Corp Internship] Tasks Management System

A simple task management system implemented with a microservices architecture, built using MinimalAPI (.NET 8) and JWT role-based access authorization.

## Overview

This app allows users to manage their tasks, with different functionalities for normal users and admins. The system is composed of the following microservices:

- **Tasks API**: For normal users to manage their tasks (CRUD operations).
- **Users API**: For admin to manage user accounts (create, update, delete users).
- **Authentication API**: For login and registration, using JWT tokens for authentication.

The app implements **JWT role-based access control** to secure access to the APIs, with different permissions for normal users and admins.

## Features

- **Role-based Access**: Admins can manage users and their tasks, while regular users can only manage their own tasks.
- **JWT Authentication**: Login and register functionality that generates secure JWT tokens.
- **Microservices Architecture**: Separated functionalities for tasks management, user management, and authentication.
- **MinimalAPI**: Built using .NET 8 Minimal API for a lightweight, fast, and modern solution.

## Requirements

- Docker
- .NET 8 SDK (for development purposes)

## Running the App

To run the app, you'll need Docker and Docker Compose installed.

### Steps:

1. Clone the repository:
    ```bash
    git clone https://github.com/thanhhoa0000/s3corp_internship_tasks_management_app
    cd s3corp_internship_tasks_management_app
    ```

2. Build and run the containers:
    ```bash
    docker compose up --build -d
    ```

## To verify that Docker is working correctly, you can check the status by running:
```bash
docker --version
docker-compose --version
```
