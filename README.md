# Stay Mate Hotel - Web Service

This is the Web Service (Server) portion of our final project of the Service Oriented Software Development course.

## Table of Contents

-   [Technologies Used](#technologies-used)
-   [Required Dependencies](#required-dependencies)
-   [Installation](#installation)
-   [Before You Run](#before-you-run)
-   [Development](#development)
-   [Connect other devices to this server](#connect-other-devices-to-this-server)
-   [Features](#features)
-   [Suggested VS Code Extensions](#suggested-vs-code-extensions)
-   [Contributors](#contributors)

## Technologies Used

-   [.NET Framework](https://dotnet.microsoft.com/en-us/)
-   [Entity Framework](https://learn.microsoft.com/en-us/ef/)
-   [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
-   [Swagger/OpenAPI](https://swagger.io/)

## Required Dependencies

-   `.NET 8.0.403 SDK`
-   `Entity Framework Core`
-   `Microsoft SQL Server 2022 - Express edition`
-   `Microsoft SQL Server Management Studio - SSMS`

Make sure to have these installed before proceeding with the project setup.

**Note:** If you encounter any issues installing the `.NET runtime`, consider installing it through `Visual Studio`. Details are provided below.

## Installation

### Install `dotnet runtime` through `Visual Studio`

Follow these steps to download Visual Studio 2022 and dotnet runtime.

1. Download Visual Studio 2022:

    - Visit the [official Visual Studio 2022 download page](https://visualstudio.microsoft.com/downloads/).
    - Choose the `Community` edition (free) or the version that suits your needs.
    - Click `Download` and follow the installation prompts.

2. Launch the Visual Studio Installer:

    - After downloading the installer, open it to start setting up Visual Studio.
    - You'll be prompted to select workloads to install.

3. Select the .NET Workloads and start the installation:

    - In the `Workloads` tab, find the section labeled `ASP.NET and web development`.
    - Check the box for `ASP.NET and web development` to install all necessary tools.
    - Click `Install` to download and install the selected features.

4. Verify .NET Runtime:

    - After the installation, you can verify the installed `.NET runtime` by opening a terminal and typing the following command:

        ```bash
        dotnet --version
        ```

    - It should return the version of the `.NET SDK` installed by Visual Studio.

### Cloning the project and install dependencies

Follow these steps to set up and run the application locally.

1. Clone the repository:

    ```bash
    git clone https://github.com/YGOhappy123/StayMateHotel-WebService.git
    ```

2. Change project name to `server` to avoid namespace conflicting **(Important)**:

    ```bash
    mv StayMateHotel-WebService server
    ```

3. Navigate to the project directory:

    ```bash
    cd server
    ```

4. Install dependencies:

    ```bash
    dotnet restore
    ```

## Before You Run

Before running the project, make sure to set up the database and environment variables:

1. Create a `appsettings.Development.json` file:

    In the root directory of your project (at the same level as `Program.cs`), create a `appsettings.Development.json` file.

2. Add the following content to `appsettings.Development.json`:

    ```json
    {
        "ConnectionStrings": {
            "HotelDbConnectionString": "Data Source=<your_server_name>;Initial Catalog=StayMateHotel;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        }
    }
    ```

    **Notes:**

    - You must replace `<your_server_name>` with the actual value of your SQL Server instance.

    - Ensure that `StayMateHotel` (the database name) is correct, as the application uses migrations that depend on this database.

3. Apply the migrations to create the necessary database tables. Use the following command in the terminal:

    ```bash
    dotnet ef database update
    ```

    This command will apply the existing migrations to the specified database, ensuring that the required tables are created.

    **Note:** If you encounter any errors like `dotnet-ef: command not found`, install the `dotnet-ef` tool globally on your machine using the following command an retry:

    ```bash
    dotnet tool install --global dotnet-ef --version 8.0.10
    ```

4. Verify the created database:

    If you don't see a database name `StayMateHotel` in your SQL Server Management Studio after refreshing, try disconnecting and reconnecting your SQL Server.

## Development

To start the development server, use:

```bash
dotnet watch run
```

This will start the Dotnet development server

You can access the app by visiting `http://localhost:5000/api/v1` in your browser.

You can also replace `localhost` with your device's `IPv4 Address`, which can be found by entering the following command in the `terminal` and look for `Wireless LAN adapter Wi-Fi` > `IPv4 Address`:

```bash
ipconfig
```

## Connect Other Devices To This Server

**Requirement:** All devices must be connected to the same network.

Follow these steps to ensure that your firewall allows incoming connections on port 5000.

1. Open `Windows Defender Firewall`.
2. Click on `Advanced settings`.
3. Select `Inbound Rules` and then `New Rule`.
4. Choose `Port`, click `Next`.
5. Select `TCP` and enter `5000` in the specific local ports box.
6. Allow the connection and complete the wizard.

Now you can access the app using other devices by visiting `http://<IPv4 Adddess>:5000/api/v1`

## Features

-   **RESTful API** 🛠 Exposes endpoints following REST principles for ease of use and scalability.
-   **Database Integration** 💾 Uses SQL Server with Entity Framework Core for data persistence.
-   **Swagger Documentation** 📜 Automatically generated API documentation with Swagger/OpenAPI.
-   **Authentication and Authorization** 🔑 Secure your API with JWT-based authentication.
-   **Cross-Platform** 🌐 Runs on any operating system that supports .NET Core.
-   **Migrations** 🔄 Easily handle database schema changes using Entity Framework Core migrations.

## Suggested VS Code Extensions

| Extension                     | Publisher            | Required? | Supported features                                     |
| :---------------------------- | :------------------- | :-------: | :----------------------------------------------------- |
| C# Dev Kit                    | Microsoft            |    Yes    | Install necessary tools for developing C# and .NET app |
| .NET Extension Pack           | Microsoft            |    Yes    | Install necessary tools for developing C# and .NET app |
| CSharpier - Code formatter    | csharpier            |    Yes    | Code formatting                                        |
| NuGet Gallery                 | pcislo               |    Yes    | Streamlining the process of managing NuGet packages    |
| IntelliCode for C# Dev Kit    | Microsoft            |    No     | AI-assisted development for C# Dev Kit                 |
| C# Extensions                 | JosKreativ           |    No     | Add C# related stuffs to VS Code context menu          |
| Code Spell Checker            | Street Side Software |    No     | Spelling checker for source code                       |
| Multiple cursor case preserve | Cardinal90           |    No     | Preserves case when editing with multiple cursors      |

## Contributors

Thanks to the following people for contributing to this project ✨:

<table>
    <tr>
        <td align="center">
            <a href="https://github.com/YGOhappy123">
                <img 
                    src="https://avatars.githubusercontent.com/u/90592072?v=4"
                    alt="YGOhappy123" width="100px;" height="100px;" 
                    style="border-radius: 4px; background: #fff;"
                /><br />
                <sub><b>YGOhappy123</b></sub>
            </a>
        </td>
        <td align="center">
            <a href="https://github.com/DinhToanIT2003">
                <img 
                    src="https://avatars.githubusercontent.com/u/126399422?v=4"
                    alt="DinhToanIT2003" width="100px;" height="100px;"                 
                    style="border-radius: 4px; background: #fff;"
                /><br />
                <sub><b>DinhToanIT2003</b></sub>
            </a>
        </td>
        <td align="center">
            <a href="https://github.com/Nguyen1609">
                <img 
                    src="https://avatars.githubusercontent.com/u/126648891?v=4"
                    alt="Nguyen1609" width="100px;" height="100px;"
                    style="border-radius: 4px; background: #fff;"
                /><br />
                <sub><b>Nguyen1609</b></sub>
            </a>
        </td>
        <td align="center">
            <a href="https://github.com/vthanhdat99">
                <img
                    src="https://avatars.githubusercontent.com/u/108580228?v=4"
                    alt="vthanhdat99" width="100px;" height="100px;" 
                    style="border-radius: 4px; background: #fff;"
                /><br />
                <sub><b>vthanhdat99</b></sub>
            </a>
        </td>
    </tr>
</table>
