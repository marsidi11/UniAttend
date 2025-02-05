# UniAttend
## Electronic Attendance Management System

UniAttend automates student attendance tracking through card readers and OTP verification, designed specifically for educational institutions to manage and monitor student presence in real-time.

## User Roles & Features

### 👨‍💼 Administrator
- Department management
- Academic year configuration
- Curriculum and subject management
- User management (Secretary/Professor)
- System-wide reporting

### 👩‍💼 Academic Secretary
- Student registration and management
- Study group administration
- Student card management (RFID/Barcode simulation)
- Schedule management:
  - Create/edit class schedules
  - Assign classrooms
  - Set professor teaching hours
  - Define group timetables
- Professor-subject-group assignments
- Generate attendance and schedule reports

### 👨‍🏫 Professor
- View assigned teaching schedule
- Manage class sessions:
  - Start/end sessions
  - View student attendance
  - Verify attendance records
- View subject-group assignments
- Generate and confirm attendance reports
- Access personal teaching timetable

### 👨‍🎓 Student
- View personal class schedule
- View enrolled groups/subjects
- Record attendance:
  - Simulated card scanning (RFID/Barcode)
  - OTP verification via personal device
- Track attendance status per subject
- Receive absence notifications (20% threshold)

## 🛠️ Technology Stack

### Backend
- ASP.NET Core 8.0
- Entity Framework Core
- MySQL Database
- JWT Authentication
- Serilog Logging
- FastReport for PDF Generation
- OTP.NET for OTP Generation
- MailKit for Email Services

### Frontend
- Vue.js 3
- TypeScript
- Tailwind CSS
- Pinia State Management
- Axios
- VeeValidate

## Important Notes

### Card Reader Simulation
The system uses a simulated card reader interface for development and testing:
- Simulated RFID/Barcode scanning through API endpoints
- Test card numbers for development
- Network validation of simulated scans

## 🏗️ Project Architecture

```
UniAttend/
├── UniAttend.Core/           # Domain entities, interfaces
├── UniAttend.Application/    # Business logic, DTOs
├── UniAttend.Infrastructure/ # External services implementation
├── UniAttend.API/           # REST API endpoints
├── UniAttend.Web.Client/    # Vue.js frontend
└── UniAttend.Shared/        # Shared DTOs and utilities
```

## ✨ Key Features

### Multi-authentication Methods
- RFID/Barcode card scanning
- Network-validated OTP generation
- Role-based access control

### Real-time Attendance
- Card reader integration
- Network validation
- Session management
- Attendance confirmation workflow

### Automated Notifications
- Absence threshold alerts (20%)
- Email notifications

### Reporting System
- Attendance records
- Group-subject reports
- Department analytics
- PDF export capabilities

## 🚀 Getting Started

1. Configure database connection in `appsettings.json`
2. Run migrations: `dotnet ef database update`
3. Start API: `dotnet run --project UniAttend.API`
4. Start web client:
```bash
cd UniAttend.Web.Client
npm install
npm run serve
```

## ⚙️ Required Configurations
- Database connection string
- JWT authentication settings
- SMTP email configuration
- Admin credentials
- Card reader endpoints
- Network validation settings
- Logging configuration