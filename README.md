# Dot_Net_Final_Project — Hotel Management System (ASP.NET Core MVC)

A role-based **Hotel Management System** built with **ASP.NET Core MVC** and a layered architecture (**App + BLL + DAL**) using **Entity Framework Core** and **SQL Server**.  
It supports hotel operations such as room booking with date-based availability, reservations management (check-in/check-out/cancel), payments, employee approval, and admin reporting.

## Tech Stack

- **Backend:** C# / ASP.NET Core MVC
- **Data Access:** Entity Framework Core (SQL Server)
- **Frontend:** Razor Views (HTML + Bootstrap)
- **Architecture:** 3-layer
  - `App` (UI + Controllers)
  - `BLL` (Services + DTOs + AutoMapper config)
  - `DAL` (DbContext + EF models + repositories)
 
## Code Structure

This solution follows a 3‑layer architecture:

- **App** (Presentation/UI): MVC Controllers + Razor Views
- **BLL** (Business Logic Layer): Services + DTOs + mapping (AutoMapper)
- **DAL** (Data Access Layer): EF Core DbContext + Entities + Repositories

### High-level folder layout

```text
Dot_Net_Final_Project/
├── Final_Project.slnx
├── App/
│   ├── Program.cs
│   ├── appsettings.json
│   │
│   ├── Controllers/
│   │   ├── AdminController.cs
│   │   ├── AuthController.cs
│   │   ├── CustomerController.cs
│   │   ├── EmployeeController.cs
│   │   ├── HomeController.cs
│   │   ├── ReceptionistController.cs
│   │   ├── RegistrationController.cs
│   │   ├── RoomManagementController.cs
│   │   └── UserManagementController.cs
│   │
│   │
│   ├── Views/
│   │   │
│   │   ├── Admin/
│   │   │   ├── Dashboard.cshtml
│   │   │   ├── PendingEmployees.cshtml
│   │   │   └── ViewPayments.cshtml
│   │   │
│   │   ├── Auth/
│   │   │   └── Login.cshtml
│   │   │
│   │   ├── Customer/
│   │   │   ├── AvailableRooms.cshtml
│   │   │   └── Dashboard.cshtml
│   │   │
│   │   ├── Employee/
│   │   │   └── Dashboard.cshtml
│   │   │
│   │   ├── Home/
│   │   ├── Receptionist/
│   │   ├── Registration/
│   │   ├── RoomManagement/
│   │   ├── Shared/
│   │   └── UserManagement/
|   |
├── BLL/
│   ├── MapperConfig.cs
│   ├── DTOs/
│   │   ├── CustomerDTO.cs
│   │   ├── CustomerDashboardDTO.cs
│   │   ├── CustomerpDTO.cs
│   │   ├── DashboardDTO.cs
│   │   ├── EmployeeDTO.cs
│   │   ├── EmployeeDashboardDTO.cs
│   │   ├── LoginDTO.cs
│   │   ├── PaymentDTO.cs
│   │   ├── PendingEmployeeDTO.cs
│   │   ├── RecetionistDTOs.cs
│   │   ├── ReservationDTO.cs
│   │   ├── RoomDTO.cs
│   │   ├── RoomTypeDTO.cs
│   │   └── UserDTO.cs
│   └── Seervices/
│       ├── AdminService.cs
│       ├── AuthService.cs
│       ├── CustomerService.cs
│       ├── EmployeeService.cs
│       ├── ReceptionistService.cs
│       ├── RegistrationService.cs
│       ├── RoomManagementService.cs
│       ├── UserManagementService.cs
│       └── practice.cs
│
├── DAL/
│   ├── EF/
│   │   ├── HotelManagementContext.cs
│   │   └── Tables/
│   │       ├── Customer.cs
│   │       ├── Employee.cs
│   │       ├── Payment.cs
│   │       ├── Reservation.cs
│   │       ├── ReservationRoom.cs
│   │       ├── Room.cs
│   │       ├── RoomType.cs
│   │       └── User.cs
│   └── Repos/
│       ├── AdminRepo.cs
│       ├── AuthRepo.cs
│       ├── CustomerRepo.cs
│       ├── EmployeeRepo.cs
│       ├── ReceptionistRepo.cs
│       ├── RegistrationRepo.cs
│       ├── RoomManagementRepo.cs
│       └── UserManagementRepo.cs
│
└── DLL/
    ├── BLL.csproj
    └── Class1.cs
```

### Layer responsibilities

- **Controllers (App)**: Receive requests, call BLL services, return Views.
- **Services (BLL)**: Contain business rules, validation, and mapping to DTOs.
- **Repos (DAL)**: Contain database queries and EF Core operations.

## Repository Structure

- `App/` — ASP.NET Core MVC web project (Controllers, Views, wwwroot)
- `BLL/` — Business logic layer (DTOs, Services, AutoMapper configuration)
- `DAL/` — Data access layer (EF DbContext, Entities, Repositories)
- `Final_Project.slnx` — Solution file including `App`, `BLL`, `DAL`

## Main Modules / Controllers (App Layer)

From `App/Controllers`:

- **AuthController** — login/auth flows
- **RegistrationController** — user registration
- **AdminController** — admin dashboard/reporting/management features
- **UserManagementController** — user management
- **RoomManagementController** — manage rooms/room types/statuses
- **ReceptionistController** — reservations handling (search, check-in/out, cancel)
- **CustomerController** — customer dashboard, search/book rooms, reservation history
- **EmployeeController** — employee dashboard/work tasks (e.g., housekeeping/maintenance)
- **HomeController** — general navigation/pages

## Core Features (based on project chats)

### Customer
- View dashboard (profile/VIP status, reservation history, payment history)
- Search available rooms by:
  - Check-in / Check-out date range
  - Room type
  - Min/Max price
- Book room (prevents double booking by checking overlapping dates)
- Cancel reservation (optionally removes payment record on cancel, depending on implementation)

### Receptionist
- Search reservations by:
  - Customer name
  - Date
  - Status
- Actions:
  - **Check-In** (e.g., set reservation status + room to *Occupied*)
  - **Check-Out** (e.g., set reservation to *Completed* and room to *Cleaning*)
  - **Cancel** reservation

### Admin
- Dashboard metrics (intended to be real DB stats, not demo):
  - Revenue today / this month
  - Occupancy rate
  - Reservation counts by status (confirmed/pending/cancelled)
  - Pending employee approvals
- Employee approvals:
  - Approve/Reject employees
  - Approve while setting/updating salary
- Payments view:
  - View all payments with **Customer name + Room number + Payment details**
- Reporting:
  - Revenue report generation by date range (Start Date / End Date)

### Employee
- Employee dashboard and workflow support (e.g., room cleaning tasks triggered after checkout)

## Getting Started

### Prerequisites
- Visual Studio 2022 (recommended) or VS Code
- .NET SDK (matching the project’s target framework in `App/App.csproj`)
- SQL Server / SQL Express

### Configure Database
The application uses a SQL Server connection string in:

`App/appsettings.json`
```json
"ConnectionStrings": {
  "DbConn": "server=DESKTOP-QR71IO6\\SQLEXPRESS; initial catalog=hotel_management; TrustServerCertificate=True; Integrated Security=True;"
}
```

Update this connection string to match your SQL Server instance.

### Run the App
1. Open `Final_Project.slnx` in Visual Studio
2. Restore NuGet packages
3. Update the connection string
4. Run the `App` project

The default route is configured to start at:
- `Auth/Login`

## Notes (Important)
- The project is configured with **Session** support in `App/Program.cs`.
- Dependency injection is set up for repos/services for:
  - Registration, Auth, Admin, UserManagement, Employee, RoomManagement, Receptionist, Customer
- AutoMapper is registered through `BLL.MapperConfig`.

## Screenshots / Demo (Optional)
Add screenshots under a folder like:
- `assets/Screenshot 2026-05-18 225313.png`
and link them here in a table for a professional GitHub presentation.

## License
This repository currently does not specify a license. If you want, add a `LICENSE` file (e.g., MIT).
