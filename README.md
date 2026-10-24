# MediCore — Healthcare & Hospital Management System

A comprehensive multi-tenant healthcare and hospital management system built with **ASP.NET Core 9.0** backend and **Angular 19** frontend. MediCore enables hospitals to manage appointments, doctors, patients, and various healthcare services including pharmacy, laboratory, and imaging services.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

- **Domain Layer**: Core business entities and enums
- **Application Layer**: Business logic, CQRS commands/queries, DTOs
- **Infrastructure Layer**: Data access, repositories, external services
- **API Layer**: RESTful API endpoints, middleware, authentication

### Design Patterns Used

- ✅ **Repository Pattern**: Abstraction over data access
- ✅ **Unit of Work Pattern**: Transaction management
- ✅ **CQRS Pattern**: Separation of read and write operations using MediatR
- ✅ **Dependency Injection**: Built-in .NET DI container
- ✅ **Multi-Tenancy**: Tenant isolation at data level

## 🚀 Technology Stack

### Backend
- **Framework**: ASP.NET Core 9.0
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core 9.0
- **Authentication**: JWT Bearer Tokens
- **CQRS**: MediatR
- **API Documentation**: Swagger/OpenAPI

### Frontend
- **Framework**: Angular 19
- **UI Libraries**: Tailwind CSS + Bootstrap 5
- **HTTP Client**: Angular HttpClient with Interceptors
- **Routing**: Angular Router with Guards
- **State Management**: RxJS Observables

## 📋 Features

### Core Functionality
- 🏥 **Multi-Tenant System**: Each hospital operates as a separate tenant
- 👨‍⚕️ **Doctor Management**: Profile, specialization, availability
- 👤 **Patient Management**: Medical history, appointments, prescriptions
- 📅 **Appointment Booking**: In-clinic and home visit appointments
- ⏰ **Time Slot Management**: Doctor availability scheduling
- 💊 **Prescription Management**: Digital prescriptions with medicines
- 🔬 **Service Requests**: Lab tests, X-rays, scans, pharmacy orders
- 🏪 **Service Units**: Pharmacy, Laboratory, Imaging departments
- 👥 **Staff & Admin Management**: Role-based access control

### Security Features
- 🔐 JWT-based authentication
- 🔑 Refresh token mechanism
- 🛡️ Role-based authorization
- 🏢 Tenant isolation
- 🔒 Secure password hashing

## 📁 Project Structure

```
HealthcareSaaS/
├── src/
│   ├── HealthcareSaaS.Domain/              # Domain entities, enums, interfaces
│   │   ├── Common/
│   │   ├── Entities/
│   │   └── Enums/
│   │
│   ├── HealthcareSaaS.Application/         # Business logic, CQRS
│   │   ├── Common/
│   │   │   ├── Interfaces/
│   │   │   └── Models/
│   │   └── Features/
│   │       ├── Appointments/
│   │       ├── Auth/
│   │       ├── Doctors/
│   │       ├── Patients/
│   │       ├── Prescriptions/
│   │       └── TimeSlots/
│   │
│   ├── HealthcareSaaS.Infrastructure/      # Data access, services
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   └── Configurations/
│   │   ├── Repositories/
│   │   └── Services/
│   │
│   └── HealthcareSaaS.API/                 # API endpoints
│       ├── Controllers/
│       ├── Middleware/
│       └── Program.cs
│
└── client/
    └── healthcare-app/                      # Angular frontend
        └── src/
            └── app/
                ├── core/                    # Services, guards, interceptors
                ├── features/                # Feature modules
                ├── shared/                  # Shared components
                └── layouts/                 # Layout components
```

## 🛠️ Setup Instructions

### Prerequisites
- .NET 9.0 SDK
- Node.js 18+ and npm
- SQL Server 2019+
- Visual Studio 2022 or VS Code
- Angular CLI 19

### Backend Setup

1. **Clone the repository**
   ```bash
   cd c:/Users/Admin/CascadeProjects/windsurf-project-2
   ```

2. **Update database connection string**
   
   Edit `src/HealthcareSaaS.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=HealthcareSaaSDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Create database migration**
   ```bash
   cd src/HealthcareSaaS.API
   dotnet ef migrations add InitialCreate --project ../HealthcareSaaS.Infrastructure
   dotnet ef database update
   ```

4. **Run the API**
   ```bash
   dotnet run
   ```
   
   API will be available at: `https://localhost:7001`
   Swagger UI: `https://localhost:7001/swagger`

### Frontend Setup

1. **Navigate to Angular project**
   ```bash
   cd client/healthcare-app
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Update API URL** (if needed)
   
   Edit `src/environments/environment.ts`:
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'https://localhost:7001/api'
   };
   ```

4. **Run the application**
   ```bash
   npm start
   ```
   
   Application will be available at: `http://localhost:4200`

## 🗄️ Database Schema

### Core Tables
- **Tenants**: Hospital/clinic information
- **Users**: All system users (base table)
- **Patients**: Patient-specific data
- **Doctors**: Doctor profiles and fees
- **Staff**: Staff members
- **Admins**: Administrative users
- **TimeSlots**: Doctor availability
- **Appointments**: Booking records
- **Prescriptions**: Medical prescriptions
- **Medicines**: Prescribed medications
- **ServiceUnits**: Pharmacy, labs, imaging centers
- **ServiceRequests**: Service orders
- **LabTests**: Laboratory test results
- **PharmacyOrders**: Medication orders
- **ImagingStudies**: X-ray, CT, MRI records

## 🔌 API Endpoints

### Authentication
```
POST   /api/auth/login           - User login
POST   /api/auth/register        - User registration
POST   /api/auth/refresh-token   - Refresh JWT token
POST   /api/auth/logout          - Logout user
```

### Appointments
```
GET    /api/appointments/{id}              - Get appointment by ID
POST   /api/appointments                   - Create appointment
PUT    /api/appointments/{id}              - Update appointment
DELETE /api/appointments/{id}              - Cancel appointment
GET    /api/appointments/patient/{id}      - Get patient appointments
GET    /api/appointments/doctor/{id}       - Get doctor appointments
```

### Doctors
```
GET    /api/doctors                        - Get all doctors
GET    /api/doctors/{id}                   - Get doctor by ID
POST   /api/doctors                        - Create doctor profile
PUT    /api/doctors/{id}                   - Update doctor profile
GET    /api/doctors/{id}/timeslots         - Get doctor time slots
POST   /api/doctors/{id}/timeslots         - Create time slots
```

### Patients
```
GET    /api/patients                       - Get all patients
GET    /api/patients/{id}                  - Get patient by ID
POST   /api/patients                       - Create patient
PUT    /api/patients/{id}                  - Update patient
GET    /api/patients/{id}/appointments     - Get patient appointments
GET    /api/patients/{id}/prescriptions    - Get patient prescriptions
```

### Prescriptions
```
GET    /api/prescriptions/{id}                      - Get prescription
POST   /api/prescriptions                           - Create prescription
PUT    /api/prescriptions/{id}                      - Update prescription
GET    /api/prescriptions/appointment/{id}          - Get by appointment
```

### Service Requests
```
GET    /api/service-requests                        - Get all requests
GET    /api/service-requests/{id}                   - Get request by ID
POST   /api/service-requests                        - Create request
PUT    /api/service-requests/{id}/status            - Update status
GET    /api/service-requests/patient/{id}           - Get patient requests
```

## 🔐 Authentication Flow

1. User logs in with email and password
2. API validates credentials and returns JWT token + refresh token
3. Frontend stores tokens in localStorage
4. All subsequent requests include JWT token in Authorization header
5. Tenant ID is extracted from JWT and used for data isolation
6. Refresh token is used to obtain new JWT when expired

## 🏢 Multi-Tenancy Implementation

- **Tenant Identification**: Via JWT claims or X-Tenant-Id header
- **Data Isolation**: Global query filters in EF Core
- **Automatic Tenant Assignment**: Middleware sets tenant context
- **Tenant-Scoped Queries**: All queries automatically filtered by tenant

## 🎨 Frontend Features

### Components
- **Login Component**: User authentication
- **Dashboard**: Overview with statistics
- **Appointment Management**: Book, view, cancel appointments
- **Doctor Directory**: Browse and search doctors
- **Patient Records**: View patient information

### Services
- **AuthService**: Authentication and token management
- **AppointmentService**: Appointment CRUD operations
- **DoctorService**: Doctor and time slot management

### Guards
- **AuthGuard**: Protects routes requiring authentication

### Interceptors
- **AuthInterceptor**: Adds JWT token to requests
- **TenantInterceptor**: Adds tenant ID to requests

## 🧪 Testing

### Backend Testing
```bash
cd src/HealthcareSaaS.API
dotnet test
```

### Frontend Testing
```bash
cd client/healthcare-app
npm test
```

## 📦 Build for Production

### Backend
```bash
cd src/HealthcareSaaS.API
dotnet publish -c Release -o ./publish
```

### Frontend
```bash
cd client/healthcare-app
npm run build
```

Build artifacts will be in `dist/healthcare-app/`

## 🔄 Future Enhancements

- [ ] Real-time notifications using SignalR
- [ ] Video consultation integration
- [ ] Payment gateway integration
- [ ] Mobile app (React Native/Flutter)
- [ ] Advanced reporting and analytics
- [ ] AI-powered diagnosis assistance
- [ ] Multi-language support
- [ ] Email/SMS notifications
- [ ] Document management system
- [ ] Insurance claim processing

## 📝 Environment Variables

### Backend (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "HealthcareSaaS",
    "Audience": "HealthcareSaaSClient"
  }
}
```

### Frontend (environment.ts)
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7001/api'
};
```

## 🤝 Contributing

This is a foundational project structure. To extend:

1. Add new entities in Domain layer
2. Create corresponding repositories in Infrastructure
3. Implement CQRS commands/queries in Application
4. Add API controllers in API layer
5. Create Angular services and components in frontend

## 📄 License

This project is created for educational and development purposes.

## 👨‍💻 Development Notes

- **Code Style**: Follow C# and TypeScript best practices
- **Naming Conventions**: PascalCase for C#, camelCase for TypeScript
- **Async/Await**: Used throughout for better performance
- **Error Handling**: Centralized error handling in both backend and frontend
- **Validation**: Data validation at both client and server side

## 🆘 Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure database exists or run migrations

### CORS Errors
- Verify CORS policy in Program.cs
- Check Angular app URL matches allowed origins

### JWT Token Issues
- Ensure JWT secret key is consistent
- Check token expiration time
- Verify token is being sent in Authorization header

## 📞 Support

For issues or questions:
1. Check the ARCHITECTURE.md file for system design details
2. Review API documentation in Swagger UI
3. Examine console logs for error messages

---

**Built with ❤️ using ASP.NET Core 9.0 and Angular 19**
