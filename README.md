
# 📰 Blog API (.NET 8)

A simple ASP.NET Core Web API for managing blog posts with **JWT authentication** and **SQL Server integration**.

---

## 🚀 Features

- CRUD operations for blog posts (Create, Read, Update, Delete)
- Secure authentication using JWT
- Role-based API protection
- Entity Framework Core with SQL Server
- Swagger UI for testing
- CORS configured for React frontend (`http://localhost:3000`)

---

## 🛠️ Tech Stack

- **.NET 8**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **Swagger (OpenAPI)**

---

## ⚙️ Prerequisites

Before running the project, make sure you have the following installed:

- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Git](https://git-scm.com/)
- Optional: [Postman](https://www.postman.com/) for API testing

---

## 📂 Project Setup

### Clone the Repository

```bash
git clone https://github.com/prakashbudala/blog-api.git
cd blog-api
## ⚙️ Update connection string
Open appsettings.json in the root of your project and find this section:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=BlogDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

### Apply Database Migrations
Run the following commands in the terminal (inside the project folder):

dotnet ef migrations add InitialCreate
dotnet ef database update

###  Run the API
dotnet run

