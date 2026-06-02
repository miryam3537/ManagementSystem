# SYSTEMTEST - Auction System

## 📌 Overview

SYSTEMTEST is a real-time auction system built with **ASP.NET Core Web API**, **Entity Framework Core**, and **SignalR**.
The system allows users to view auctions and place bids in real time, with live updates pushed to all connected clients.

## ⚙️ Technologies Used

* ASP.NET Core Web API
* Entity Framework Core
* SignalR (Real-time communication)
* SQL Server
* C#
* Background Services
* REST API Architecture

## 🧩 Main Features

* Create and manage auctions
* Place bids on active auctions
* Real-time bid updates using SignalR
* Optimistic concurrency control using RowVersion
* Background service for auction processing
* Clean layered architecture (Controllers, Services, DTOs)

## 📁 Project Structure

* Controllers → API endpoints
* Services → Business logic layer
* DTOs → Data transfer objects
* Entities → Database models
* Data → DbContext (EF Core)
* Hubs → SignalR real-time hub
* Middleware → Global exception handling
* Migrations → Database migrations

## 🚀 API Endpoints

### Auctions

* `GET /api/auctions` → Get all auctions
* `GET /api/auctions/{id}` → Get auction by ID
* `POST /api/auctions/{id}/bid` → Place a bid

## 📡 Real-Time Features

The system uses SignalR (`AuctionHub`) to broadcast new bids instantly to all connected clients.

## 🗄️ Database

The project uses Entity Framework Core with SQL Server.
Run migrations to create the database:

```bash
dotnet ef database update
```

## ▶️ How to Run

1. Clone the repository

```bash
git clone https://github.com/your-username/SYSTEMTEST.git
```

2. Restore dependencies

```bash
dotnet restore
```

3. Run the project

```bash
dotnet run
```

## 📌 Notes

* Make sure SQL Server is running
* Update connection string in `appsettings.json`
* SignalR requires client connection for real-time updates

## 👩‍💻 Author

Built by Miryam Yellin
