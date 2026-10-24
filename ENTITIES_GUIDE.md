# Healthcare SaaS - Entities and Relationships Guide

## Entity Relationship Overview

This document provides detailed information about all entities in the system and their relationships.

## Core Entities

### 1. Tenant (Hospital/Clinic)
**Purpose:** Represents a hospital or clinic in the multi-tenant system.

**Properties:**
- `Id` (Guid): Primary key
- `Name` (string): Hospital/clinic name
- `Address` (string): Physical address
- `ContactEmail` (string): Contact email
- `ContactPhone` (string): Contact phone
- `SubscriptionPlan` (string): Subscription tier (Basic, Premium, Enterprise)
- `IsActive` (bool): Active status
- `SubscriptionStartDate` (DateTime?): Subscription start
- `SubscriptionEndDate` (DateTime?): Subscription end

**Relationships:**
- One-to-Many with Users
- One-to-Many with ServiceUnits
- One-to-Many with TimeSlots
- One-to-Many with Appointments

---

### 2. User (Base User Entity)
**Purpose:** Base entity for all system users (patients, doctors, staff, admins).

**Properties:**
- `Id` (Guid): Primary key
- `TenantId` (Guid): Foreign key to Tenant
- `Email` (string): User email (unique per tenant)
- `PasswordHash` (string): Hashed password
- `FirstName` (string): First name
- `LastName` (string): Last name
- `Phone` (string): Phone number
- `UserType` (enum): Patient, Doctor, Staff, Admin, SuperAdmin
- `IsActive` (bool): Active status
- `LastLoginDate` (DateTime?): Last login timestamp
- `RefreshToken` (string?): JWT refresh token
- `RefreshTokenExpiryTime` (DateTime?): Refresh token expiry

**Relationships:**
- Many-to-One with Tenant
- One-to-One with Patient (optional)
- One-to-One with Doctor (optional)
- One-to-One with Staff (optional)
- One-to-One with Admin (optional)

---

### 3. Patient
**Purpose:** Patient-specific information and medical history.

**Properties:**
- `Id` (Guid): Primary key
- `UserId` (Guid): Foreign key to User
- `DateOfBirth` (DateTime): Birth date
- `Gender` (enum): Male, Female, Other
- `BloodGroup` (string?): Blood type
- `Address` (string): Home address
- `EmergencyContact` (string?): Emergency contact info
- `MedicalHistory` (string?): Medical history notes
- `Allergies` (string?): Known allergies

**Relationships:**
- One-to-One with User
- One-to-Many with Appointments
- One-to-Many with Prescriptions
- One-to-Many with ServiceRequests

**Use Cases:**
- Patient registration
- Medical record management
- Appointment booking
- Prescription tracking

---

### 4. Doctor
**Purpose:** Doctor profile, specialization, and fee information.

**Properties:**
- `Id` (Guid): Primary key
- `UserId` (Guid): Foreign key to User
- `Specialization` (string): Medical specialization
- `LicenseNumber` (string): Medical license number
- `Qualification` (string): Educational qualifications
- `ExperienceYears` (int): Years of experience
- `ConsultationFee` (decimal): In-clinic consultation fee
- `IsAvailableForHomeVisit` (bool): Home visit availability
- `HomeVisitFee` (decimal?): Home visit fee
- `Bio` (string?): Doctor biography

**Relationships:**
- One-to-One with User
- One-to-Many with TimeSlots
- One-to-Many with Appointments
- One-to-Many with Prescriptions

**Use Cases:**
- Doctor profile management
- Availability scheduling
- Fee management
- Appointment handling

---

### 5. Staff
**Purpose:** Hospital staff members (nurses, technicians, etc.).

**Properties:**
- `Id` (Guid): Primary key
- `UserId` (Guid): Foreign key to User
- `Department` (string): Department name
- `Role` (string): Staff role/position
- `JoinDate` (DateTime): Employment start date
- `ServiceUnitId` (Guid?): Assigned service unit

**Relationships:**
- One-to-One with User
- Many-to-One with ServiceUnit (optional)

**Use Cases:**
- Staff management
- Department assignment
- Service unit operations

---

### 6. Admin
**Purpose:** Administrative users with elevated permissions.

**Properties:**
- `Id` (Guid): Primary key
- `UserId` (Guid): Foreign key to User
- `AccessLevel` (string): Standard, Manager, SuperAdmin
- `Permissions` (string): JSON string of permissions

**Relationships:**
- One-to-One with User

**Use Cases:**
- System administration
- User management
- Configuration management

---

### 7. TimeSlot
**Purpose:** Doctor availability slots for appointments.

**Properties:**
- `Id` (Guid): Primary key
- `TenantId` (Guid): Foreign key to Tenant
- `DoctorId` (Guid): Foreign key to Doctor
- `Date` (DateTime): Slot date
- `StartTime` (TimeSpan): Start time
- `EndTime` (TimeSpan): End time
- `IsBooked` (bool): Booking status
- `IsAvailable` (bool): Availability status
- `SlotType` (enum): InClinic, HomeVisit, Telemedicine

**Relationships:**
- Many-to-One with Doctor
- Many-to-One with Tenant
- One-to-One with Appointment (when booked)

**Use Cases:**
- Doctor schedule management
- Appointment slot availability
- Calendar management

---

### 8. Appointment
**Purpose:** Patient-doctor appointment records.

**Properties:**
- `Id` (Guid): Primary key
- `TenantId` (Guid): Foreign key to Tenant
- `PatientId` (Guid): Foreign key to Patient
- `DoctorId` (Guid): Foreign key to Doctor
- `TimeSlotId` (Guid): Foreign key to TimeSlot
- `AppointmentType` (enum): InClinic, HomeVisit, Telemedicine
- `Status` (enum): Scheduled, Confirmed, InProgress, Completed, Cancelled, NoShow
- `Symptoms` (string?): Patient symptoms
- `Diagnosis` (string?): Doctor's diagnosis
- `VisitAddress` (string?): Address for home visits
- `AppointmentDate` (DateTime): Appointment date
- `Notes` (string?): Additional notes
- `TotalFee` (decimal?): Total consultation fee

**Relationships:**
- Many-to-One with Patient
- Many-to-One with Doctor
- One-to-One with TimeSlot
- Many-to-One with Tenant
- One-to-One with Prescription (optional)
- One-to-Many with ServiceRequests

**Use Cases:**
- Appointment booking
- Consultation management
- Medical record keeping
- Billing

---

### 9. Prescription
**Purpose:** Medical prescriptions issued by doctors.

**Properties:**
- `Id` (Guid): Primary key
- `TenantId` (Guid): Foreign key to Tenant
- `AppointmentId` (Guid): Foreign key to Appointment
- `DoctorId` (Guid): Foreign key to Doctor
- `PatientId` (Guid): Foreign key to Patient
- `PrescriptionDate` (DateTime): Issue date
- `Notes` (string?): General notes
- `DietaryInstructions` (string?): Diet recommendations
- `FollowUpInstructions` (string?): Follow-up care
- `NextVisitDate` (DateTime?): Next appointment date

**Relationships:**
- One-to-One with Appointment
- Many-to-One with Doctor
- Many-to-One with Patient
- Many-to-One with Tenant
- One-to-Many with Medicines

**Use Cases:**
- Digital prescription management
- Medicine tracking
- Follow-up scheduling

---

### 10. Medicine
**Purpose:** Individual medicines in a prescription.

**Properties:**
- `Id` (Guid): Primary key
- `PrescriptionId` (Guid): Foreign key to Prescription
- `MedicineName` (string): Medicine name
- `Dosage` (string): Dosage amount
- `Frequency` (string): How often to take
- `DurationDays` (int): Treatment duration
- `Instructions` (string?): Special instructions
- `Quantity` (int): Total quantity

**Relationships:**
- Many-to-One with Prescription

**Use Cases:**
- Prescription details
- Pharmacy order fulfillment
- Medicine tracking

---

### 11. ServiceUnit
**Purpose:** Hospital service departments (Pharmacy, Lab, X-Ray, etc.).

**Properties:**
- `Id` (Guid): Primary key
- `TenantId` (Guid): Foreign key to Tenant
- `UnitType` (enum): Pharmacy, Laboratory, XRay, Scan, Ultrasound
- `Name` (string): Unit name
- `Location` (string): Physical location
- `ContactEmail` (string): Contact email
- `ContactPhone` (string): Contact phone
- `IsActive` (bool): Active status
- `OperatingHours` (string?): Operating hours

**Relationships:**
- Many-to-One with Tenant
- One-to-Many with ServiceRequests
- One-to-Many with Staff

**Use Cases:**
- Service department management
- Request routing
- Staff assignment

---

### 12. ServiceRequest
**Purpose:** Requests for lab tests, imaging, pharmacy orders, etc.

**Properties:**
- `Id` (Guid): Primary key
- `TenantId` (Guid): Foreign key to Tenant
- `AppointmentId` (Guid): Foreign key to Appointment
- `PatientId` (Guid): Foreign key to Patient
- `ServiceType` (enum): Pharmacy, Laboratory, XRay, Scan, Ultrasound
- `ServiceUnitId` (Guid): Foreign key to ServiceUnit
- `Status` (enum): Pending, Assigned, InProgress, Completed, Cancelled, Rejected
- `RequestDate` (DateTime): Request timestamp
- `CompletionDate` (DateTime?): Completion timestamp
- `Instructions` (string?): Special instructions
- `Notes` (string?): Additional notes
- `TotalAmount` (decimal?): Service cost

**Relationships:**
- Many-to-One with Appointment
- Many-to-One with Patient
- Many-to-One with ServiceUnit
- Many-to-One with Tenant
- One-to-One with LabTest (optional)
- One-to-One with PharmacyOrder (optional)
- One-to-One with ImagingStudy (optional)

**Use Cases:**
- Service order management
- Workflow tracking
- Inter-department communication

---

### 13. LabTest
**Purpose:** Laboratory test details and results.

**Properties:**
- `Id` (Guid): Primary key
- `ServiceRequestId` (Guid): Foreign key to ServiceRequest
- `TestName` (string): Test name
- `TestType` (string): Test category
- `Results` (string?): Test results
- `PerformedBy` (string?): Technician name
- `PerformedDate` (DateTime?): Test date
- `ReportUrl` (string?): Report file URL
- `ReferenceRange` (string?): Normal range values

**Relationships:**
- One-to-One with ServiceRequest

**Use Cases:**
- Lab test management
- Result reporting
- Medical records

---

### 14. PharmacyOrder
**Purpose:** Pharmacy medication orders.

**Properties:**
- `Id` (Guid): Primary key
- `ServiceRequestId` (Guid): Foreign key to ServiceRequest
- `TotalAmount` (decimal): Order total
- `Status` (string): Order status
- `DispensedBy` (string?): Pharmacist name
- `DispensedDate` (DateTime?): Dispensing date
- `PaymentMethod` (string?): Payment type
- `IsPaid` (bool): Payment status

**Relationships:**
- One-to-One with ServiceRequest
- One-to-Many with PharmacyOrderItems

**Use Cases:**
- Medication dispensing
- Inventory management
- Billing

---

### 15. PharmacyOrderItem
**Purpose:** Individual items in a pharmacy order.

**Properties:**
- `Id` (Guid): Primary key
- `PharmacyOrderId` (Guid): Foreign key to PharmacyOrder
- `MedicineName` (string): Medicine name
- `Quantity` (int): Quantity ordered
- `UnitPrice` (decimal): Price per unit
- `TotalPrice` (decimal): Line total
- `BatchNumber` (string?): Medicine batch
- `ExpiryDate` (DateTime?): Expiry date

**Relationships:**
- Many-to-One with PharmacyOrder

---

### 16. ImagingStudy
**Purpose:** X-Ray, CT, MRI, and other imaging records.

**Properties:**
- `Id` (Guid): Primary key
- `ServiceRequestId` (Guid): Foreign key to ServiceRequest
- `StudyType` (string): X-Ray, CT, MRI, Ultrasound
- `BodyPart` (string): Area scanned
- `ImageUrl` (string?): Image file URL
- `Report` (string?): Radiologist report
- `PerformedBy` (string?): Technician name
- `PerformedDate` (DateTime?): Study date
- `Findings` (string?): Key findings
- `Recommendations` (string?): Recommendations

**Relationships:**
- One-to-One with ServiceRequest

**Use Cases:**
- Imaging management
- Radiology reporting
- Medical records

---

## Entity Relationship Diagram (Simplified)

```
Tenant
  ├── Users (1:N)
  │     ├── Patient (1:1)
  │     ├── Doctor (1:1)
  │     ├── Staff (1:1)
  │     └── Admin (1:1)
  ├── ServiceUnits (1:N)
  ├── TimeSlots (1:N)
  └── Appointments (1:N)

Appointment
  ├── Patient (N:1)
  ├── Doctor (N:1)
  ├── TimeSlot (1:1)
  ├── Prescription (1:1)
  └── ServiceRequests (1:N)

Prescription
  ├── Medicines (1:N)
  └── Appointment (1:1)

ServiceRequest
  ├── LabTest (1:1)
  ├── PharmacyOrder (1:1)
  └── ImagingStudy (1:1)

PharmacyOrder
  └── PharmacyOrderItems (1:N)
```

---

## Common Query Patterns

### Get Patient's Appointments
```csharp
var appointments = await _unitOfWork.Appointments
    .FindAsync(a => a.PatientId == patientId);
```

### Get Doctor's Schedule
```csharp
var timeSlots = await _unitOfWork.TimeSlots
    .FindAsync(t => t.DoctorId == doctorId && t.Date >= DateTime.Today);
```

### Get Prescription with Medicines
```csharp
var prescription = await _context.Prescriptions
    .Include(p => p.Medicines)
    .FirstOrDefaultAsync(p => p.Id == prescriptionId);
```

### Get Service Request with Details
```csharp
var request = await _context.ServiceRequests
    .Include(sr => sr.LabTest)
    .Include(sr => sr.PharmacyOrder)
    .Include(sr => sr.ImagingStudy)
    .FirstOrDefaultAsync(sr => sr.Id == requestId);
```

---

This guide provides a foundation for understanding the data model. Extend entities as needed for your specific requirements.
