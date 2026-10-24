# Healthcare SaaS - System Architecture

## Overview
Multi-tenant healthcare appointment booking and service management system.

## Technology Stack
- **Backend**: ASP.NET Core 9.0
- **Frontend**: Angular 19/20
- **Database**: Microsoft SQL Server
- **Authentication**: JWT (JSON Web Tokens)
- **Patterns**: Clean Architecture, CQRS, Repository Pattern
- **UI**: Tailwind CSS + Bootstrap

## Core Entities

### 1. **Tenant (Hospital)**
- TenantId, Name, Address, ContactInfo, SubscriptionPlan, IsActive, CreatedDate

### 2. **User** (Base for all user types)
- UserId, TenantId, Email, PasswordHash, FirstName, LastName, Phone, UserType, IsActive, CreatedDate

### 3. **Patient**
- PatientId, UserId, DateOfBirth, Gender, BloodGroup, Address, EmergencyContact, MedicalHistory

### 4. **Doctor**
- DoctorId, UserId, Specialization, LicenseNumber, Qualification, ExperienceYears, ConsultationFee, IsAvailableForHomeVisit

### 5. **Staff**
- StaffId, UserId, Department, Role, JoinDate

### 6. **Admin**
- AdminId, UserId, AccessLevel, Permissions

### 7. **TimeSlot**
- TimeSlotId, DoctorId, TenantId, Date, StartTime, EndTime, IsBooked, IsAvailable, SlotType (InClinic/HomeVisit)

### 8. **Appointment**
- AppointmentId, PatientId, DoctorId, TimeSlotId, TenantId, AppointmentType (InClinic/HomeVisit), Status, Symptoms, Diagnosis, CreatedDate, VisitAddress (for home visits)

### 9. **Prescription**
- PrescriptionId, AppointmentId, DoctorId, PatientId, TenantId, PrescriptionDate, Notes

### 10. **Medicine**
- MedicineId, PrescriptionId, MedicineName, Dosage, Frequency, Duration, Instructions

### 11. **ServiceRequest**
- ServiceRequestId, AppointmentId, PatientId, ServiceType (Lab/XRay/Scan/Pharmacy), ServiceUnitId, Status, RequestDate, CompletionDate

### 12. **ServiceUnit** (Pharmacy, Lab, X-Ray, Scan)
- ServiceUnitId, TenantId, UnitType, Name, Location, ContactInfo, IsActive

### 13. **LabTest**
- LabTestId, ServiceRequestId, TestName, TestType, Results, PerformedBy, PerformedDate

### 14. **PharmacyOrder**
- PharmacyOrderId, ServiceRequestId, TotalAmount, Status, DispensedBy, DispensedDate

### 15. **ImagingStudy** (X-Ray/Scan)
- ImagingStudyId, ServiceRequestId, StudyType, BodyPart, ImageUrl, Report, PerformedBy, PerformedDate

## Layer Architecture

```
HealthcareSaaS/
├── src/
│   ├── HealthcareSaaS.Domain/              # Domain Layer
│   │   ├── Entities/
│   │   ├── Enums/
│   │   ├── ValueObjects/
│   │   ├── Interfaces/
│   │   └── Common/
│   │
│   ├── HealthcareSaaS.Application/         # Application Layer
│   │   ├── Common/
│   │   │   ├── Interfaces/
│   │   │   ├── Models/
│   │   │   └── Behaviors/
│   │   ├── Features/
│   │   │   ├── Appointments/
│   │   │   │   ├── Commands/
│   │   │   │   ├── Queries/
│   │   │   │   └── DTOs/
│   │   │   ├── Doctors/
│   │   │   ├── Patients/
│   │   │   ├── Prescriptions/
│   │   │   ├── ServiceRequests/
│   │   │   ├── TimeSlots/
│   │   │   └── Auth/
│   │   └── Mappings/
│   │
│   ├── HealthcareSaaS.Infrastructure/      # Infrastructure Layer
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── Configurations/
│   │   │   └── Migrations/
│   │   ├── Repositories/
│   │   ├── Services/
│   │   │   ├── AuthService.cs
│   │   │   ├── TenantService.cs
│   │   │   └── EmailService.cs
│   │   └── Identity/
│   │
│   └── HealthcareSaaS.API/                 # API Layer
│       ├── Controllers/
│       ├── Middleware/
│       ├── Filters/
│       ├── Extensions/
│       ├── Program.cs
│       └── appsettings.json
│
└── client/                                  # Angular Frontend
    └── healthcare-app/
        ├── src/
        │   ├── app/
        │   │   ├── core/
        │   │   │   ├── guards/
        │   │   │   ├── interceptors/
        │   │   │   ├── services/
        │   │   │   └── models/
        │   │   ├── features/
        │   │   │   ├── auth/
        │   │   │   ├── appointments/
        │   │   │   ├── doctors/
        │   │   │   ├── patients/
        │   │   │   ├── prescriptions/
        │   │   │   ├── services/
        │   │   │   └── admin/
        │   │   ├── shared/
        │   │   │   ├── components/
        │   │   │   ├── directives/
        │   │   │   └── pipes/
        │   │   └── layouts/
        │   ├── assets/
        │   └── styles/
        └── angular.json
```

## Key Features

### Multi-Tenancy
- Tenant isolation at data level
- Tenant identification via subdomain or header
- Tenant-specific configurations

### Authentication & Authorization
- JWT-based authentication
- Role-based access control (Patient, Doctor, Staff, Admin)
- Tenant-scoped authorization

### CQRS Pattern
- Commands for write operations
- Queries for read operations
- MediatR for request handling

### Repository Pattern
- Generic repository for common operations
- Specific repositories for complex queries
- Unit of Work pattern

## API Endpoints Structure

### Authentication
- POST /api/auth/login
- POST /api/auth/register
- POST /api/auth/refresh-token
- POST /api/auth/logout

### Appointments
- GET /api/appointments
- GET /api/appointments/{id}
- POST /api/appointments
- PUT /api/appointments/{id}
- DELETE /api/appointments/{id}
- GET /api/appointments/patient/{patientId}
- GET /api/appointments/doctor/{doctorId}

### Doctors
- GET /api/doctors
- GET /api/doctors/{id}
- POST /api/doctors
- PUT /api/doctors/{id}
- GET /api/doctors/{id}/timeslots
- POST /api/doctors/{id}/timeslots

### Patients
- GET /api/patients
- GET /api/patients/{id}
- POST /api/patients
- PUT /api/patients/{id}
- GET /api/patients/{id}/appointments
- GET /api/patients/{id}/prescriptions

### Prescriptions
- GET /api/prescriptions/{id}
- POST /api/prescriptions
- PUT /api/prescriptions/{id}
- GET /api/prescriptions/appointment/{appointmentId}

### Service Requests
- GET /api/service-requests
- GET /api/service-requests/{id}
- POST /api/service-requests
- PUT /api/service-requests/{id}/status
- GET /api/service-requests/patient/{patientId}

### Service Units
- GET /api/service-units
- GET /api/service-units/{id}
- POST /api/service-units
- PUT /api/service-units/{id}

## Database Design Principles
- Multi-tenant data isolation
- Soft deletes for audit trail
- Audit fields (CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)
- Proper indexing for performance
- Foreign key constraints for referential integrity
