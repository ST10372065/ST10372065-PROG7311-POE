# Agri-Energy Connect

A collaborative platform for sustainable agriculture and renewable energy solutions, built with ASP.NET Core Razor Pages (.NET 8).

---

## Table of Contents

- [Project Overview](#project-overview)
- [System Functionalities](#system-functionalities)
- [User Roles](#user-roles)
- [Development Environment Setup](#development-environment-setup)
- [Building and Running the Prototype](#building-and-running-the-prototype)
- [Troubleshooting](#troubleshooting)

---

## Project Overview

**Agri-Energy Connect** enables farmers and employees to collaborate, share resources, and manage products related to sustainable agriculture and renewable energy. The platform features user authentication, product management, and role-based access. Agri-Energy Connect aims to bridge the gap between the agricultural secotr and green energry technology provicers.
---

## System Functionalities

- **User Authentication:** Secure login and logout for all users.
- **Role-Based Access:**
  - **Farmers** can add and manage their own products.
  - **Employees** can view, filter, and manage all products and users.
- **Product Management:** Add, view, and filter products by category, date, and farmer.
- **Error Handling:** User-friendly error messages and secure handling of sensitive information.

---

## User Roles

### Farmer

- Can log in.
- Can add new products (e.g., crop seeds, solar) with details and images.
- Can view only their own products.

### Employee

- Can register a farmer and log in.
- Can view all products from all farmers.
- Can filter products by farmer, category, and date.

---

## Development Environment Setup

### Prerequisites

- **Visual Studio 2022** (or later) with .NET 8 SDK installed.
- **Git** (for source control, optional).
- **SQLite** (used as the local database, no separate installation required).

### Step-by-Step Setup

1. **Clone the Repository**



2. **Open the Solution**
   - Launch Visual Studio 2022.
   - Open the `ST10372065-PROG7311.sln` file.

3. **Restore NuGet Packages**
   - Visual Studio will automatically restore packages on build.
   - If not, right-click the solution and select __Restore NuGet Packages__.

4. **Database Setup**
   - The project uses SQLite (`local.db`) and will create the database automatically on first run.
   - No manual migration is required for initial setup.

---

## Building and Running the Prototype

1. **Build the Solution**
   - In Visual Studio, select __Build > Build Solution__ or press `Ctrl+Shift+B`.

2. **Run the Application**
   - Press `F5` or click the __Start Debugging__ button.
   - The application will launch in your default browser at `https://localhost:<port>/`.

3. **Login and Use**
   - Register as a Farmer or Employee.
   - Log in to access role-specific features.
   - Employees can add new users and view all products.
   - Farmers can add and manage their own products.

---

## Troubleshooting

- **Port Conflicts:** If the application fails to start, ensure no other application is using the same port.
- **Database Issues:** Delete `local.db` in the project root to reset the database if needed.
- **Authentication Errors:** Ensure cookies are enabled in your browser.

