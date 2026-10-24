# Seeded Data Reference

This document provides details about the seeded data in the Healthcare SaaS application.

## Overview

The data seeder automatically populates the database with sample data for testing and development purposes. The seeder runs automatically on application startup and will only seed data if the database is empty.

## Seeded Data Summary

### Tenants (4 Organizations)

| Name | Subscription Plan | Contact Email | Contact Phone |
|------|------------------|---------------|---------------|
| City General Hospital | Enterprise | contact@citygeneralhospital.com | +1-555-0101 |
| Sunrise Medical Center | Professional | info@sunrisemedical.com | +1-555-0102 |
| Green Valley Clinic | Basic | hello@greenvalleyclinic.com | +1-555-0103 |
| Metro Health Services | Enterprise | support@metrohealth.com | +1-555-0104 |

---

## Users by Type

### 1. Super Admin (1 user)

**Super Admin Account:**
- **Email:** superadmin@healthcaresaas.com
- **Password:** SuperAdmin@123
- **Name:** Super Admin
- **Phone:** +1-555-9999
- **User Type:** SuperAdmin
- **Description:** Platform-level administrator with access to all tenants

---

### 2. Admins (3 users)

| Name | Email | Password | Tenant | Access Level |
|------|-------|----------|--------|--------------|
| John Anderson | admin1@citygeneralhospital.com | Admin@123 | City General Hospital | Full |
| Sarah Williams | admin2@sunrisemedical.com | Admin@123 | Sunrise Medical Center | Full |
| Michael Brown | admin3@greenvalleyclinic.com | Admin@123 | Green Valley Clinic | Standard |

**Permissions:**
- **Full Access:** Can manage users, settings, and view reports
- **Standard Access:** Can manage users and view reports (no settings access)

---

### 3. Doctors (6 users)

| Name | Email | Password | Specialization | Tenant | Consultation Fee | Home Visit |
|------|-------|----------|----------------|--------|------------------|------------|
| Dr. Emily Smith | dr.smith@citygeneralhospital.com | Doctor@123 | Cardiology | City General Hospital | $150 | Yes ($250) |
| Dr. David Johnson | dr.johnson@citygeneralhospital.com | Doctor@123 | Pediatrics | City General Hospital | $120 | Yes ($200) |
| Dr. Jennifer Davis | dr.davis@sunrisemedical.com | Doctor@123 | Orthopedics | Sunrise Medical Center | $180 | No |
| Dr. Robert Wilson | dr.wilson@sunrisemedical.com | Doctor@123 | Neurology | Sunrise Medical Center | $200 | Yes ($300) |
| Dr. Maria Martinez | dr.martinez@greenvalleyclinic.com | Doctor@123 | General Practice | Green Valley Clinic | $100 | Yes ($150) |
| Dr. James Taylor | dr.taylor@metrohealth.com | Doctor@123 | Dermatology | Metro Health Services | $140 | No |

**Additional Doctor Details:**
- All doctors have license numbers (MD-12345 through MD-12350)
- Experience ranges from 6 to 15 years
- Each has appropriate qualifications and bio

---

### 4. Staff (5 users)

| Name | Email | Password | Role | Department | Tenant |
|------|-------|----------|------|------------|--------|
| Linda Thompson | nurse.thompson@citygeneralhospital.com | Staff@123 | Senior Nurse | Nursing | City General Hospital |
| Carlos Garcia | receptionist.garcia@citygeneralhospital.com | Staff@123 | Front Desk Receptionist | Administration | City General Hospital |
| Amy Lee | nurse.lee@sunrisemedical.com | Staff@123 | Registered Nurse | Nursing | Sunrise Medical Center |
| Kevin White | lab.tech@sunrisemedical.com | Staff@123 | Lab Technician | Laboratory | Sunrise Medical Center |
| Patricia Moore | pharmacist@greenvalleyclinic.com | Staff@123 | Pharmacist | Pharmacy | Green Valley Clinic |

---

### 5. Patients (8 users)

| Name | Email | Password | Gender | Blood Group | Medical History |
|------|-------|----------|--------|-------------|-----------------|
| Alice Cooper | patient1@example.com | Patient@123 | Female | A+ | Hypertension |
| Bob Miller | patient2@example.com | Patient@123 | Male | O+ | Diabetes Type 2 |
| Catherine Harris | patient3@example.com | Patient@123 | Female | B+ | Asthma |
| Daniel Clark | patient4@example.com | Patient@123 | Male | AB+ | Heart Disease |
| Emma Lewis | patient5@example.com | Patient@123 | Female | O- | None |
| Frank Walker | patient6@example.com | Patient@123 | Male | A- | Migraine |
| Grace Hall | patient7@example.com | Patient@123 | Female | B- | Thyroid disorder |
| Henry Young | patient8@example.com | Patient@123 | Male | AB- | Arthritis |

**Patient Details:**
- All patients have complete profiles with date of birth, address, and emergency contacts
- Various medical histories and allergies for realistic testing scenarios
- Age range from early 30s to late 50s

---

## Testing Scenarios

### Login Testing

You can test the application with any of the following credentials:

1. **Super Admin Access:**
   ```
   Email: superadmin@healthcaresaas.com
   Password: SuperAdmin@123
   ```

2. **Hospital Admin:**
   ```
   Email: admin1@citygeneralhospital.com
   Password: Admin@123
   ```

3. **Doctor:**
   ```
   Email: dr.smith@citygeneralhospital.com
   Password: Doctor@123
   ```

4. **Staff:**
   ```
   Email: nurse.thompson@citygeneralhospital.com
   Password: Staff@123
   ```

5. **Patient:**
   ```
   Email: patient1@example.com
   Password: Patient@123
   ```

### Multi-Tenancy Testing

- Test tenant isolation by logging in as users from different organizations
- Verify that admins can only see data from their own tenant
- Check that doctors can work with multiple hospitals through associations

### Role-Based Access Testing

- Test different permission levels between Full and Standard admin access
- Verify doctor-specific features and patient management
- Test staff workflows in different departments

---

## Database Reset

To reset the database and re-seed:

1. Delete the existing database
2. Run the application - it will automatically:
   - Create the database
   - Apply migrations
   - Seed the data

Alternatively, you can manually trigger migrations:
```bash
dotnet ef database drop --project src/HealthcareSaaS.Infrastructure --startup-project src/HealthcareSaaS.API
dotnet ef database update --project src/HealthcareSaaS.Infrastructure --startup-project src/HealthcareSaaS.API
```

---

## Notes

- **Password Hashing:** All passwords are hashed using SHA256 (same as AuthService)
- **Default Password Pattern:** Each user type has a consistent password pattern for easy testing
- **Data Relationships:** All foreign key relationships are properly established
- **Tenant Associations:** Doctors, Staff, and Admins are properly associated with their respective tenants
- **Realistic Data:** Names, addresses, and medical information are varied for realistic testing

---

## Security Warning

⚠️ **Important:** This seeded data is for **development and testing purposes only**. 

- Never use these credentials in production
- Always change default passwords in production environments
- Consider using a more secure password hashing algorithm (like BCrypt) for production
