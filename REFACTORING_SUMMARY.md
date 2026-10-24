# Multi-Tenant Refactoring Summary

## Overview
Successfully refactored the multi-tenant system to implement a hybrid approach where users, patients, and doctors are global entities, while their associations with hospitals are tenant-specific.

## Date
October 15, 2025

## Key Changes

### 1. Global Entities (No TenantId)
The following entities are now **global** and can be shared across multiple hospitals:

- **User**: Global user accounts with single email across the system
- **Patient**: Global patient profiles that can visit multiple hospitals
- **Doctor**: Global doctor profiles that can work at multiple hospitals

### 2. Tenant-Specific Entities
The following entities remain **tenant-specific** (inherit from `BaseTenantEntity`):

- **Staff**: Hospital-specific staff roles
- **Admin**: Hospital-specific admin roles
- **Appointment**: Appointments are tied to specific hospitals
- **TimeSlot**: Doctor availability is hospital-specific
- **Prescription**: Prescriptions are issued at specific hospitals
- **ServiceUnit**: Lab, Pharmacy, Imaging units are hospital-specific
- **ServiceRequest**: Service requests are hospital-specific
- **DoctorHospitalAssociation**: NEW - Links doctors to hospitals with hospital-specific details

### 3. New Entity: DoctorHospitalAssociation

Created to manage doctor-hospital relationships with hospital-specific data:

```csharp
public class DoctorHospitalAssociation : BaseTenantEntity
{
    public Guid DoctorId { get; set; }
    public decimal ConsultationFee { get; set; }
    public bool IsAvailableForHomeVisit { get; set; }
    public decimal? HomeVisitFee { get; set; }
    public bool IsActive { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    // Navigation Properties
    public Doctor Doctor { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
}
```

**Key Features:**
- Stores hospital-specific consultation fees
- Manages home visit availability and fees per hospital
- Tracks doctor's active status at each hospital
- Records join and end dates for employment history

### 4. Entity Modifications

#### User Entity
- **Removed**: `TenantId` property
- **Changed**: Email is now globally unique (not per tenant)
- **Updated**: Navigation properties to support multiple Staff and Admin roles
  - `ICollection<Staff> StaffRoles`
  - `ICollection<Admin> AdminRoles`

#### Doctor Entity
- **Removed**: 
  - `TenantId` property
  - `ConsultationFee` property
  - `HomeVisitFee` property
  - `IsAvailableForHomeVisit` property
- **Added**: `ICollection<DoctorHospitalAssociation> HospitalAssociations`

#### Patient Entity
- **Removed**: `TenantId` property
- **Note**: Medical records remain tenant-specific through appointments and prescriptions

#### Staff Entity
- **Changed**: Now inherits from `BaseTenantEntity`
- **Added**: `TenantId` property
- **Added**: `Tenant` navigation property

#### Admin Entity
- **Changed**: Now inherits from `BaseTenantEntity`
- **Added**: `TenantId` property
- **Added**: `Tenant` navigation property

#### Tenant Entity
- **Removed**: `ICollection<User> Users` navigation
- **Added**: 
  - `ICollection<DoctorHospitalAssociation> DoctorAssociations`
  - `ICollection<Staff> Staff`
  - `ICollection<Admin> Admins`

### 5. Authentication Changes

#### LoginDto
- **Removed**: `TenantId` property (login is now global)

#### RegisterDto
- **Removed**: `TenantId` property (registration is now global)

#### AuthResponseDto
- **Removed**: `TenantId` property
- **Added**: 
  - `FirstName` property
  - `LastName` property

#### AuthService
- **Updated**: `GenerateJwtToken()` no longer includes TenantId in JWT claims
- **Updated**: Login and registration no longer check/require TenantId
- **Updated**: Users are globally unique by email

### 6. Application Layer Updates

#### CreateAppointmentCommandHandler
- **Updated**: Now fetches `DoctorHospitalAssociation` to get fees
- **Added**: Validation to ensure doctor is associated with the hospital
- **Added**: Logic to check home visit availability per hospital
- **Improved**: Fee calculation based on appointment type and hospital association

#### IUnitOfWork & UnitOfWork
- **Added**: `IRepository<DoctorHospitalAssociation> DoctorHospitalAssociations`

### 7. Database Configuration

#### UserConfiguration
- **Changed**: Email index is now globally unique (removed TenantId from index)
- **Updated**: Relationships to support one-to-many for Staff and Admin

#### DoctorHospitalAssociationConfiguration
- **Created**: New configuration for the association entity
- **Features**:
  - Composite index on (DoctorId, TenantId)
  - Cascade delete on Doctor
  - Restrict delete on Tenant
  - Decimal precision for fees

#### TenantConfiguration
- **Removed**: User relationship configuration

### 8. Database Migration

**Migration Name**: `RefactorToGlobalUsers`

**Changes Applied**:
1. Dropped `TenantId` column from `Users` table
2. Dropped `ConsultationFee`, `HomeVisitFee`, `IsAvailableForHomeVisit` from `Doctors` table
3. Added `TenantId` column to `Staff` and `Admins` tables
4. Created `DoctorHospitalAssociations` table
5. Updated indexes:
   - Created unique index on `Users.Email` (global)
   - Created indexes on `Staff.TenantId` and `Admins.TenantId`
   - Created composite index on `DoctorHospitalAssociations (DoctorId, TenantId)`
6. Updated foreign key constraints

## Benefits of This Approach

### 1. **Global User Identity**
- Users have one account across all hospitals
- Single sign-on experience
- Simplified user management

### 2. **Doctor Flexibility**
- Doctors can work at multiple hospitals
- Different fees per hospital
- Independent availability schedules
- Employment history tracking

### 3. **Patient Continuity**
- Patients can visit any hospital
- Medical history is portable
- Single patient profile across the system

### 4. **Data Isolation**
- Medical records remain hospital-specific
- Appointments, prescriptions tied to hospitals
- Proper data privacy and compliance

### 5. **Scalability**
- Easy to add doctors to new hospitals
- Patients can seamlessly switch hospitals
- Staff and admin roles are hospital-specific

## Usage Examples

### 1. Doctor Working at Multiple Hospitals

```csharp
// Doctor has one global profile
var doctor = new Doctor 
{
    UserId = userId,
    Specialization = "Cardiology",
    LicenseNumber = "DOC12345"
};

// But different associations with hospitals
var hospital1Association = new DoctorHospitalAssociation
{
    DoctorId = doctor.Id,
    TenantId = hospital1Id,
    ConsultationFee = 500,
    IsAvailableForHomeVisit = true,
    HomeVisitFee = 1000
};

var hospital2Association = new DoctorHospitalAssociation
{
    DoctorId = doctor.Id,
    TenantId = hospital2Id,
    ConsultationFee = 600,
    IsAvailableForHomeVisit = false
};
```

### 2. Patient Visiting Multiple Hospitals

```csharp
// Patient has one global profile
var patient = new Patient 
{
    UserId = userId,
    BloodGroup = "O+",
    DateOfBirth = new DateTime(1990, 1, 1)
};

// Can book appointments at different hospitals
var appointment1 = new Appointment
{
    PatientId = patient.Id,
    DoctorId = doctorId,
    TenantId = hospital1Id
};

var appointment2 = new Appointment
{
    PatientId = patient.Id,
    DoctorId = anotherDoctorId,
    TenantId = hospital2Id
};
```

### 3. User with Multiple Roles

```csharp
// User can be a doctor globally
var user = new User { Email = "dr.smith@example.com" };
var doctor = new Doctor { UserId = user.Id };

// And also have staff roles at specific hospitals
var staffRole1 = new Staff 
{
    UserId = user.Id,
    TenantId = hospital1Id,
    Department = "Emergency",
    Role = "Department Head"
};

var staffRole2 = new Staff 
{
    UserId = user.Id,
    TenantId = hospital2Id,
    Department = "Cardiology",
    Role = "Consultant"
};
```

## Migration Path

### For Existing Data
If you have existing data in the old schema:

1. **Users**: TenantId will be removed - ensure email uniqueness across tenants
2. **Doctors**: Fees will be migrated to DoctorHospitalAssociation records
3. **Staff/Admin**: TenantId will be added - ensure proper tenant assignment

### Recommended Steps
1. Backup existing database
2. Review migration script
3. Apply migration: `dotnet ef database update`
4. Verify data integrity
5. Test authentication flow
6. Test appointment creation with new fee structure

## Testing Checklist

- [ ] User registration (global email uniqueness)
- [ ] User login (no tenant required)
- [ ] Doctor creation and hospital association
- [ ] Patient creation and multi-hospital visits
- [ ] Appointment creation with fee calculation
- [ ] Staff role assignment to specific hospitals
- [ ] Admin role assignment to specific hospitals
- [ ] Doctor working at multiple hospitals
- [ ] Patient visiting multiple hospitals
- [ ] Query filters for tenant-specific data

## Future Enhancements

1. **Patient Medical Records Portal**
   - Allow patients to view all their records across hospitals
   - Implement consent management for record sharing

2. **Doctor Availability Sync**
   - Sync time slots across hospitals
   - Manage conflicts for doctors working at multiple locations

3. **Hospital Transfer**
   - Transfer patient records between hospitals
   - Transfer staff/admin roles

4. **Analytics**
   - Cross-hospital patient analytics
   - Doctor performance across multiple hospitals

## Notes

- The `TenantService` still provides tenant context for hospital-specific operations
- Global query filters remain active for tenant-specific entities
- Authentication no longer requires tenant selection at login
- Tenant context is determined by the resource being accessed (e.g., appointment, time slot)

## Files Modified

### Domain Layer
- `User.cs`
- `Patient.cs`
- `Doctor.cs`
- `Staff.cs`
- `Admin.cs`
- `Tenant.cs`
- `DoctorHospitalAssociation.cs` (NEW)

### Application Layer
- `LoginDto.cs`
- `RegisterDto.cs`
- `AuthResponseDto.cs`
- `IAuthService.cs`
- `IUnitOfWork.cs`
- `CreateAppointmentCommandHandler.cs`

### Infrastructure Layer
- `ApplicationDbContext.cs`
- `AuthService.cs`
- `UnitOfWork.cs`
- `UserConfiguration.cs`
- `TenantConfiguration.cs`
- `DoctorHospitalAssociationConfiguration.cs` (NEW)

### Migrations
- `20251014201431_RefactorToGlobalUsers.cs` (NEW)

## Conclusion

This refactoring successfully implements a hybrid multi-tenant architecture that balances global user identity with hospital-specific data isolation. The system now supports:

- **Global**: Users, Patients, Doctors
- **Tenant-Specific**: Medical records, appointments, staff roles, admin roles, doctor-hospital associations

This provides the flexibility needed for a modern healthcare SaaS platform while maintaining proper data privacy and isolation.
