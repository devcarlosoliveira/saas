# Saas.Web

This is a foundational ASP.NET Core web application, intended to be the starting point for a new Software-as-a-Service (SaaS) product. It is built with .NET 10 (Preview) and includes basic setup for user authentication and data persistence.

## Core Technologies

*   **.NET 10 (Preview)**: The underlying framework for the application.
*   **ASP.NET Core**: Used for building the web application, following the MVC (Model-View-Controller) pattern.
*   **Entity Framework Core**: Used as the Object-Relational Mapper (ORM) to interact with the database.
*   **ASP.NET Core Identity**: Provides a complete user authentication and management system, including registration, login, and user roles.
*   **SQLite**: The default database provider, chosen for its simplicity and ease of setup for development.

## Development Tools

This project uses Entity Framework Core for database management. The `dotnet-ef` command-line tool is required to create and apply database migrations. If you haven't already, you can install this tool globally by running the following command:

```sh
dotnet tool install --global dotnet-ef
```

This tool allows you to run `dotnet ef` commands against the project to manage the database schema.

## Getting Started

Follow these instructions to get the project up and running on your local machine.

### Prerequisites

You must have the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) installed on your system.

### Installation & Setup

1.  **Clone the repository:**
    ```sh
    git clone <repository-url>
    cd saas
    ```

2.  **Restore Dependencies:**
    Navigate to the `Saas.Web` directory and run the following command to restore the necessary .NET packages.
    ```sh
    dotnet restore Saas.Web
    ```

3.  **Create the Database:**
    The project is configured to use Entity Framework Core migrations to set up the database schema. Run the following command from the `Saas.Web` directory to create the initial database.
    ```sh
    dotnet ef database update
    ```
    This will create an `app.db` file in the `Saas.Web` directory, containing the tables required by ASP.NET Core Identity.

4.  **Run the Application:**
    You can run the application using the .NET CLI:
    ```sh
    dotnet run --project Saas.Web
    ```

5.  **Access the Application:**
    Once running, the application will be accessible at the following URLs:
    -   **HTTPS**: `https://localhost:7053`
    -   **HTTP**: `http://localhost:5075`

## Project Structure

-   **`Saas.Web.csproj`**: The main project file, defining the .NET version and all package dependencies.
-   **`Program.cs`**: The application's entry point, where services are configured and the HTTP request pipeline is defined.
-   **`appsettings.json`**: Contains configuration settings, including the `DefaultConnection` string for the SQLite database.
-   **`Data/ApplicationDbContext.cs`**: The Entity Framework Core database context. It inherits from `IdentityDbContext` to provide the Identity schema. Custom data models should be added here.
-   **`Controllers/`**: Contains the MVC controllers that handle web requests.
-   **`Views/`**: Contains the Razor views (`.cshtml` files) that define the application's UI.
-   **`wwwroot/`**: The directory for static assets like CSS, JavaScript, and images.

## Authentication

Authentication is managed by **ASP.NET Core Identity**. By default, it is configured with the following features:
- User registration and login.
- Email confirmation is required for a new user to sign in.
- The user interface for Identity is provided through Razor Pages located in the `Areas/Identity/` directory.

## Database

The application uses **SQLite** for data storage, which is configured in `Program.cs`. The connection string is located in `appsettings.json`. The database schema is managed via **Entity Framework Core Migrations**. To create a new migration after changing your data models, run:

```sh
dotnet ef migrations add "YourMigrationName"
```
To apply the migration to the database, run:
```sh
dotnet ef database update
```
