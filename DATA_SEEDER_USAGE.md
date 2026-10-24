# Data Seeder Usage Guide

## Overview

The Healthcare SaaS application now includes an automatic data seeder that populates the database with sample data for all user types and tenants.

## How It Works

### Automatic Seeding

The data seeder runs automatically when the application starts:

1. **On Application Startup:** The seeder is called in `Program.cs` after the app is built
2. **Database Check:** It checks if the database already contains data
3. **Conditional Seeding:** Only seeds if the database is empty (no existing users)
4. **Migration:** Automatically applies any pending migrations before seeding

### What Gets Seeded

The seeder creates:

✅ **4 Tenants** (Healthcare organizations with different subscription plans)
✅ **1 Super Admin** (Platform-level administrator)
✅ **3 Admins** (One per tenant with different access levels)
✅ **6 Doctors** (Various specializations across different tenants)
✅ **5 Staff Members** (Nurses, receptionists, lab techs, pharmacists)
✅ **8 Patients** (With complete medical profiles)

## Quick Start

### 1. First Time Setup

```bash
# Navigate to the project directory
cd c:\Users\Admin\CascadeProjects\windsurf-project-2

# Run the application
dotnet run --project src/HealthcareSaaS.API
```

The seeder will automatically:
- Create the database if it doesn't exist
- Apply all migrations
- Seed the data

### 2. Test Login Credentials

After seeding, you can log in with any of these accounts:

**Super Admin:**
```
Email: superadmin@healthcaresaas.com
Password: SuperAdmin@123
```

**Admin (City General Hospital):**
```
Email: admin1@citygeneralhospital.com
Password: Admin@123
```

**Doctor (Cardiologist):**
```
Email: dr.smith@citygeneralhospital.com
Password: Doctor@123
```

**Staff (Nurse):**
```
Email: nurse.thompson@citygeneralhospital.com
Password: Staff@123
```

**Patient:**
```
Email: patient1@example.com
Password: Patient@123
```

## Advanced Usage

### Resetting the Database

If you want to reset and re-seed the database:

#### Option 1: Delete Database via SQL Server Management Studio
1. Connect to `(localdb)\MSSQLLocalDB`
2. Delete the `HealthcareSaaSDb` database
3. Run the application - it will recreate and seed automatically

#### Option 2: Using EF Core CLI
```bash
# Drop the database
dotnet ef database drop --project src/HealthcareSaaS.Infrastructure --startup-project src/HealthcareSaaS.API --force

# Run the application (it will recreate and seed)
dotnet run --project src/HealthcareSaaS.API
```

### Disabling Auto-Seeding

If you want to disable automatic seeding, comment out this line in `Program.cs`:

```csharp
// await DataSeeder.SeedAsync(app.Services);
```

### Manual Seeding

To manually trigger seeding from code:

```csharp
using HealthcareSaaS.Infrastructure.Data;

// In your service or controller
await DataSeeder.SeedAsync(serviceProvider);
```

## Seeder Implementation Details

### Location
- **File:** `src/HealthcareSaaS.Infrastructure/Data/DataSeeder.cs`
- **Namespace:** `HealthcareSaaS.Infrastructure.Data`

### Key Features

1. **Idempotent:** Safe to run multiple times - checks if data exists before seeding
2. **Transactional:** All seeding happens within database transactions
3. **Logging:** Provides detailed logs about the seeding process
4. **Error Handling:** Catches and logs any errors during seeding
5. **Realistic Data:** Uses realistic names, addresses, and medical information

### Seeding Order

The seeder follows this order to maintain referential integrity:

1. **Tenants** (Organizations)
2. **Super Admin** (Platform user)
3. **Admin Users & Roles** (Tenant-specific admins)
4. **Doctor Users & Profiles** (With specializations)
5. **Doctor-Hospital Associations** (Links doctors to tenants)
6. **Staff Users & Profiles** (Various roles)
7. **Patient Users & Profiles** (With medical histories)

## Testing Scenarios

### Multi-Tenancy Testing

Test tenant isolation:
```
1. Login as admin1@citygeneralhospital.com
2. Verify you only see City General Hospital data
3. Login as admin2@sunrisemedical.com
4. Verify you only see Sunrise Medical Center data
```

### Role-Based Access Testing

Test different user roles:
```
1. Login as SuperAdmin - should access all tenants
2. Login as Admin - should manage tenant users
3. Login as Doctor - should manage appointments
4. Login as Staff - should assist with operations
5. Login as Patient - should book appointments
```

### Doctor-Hospital Associations

Test multi-hospital doctors:
```
1. Doctors can be associated with multiple hospitals
2. Each association has different consultation fees
3. Some doctors offer home visits, others don't
```

## Troubleshooting

### Issue: "Database already seeded"

**Solution:** This is normal. The seeder detected existing data and skipped seeding.

### Issue: Migration errors

**Solution:** 
```bash
# Check migration status
dotnet ef migrations list --project src/HealthcareSaaS.Infrastructure --startup-project src/HealthcareSaaS.API

# Apply migrations manually
dotnet ef database update --project src/HealthcareSaaS.Infrastructure --startup-project src/HealthcareSaaS.API
```

### Issue: Foreign key constraint errors

**Solution:** The seeder maintains proper order. If you see FK errors, ensure:
1. Database is completely empty before seeding
2. All migrations are applied
3. No partial data exists

### Issue: Connection string errors

**Solution:** Check `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=HealthcareSaaSDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

## Data Reference

For a complete list of all seeded users and their credentials, see:
- **[SEEDED_DATA.md](./SEEDED_DATA.md)** - Comprehensive reference of all seeded data

## Security Notes

⚠️ **Important Security Considerations:**

1. **Development Only:** This seeder is for development/testing only
2. **Default Passwords:** Never use these default passwords in production
3. **Password Hashing:** Currently uses SHA256 (consider BCrypt for production)
4. **Sensitive Data:** Remove or secure seeded data before deploying to production
5. **Environment Check:** Consider adding environment checks to prevent seeding in production

### Production Recommendations

For production environments:
1. Disable automatic seeding
2. Use secure, randomly generated passwords
3. Implement proper user onboarding flows
4. Use environment variables for sensitive configuration
5. Implement proper password policies and MFA

## Customization

### Adding More Seed Data

To add more seed data, edit `DataSeeder.cs`:

```csharp
// Add more tenants
var newTenant = new Tenant
{
    Id = Guid.NewGuid(),
    Name = "Your Hospital Name",
    // ... other properties
};

// Add more users
var newUser = new User
{
    Id = Guid.NewGuid(),
    Email = "user@example.com",
    PasswordHash = HashPassword("YourPassword"),
    // ... other properties
};
```

### Modifying Existing Data

Edit the respective seed methods:
- `SeedTenants()` - Modify tenant data
- `SeedSuperAdmin()` - Modify super admin
- `SeedAdmins()` - Modify admin users
- `SeedDoctors()` - Modify doctor data
- `SeedStaff()` - Modify staff data
- `SeedPatients()` - Modify patient data

## Support

For issues or questions:
1. Check the logs in the console output
2. Review the `SEEDED_DATA.md` for credential reference
3. Verify database connection in `appsettings.json`
4. Ensure all migrations are applied
