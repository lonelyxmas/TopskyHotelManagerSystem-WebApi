<h1 align="center"><img src="https://foruda.gitee.com/avatar/1677165732744604624/7158691_java-and-net_1677165732.png!avatar100" alt="Organization Logo.png" /></h1>
<h1 align="center">TopskyHotelManagementSystem-WebApi</h1>
<p align="center">
		<a href='https://github.com/easy-open-meta/TopskyHotelManagerSystem-WebApi/stargazers'><img src='https://img.shields.io/github/stars/easy-open-meta/TopskyHotelManagerSystem-WebApi?style=social' alt='star'></img></a>
        <a href='https://github.com/easy-open-meta/TopskyHotelManagerSystem-WebApi/forks'><img src='https://img.shields.io/github/forks/easy-open-meta/TopskyHotelManagerSystem-WebApi' alt='fork'></img></a>
        <a href='https://img.shields.io/badge/license-MIT-000000.svg'><img src="https://img.shields.io/badge/license-MIT-000000.svg" alt=""></img></a>
        <a href='https://img.shields.io/badge/language-C#-red.svg'><img src="https://img.shields.io/badge/language-CSharp-red.svg" alt=""></img></a>
</p>
<div align="center">
	<p><a href="./README.md">中文文档</a> | English Document</p>
</div>


## Project Overview

This project constitutes the backend Web API component for the **TS Hotel Management System**, developed using **.NET 10**. Written in **C#** and utilising the **SqlSugar** ORM framework, it supports multiple databases including **MariaDB**, **MySQL**, **PostgreSQL**, and **SQL Server**.

Primarily designed to achieve front-end/back-end separation following the upgrade to version 2.0 of an existing client/server hotel management system, it provides comprehensive business interface support. This project excludes front-end UI components, delivering solely RESTful API services.

## Core Functional Features

### 1. Business Management Modules
- **Room Management**: Supports room status management (Vacant, Occupied, Under Maintenance, Dirty, Reserved), check-in/check-out, room transfers, and configuration (type, pricing).
- **Guest Management**: Guest profile management, account registration/login, membership tier administration.
- **Reservation Management**: Handles room bookings, automatic detection of expired reservations (Quartz-based), and email notification alerts.
- **Consumption Management**: Manages merchandise sales (including inventory), room charges (utilities, accommodation fees), and settlement status tracking.
- **Staff Management**: Oversees employee records, attendance tracking (clocking in/out), reward/disciplinary records, CV management, and ID photo storage.
- **Asset Management**: Hotel fixed asset management (asset ID, value, origin, handler).
- **News & Announcements**: Internal hotel news publishing, system announcement management.

### 2. System Management Module (RBAC)
- **Administrator Management**: Supports super administrators, regular administrators, administrator types.
- **Roles and Permissions**: Role-Based Access Control (RBAC), granular permission allocation (menu permissions, button permissions).
- **Menu Management**: Dynamic menu configuration, supports role-based menu tree construction.
- **Data Dictionary**: Supports maintenance of foundational data such as ethnicity, educational background, department, position, ID type, etc.

### 3. Data Statistics and Monitoring
- **Management Dashboard**: Provides room status statistics (Vacant/Occupied/Under Maintenance/Uncleaned/Reserved), operational metrics (member gender ratio, monthly/annual consumption trends), inventory alerts, and staff attendance records (attendance/late arrivals/absenteeism).
- **Operational Logs**: Detailed HTTP request logs (duration, parameters, IP) and business operation logs.
- **Supervisory Statistics**: Facilitates entry and querying of internal hotel oversight data.

### 4. Infrastructure and Security
- **Multi-Database Support**: Enables seamless switching between mainstream relational databases via SqlSugar, with one-click database and table initialisation.
- **Security Mechanisms**:
    - **JWT**: Secure token authentication mechanism.
    - **CSRF**: Prevents cross-site request forgery attacks.
    - **Data Protection**: Sensitive data (e.g., ID numbers, contact details) encrypted and stored using ASP.NET Core Data Protection API.
    - **Request Logging**: Global request middleware records API call details.
- **Scheduled Tasks**: Utilises Quartz .NET to process scheduled expiry reminder email dispatches.
- **Third-Party Integrations**:
    - **Email Service**: MailKit integration for sending reminder emails (HTML templates).
    - **Image Hosting Service**: Lsky integration for image uploads and hosting.

## Technology Stack

- **.NET 10**: Foundational framework.
- **C# 12**: Development language.
- **SqlSugar**: ORM framework (multi-tenant/multi-database support, high performance).
- **Autofac**: IoC container (dependency injection).
- **Quartz .NET**: Scheduled task management (handling expired reservations).
- **MailKit**: Email sending library.
- **NSwag**: API documentation generation (Swagger UI).
- **Newtonsoft.Json**: JSON serialisation library.

## Project Structure

The project employs a layered architecture design, featuring a clear structure that facilitates maintenance and extensibility:

```text
EOM.TSHotelManagement.Web
├─ EOM.TSHotelManagement.API          # API entry layer (Controllers, Middleware, Config)
├─ EOM.TSHotelManagement.Contract     # Contract layer (DTO, Request/Response Models, Service Interfaces)
├─ EOM.TSHotelManagement.Domain       # Domain layer (Entities, Domain Logic)
├─ EOM.TSHotelManagement.Service      # Service Layer (Business Logic Implementation)
├─ EOM.TSHotelManagement.Data         # Data Layer (DbContext, Repositories, Database Init)
├─ EOM.TSHotelManagement.Infrastructure # Infrastructure Layer (Config Models, Helpers, JWT Config)
├─ EOM.TSHotelManagement.Common       # Common Layer (Utils, Constants, Enums, Email Templates)
└─ EOM.TSHotelManagement.Migration    # Database Migration Tool
```

## Database Support

This project utilises the SqlSugar framework to support one-click database and table creation across multiple databases:

| Database     | Version             | Supported Database/Table Creation | Status |
| ---------- | ---------------- | :----------: | :--: |
| MariaDB    | 10.11.10+        |      ✅       |  ✅   |
| PostgreSQL | 13+              |      ✅       |  ✅   |
| MySQL      | 5.7+             |      ✅       |  ✅   |
| SQL Server | 2022+            |      ✅       |  ✅   |
| Oracle     | -                |      ❌       |  ❌   |
| SQLite     | -                |      ❌       |  ❌   |

## Quick Start

### Environment Requirements
- .NET 10 SDK
- Runtime 10.x
- Visual Studio 2026 (or VS Code + C# Dev Kit)

### Running Locally

1. **Clone the project**:
   ```bash
   git clone https://gitee.com/java-and-net/topsky-hotel-management-system-web-api.git
   ```

2. **Configure the database**:
   Modify `EOM.TSHotelManagement.API/appsettings.json`:
   ```json
   {
     "DefaultDatabase": "MariaDB", // Optional values: MariaDB, MySql, PgSql, SqlServer
     "ConnectionStrings": {
       "MariaDB": "Server=localhost;Database=tshoteldb;User=root;Password=123456;"
       // ...
     },
     "InitializeDatabase": true // ⚠️ Set to true on first run to automatically create database and table structure
   }
   ```

3. **Configure Keys**:
   Set `Jwt__Key` (for token generation) and `DataProtection`-related keys (for data encryption/decryption) in `appsettings.json`.

4. **Running the Project**:
   Open `EOM.TSHotelManagement.Web.sln` in Visual Studio and start the `EOM.TSHotelManagement.API` project.

### Docker Deployment

The project provides a Dockerfile (alternatively, images can be rapidly built via the `build.ps1` script, provided WSL 2.0 and Hyper-V are enabled locally and Docker Desktop is installed), supporting Docker containerised deployment. The API listens on port 8080 by default.

```bash
# Please modify environment variable parameters according to your actual setup
docker run -d \
  --name tshotel-api \
  --health-cmd="curl -f http://localhost:8080/health || exit 1" \
  --health-interval=30s \
  --health-timeout=10s \
  --health-retries=3 \
  -v /app/config:/app/config \
  -v /app/keys:/app/keys \
  -p 63001:8080 \
  -e ASPNETCORE_ENVIRONMENT=docker \
  -e DefaultDatabase=MariaDB \
  -e MariaDBConnectStr="Server=your_db_host;Database=tshoteldb;User=tshoteldb;Password=your_password;" \
  -e InitializeDatabase=true \
  -e Jwt__Key=your_super_secret_key \
  -e Jwt__ExpiryMinutes=20 \
  -e Mail__Enabled=true \
  -e Mail__Host=smtp.example.com \
  -e Mail__UserName=admin@example.com \
  -e Mail__Password=your_email_password \
  -e Mail__Port=465 \
  -e AllowedOrigins__0=http://localhost:8080 \
  -e AllowedOrigins__1=https://www.yourdomain.com \
  yjj6731/tshotel-management-system-api:latest
```

| Parameter Name | Parameter Description | Required (Y/N) | Default Value | Available Values |
|------|------|---------|-----|-----|
|name|Container Name|Y|N/A|N/A|
|DefaultDatabase|Default Database|Y|N/A|MariaDB/MySQL/SQL Server/PostgreSQL|
|ASPNETCORE_ENVIRONMENT|System Environment (determines Dataprotection Key generation location and environment detection)|Y|docker|docker|
|{Default Database (e.g.: MariaDB/MySQL/SQL Server/PostgreSQL)}ConnectStr|Corresponding Database Connection String|Y|N/A|N/A|
|Jwt__Key|JWT Key|Y|None, must be set|N/A|
|Jwt__ExpiryMinutes|Token Validity Period (Minutes)|Y|20|N/A|
|Lsky__Enabled|Enable Lsky image hosting integration|Y|false|true/false|
|Lsky__BaseAddress|Lsky image hosting base address|Y|N/A|N/A|
|Lsky__Email|Lsky account email|Y|N/A|N/A|
|Lsky__Password|Lsky account password|Y|N/A|N/A|
|Lsky__UploadApi|Lsky Image Hosting Upload Interface|Y|N/A|N/A|
|Lsky__GetTokenApi|Lsky Image Hosting Token Retrieval Interface|Y|N/A|N/A|
|Mail__Enabled|Enable email service|Y|true|true/false|
|Mail__Host|Email SMTP protocol address|Y|smtp.example.com|N/A|
|Mail__UserName|Email SMTP protocol address|Y|N/A|N/A|
|Mail__Port|Email SMTP port|Y|465|N/A|
|Mail__Password|Email password|Y|N/A|N/A|
|Mail__EnableSsl|Enable SSL|Y|true|true/false|
|Mail__DisplayName|Sender display name|Y|N/A|N/A|
|InitializeDatabase|Initialise database|N|true|true/false|
|ExpirationSettings__NotifyDaysBefore|Days Before Expiration Notification|Y|3|a few days|
|ExpirationSettings__CheckIntervalMinutes|Notification Check Interval|Y|5|a few minutes|
|AllowedOrigins__0|Allowed Domain Sites (for Development Environments)|Y|http://localhost:8080|http://localhost:8080|
|AllowedOrigins__1|Allowed domain sites for production environment|Y|https://www.yourdomain.com|https://www.yourdomain.com|
|SoftwareVersion|Software version number for documentation purposes|N|N/A|N/A|
|JobKeys__0|Quartz Job 1|Y|ReservationExpirationCheckJob|ReservationExpirationCheckJob|
|JobKeys__1|Quartz Job 2|Y|MailServiceCheckJob|MailServiceCheckJob|
|JobKeys__2|Quartz Job 3|Y|RedisServiceCheckJob|RedisServiceCheckJob|
|Redis__Enabled|Enable Redis|N|false|true/false|
|Redis__ConnectionString|Redis ConnectString|N|N/A|N/A|
|Redis__DefaultDatabase|Default Database of Redis|N|0|0|

> ⚠️ **Security Advisory**: In production environments, do not directly pass password-like parameters in plaintext via the `-e` flag. It is recommended to utilise Docker Secrets or environment variable injection tools (such as HashiCorp Vault) for protection.

## Acknowledgements

We extend our gratitude to the following outstanding open-source projects:

1. **Autofac** - An addictive .NET IoC container. ([MIT](https://github.com/autofac/Autofac))
2. **SqlSugar** - China's most popular ORM framework. ([MIT](https://gitee.com/dotnetchina/SqlSugar))
3. **Mailkit** - A cross-platform .NET library for IMAP, POP3, and SMTP. ([MIT](https://github.com/jstedfast/MailKit))
4. **NSwag** - The OpenAPI/Swagger API toolchain for .NET and TypeScript. ([MIT](https://github.com/NSwag/NSwag))
5. **Quartz .NET** - Open-source job scheduling system for .NET. ([Apache 2.0](https://github.com/quartznet/quartznet))

## Project Contributors

- **Jackson** (Project Lead, core code author and later project integration)
- **Benjamin** (Development, project code authoring)
- **Bin** (Database Support)
- **Easy Open Meta** (Later Maintenance and Development)

## Licence

This project is open-sourced under the **MIT** licence. It is free and open-source, but please refrain from using it for commercial purposes (please read the LICENCE file for specifics).
