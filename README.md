# UniAttend - Attendance Management System

## Overview
UniAttend is a comprehensive attendance management system built for educational institutions using ASP.NET Core 8.0. It enables efficient tracking and management of student attendance with features like real-time monitoring, OTP verification, and detailed analytics.

## Features
- Multi-role authentication (Admin, Professor, Student)
- Real-time attendance tracking
- OTP-based attendance verification
- Card-based attendance recording
- Detailed attendance reports and analytics
- Course and session management
- Email notifications
- Mobile-friendly interface
- PDF report generation

## Technology Stack
- **Backend:** ASP.NET Core 8.0
- **Frontend:** Vue.js
- **Database:** MySQL
- **Authentication:** JWT Bearer
- **Documentation:** Swagger/OpenAPI
- **Logging:** Serilog
- **PDF Generation:** iText7
- **OTP:** Otp.NET

## Architecture
Project follows Clean Architecture with:
- `UniAttend.Core`: Domain entities and interfaces
- `UniAttend.Application`: Business logic and DTOs
- `UniAttend.Infrastructure`: External implementations
- `UniAttend.API`: API endpoints and configuration
- `UniAttend.Web.Client`: Frontend application
- `UniAttend.Shared`: Shared DTOs and utilities

## Getting Started
1. Clone the repository
2. Configure database connection in `appsettings.json`
3. Run migrations: `dotnet ef database update`
4. Start API: `dotnet run --project UniAttend.API`
5. Start web client: `cd UniAttend.Web.Client && npm install && npm run serve`

## Configuration Requirements
- Database connection string
- JWT settings
- SMTP email settings
- Admin account credentials
- Logging paths
- CORS policies