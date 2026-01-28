Gym Management System | ASP.NET Core MVC

A backend-focused Gym Management System built with ASP.NET Core MVC, following Clean Architecture and real-world business logic. The system goes beyond CRUD operations, including booking management, membership management, user management, and attendance tracking.

---


🚀 Key Features

Booking Management
Manage sessions with categories and trainers.

Book members for upcoming sessions.

Track member attendance for ongoing sessions.

Cancel bookings with validation on session start date.

Calculate available slots dynamically.

Views for Upcoming Sessions and Ongoing Sessions with action buttons.

Membership Management
Create memberships for members with active plans.

Cancel active memberships.

Display all active memberships with member and plan info.

Dropdown lists for members and plans integrated into Views.

Validation for member existence, plan existence, and active membership check.

User & Authentication Management
Register and login users using ASP.NET Core Identity.

Role-based access control (RBAC) for Admin and SuperAdmin.

Retrieve users by roles.

Password validation and role assignment during registration.

Technical Highlights
Clean Architecture: Clear separation of Presentation, Business Logic, and Data Access layers.

Repositories & Unit of Work: Generic repositories with Unit of Work pattern.

EF Core Code-First: Entity configurations and relationships defined with Fluent API.

AutoMapper: Maps entities to ViewModels seamlessly.

Validation & Alerts: Server-side and client-side validation with Bootstrap alerts.

Responsive Views: Built with Bootstrap 5, compatible with desktop and mobile.

---


⚙️ Technologies & Tools

.NET 9 / ASP.NET Core MVC

Entity Framework Core (Code-First)

SQL Server

ASP.NET Core Identity

AutoMapper

Bootstrap 5 & Select2

Dependency Injection

Unit of Work & Generic Repository Pattern

---


✅ Business Logic Implementation

Booking Workflow
Check if session exists and is upcoming.

Verify member has an active membership.

Prevent duplicate bookings.

Validate capacity before booking.

Track attendance with MemberAttended.

Cancel booking if session hasn't started.

Membership Workflow
Validate member and plan existence.

Check for existing active memberships.

Calculate membership end date based on plan duration.

Cancel membership if active.

User Management
Register new users with roles.

Validate login credentials.

Retrieve users based on roles.

---


⚡ Installation & Setup

Clone the repository:
git clone https://github.com//Gym-Management-System.git

Navigate to the project folder:
cd Gym-Management-System

Configure appsettings.json with your SQL Server connection string.

Apply migrations and update database:

dotnet ef database update

Run the project:
dotnet run

Open the application in your browser:
https://localhost:5001

Select2 Integration: Enhanced dropdowns for member selection in bookings.

