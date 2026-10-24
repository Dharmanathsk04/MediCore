using System.Security.Cryptography;
using System.Text;
using HealthcareSaaS.Domain.Entities;
using HealthcareSaaS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HealthcareSaaS.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            // Ensure database is created
            await context.Database.MigrateAsync();

            logger.LogInformation("Checking if database needs seeding...");

            // Check if data already exists
            var userCount = await context.Users.CountAsync();
            if (userCount > 0)
            {
                logger.LogInformation($"Database already contains {userCount} users. Skipping seed operation.");
                return;
            }

            logger.LogInformation("Starting database seeding...");

            // Seed Tenants
            var tenants = await SeedTenants(context, logger);
            
            // Seed Users and their related entities
            await SeedSuperAdmin(context, logger);
            await SeedAdmins(context, tenants, logger);
            await SeedDoctors(context, tenants, logger);
            await SeedStaff(context, tenants, logger);
            await SeedPatients(context, logger);

            logger.LogInformation("Database seeding completed successfully.");
            logger.LogInformation($"Seeded: {tenants.Count} tenants, 1 super admin, 3 admins, 6 doctors, 5 staff, 8 patients");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private static async Task<List<Tenant>> SeedTenants(ApplicationDbContext context, ILogger logger)
    {
        logger.LogInformation("Seeding tenants...");
        var tenants = new List<Tenant>
        {
            new Tenant
            {
                Id = Guid.NewGuid(),
                Name = "City General Hospital",
                Address = "123 Main Street, New York, NY 10001",
                ContactEmail = "contact@citygeneralhospital.com",
                ContactPhone = "+1-555-0101",
                SubscriptionPlan = "Enterprise",
                IsActive = true,
                SubscriptionStartDate = DateTime.UtcNow.AddMonths(-6),
                SubscriptionEndDate = DateTime.UtcNow.AddMonths(6),
                CreatedDate = DateTime.UtcNow
            },
            new Tenant
            {
                Id = Guid.NewGuid(),
                Name = "Sunrise Medical Center",
                Address = "456 Oak Avenue, Los Angeles, CA 90001",
                ContactEmail = "info@sunrisemedical.com",
                ContactPhone = "+1-555-0102",
                SubscriptionPlan = "Professional",
                IsActive = true,
                SubscriptionStartDate = DateTime.UtcNow.AddMonths(-3),
                SubscriptionEndDate = DateTime.UtcNow.AddMonths(9),
                CreatedDate = DateTime.UtcNow
            },
            new Tenant
            {
                Id = Guid.NewGuid(),
                Name = "Green Valley Clinic",
                Address = "789 Pine Road, Chicago, IL 60601",
                ContactEmail = "hello@greenvalleyclinic.com",
                ContactPhone = "+1-555-0103",
                SubscriptionPlan = "Basic",
                IsActive = true,
                SubscriptionStartDate = DateTime.UtcNow.AddMonths(-1),
                SubscriptionEndDate = DateTime.UtcNow.AddMonths(11),
                CreatedDate = DateTime.UtcNow
            },
            new Tenant
            {
                Id = Guid.NewGuid(),
                Name = "Metro Health Services",
                Address = "321 Elm Street, Houston, TX 77001",
                ContactEmail = "support@metrohealth.com",
                ContactPhone = "+1-555-0104",
                SubscriptionPlan = "Enterprise",
                IsActive = true,
                SubscriptionStartDate = DateTime.UtcNow.AddMonths(-12),
                SubscriptionEndDate = DateTime.UtcNow.AddMonths(12),
                CreatedDate = DateTime.UtcNow
            }
        };

        await context.Tenants.AddRangeAsync(tenants);
        await context.SaveChangesAsync();
        logger.LogInformation($"Seeded {tenants.Count} tenants");
        return tenants;
    }

    private static async Task SeedSuperAdmin(ApplicationDbContext context, ILogger logger)
    {
        logger.LogInformation("Seeding super admin...");
        var superAdminUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "superadmin@healthcaresaas.com",
            PasswordHash = HashPassword("SuperAdmin@123"),
            FirstName = "Super",
            LastName = "Admin",
            Phone = "+1-555-9999",
            UserType = UserType.SuperAdmin,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        await context.Users.AddAsync(superAdminUser);
        await context.SaveChangesAsync();
        logger.LogInformation("Seeded super admin");
    }

    private static async Task SeedAdmins(ApplicationDbContext context, List<Tenant> tenants, ILogger logger)
    {
        logger.LogInformation("Seeding admins...");
        var adminUsers = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "admin1@citygeneralhospital.com",
                PasswordHash = HashPassword("Admin@123"),
                FirstName = "John",
                LastName = "Anderson",
                Phone = "+1-555-1001",
                UserType = UserType.Admin,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "admin2@sunrisemedical.com",
                PasswordHash = HashPassword("Admin@123"),
                FirstName = "Sarah",
                LastName = "Williams",
                Phone = "+1-555-1002",
                UserType = UserType.Admin,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "admin3@greenvalleyclinic.com",
                PasswordHash = HashPassword("Admin@123"),
                FirstName = "Michael",
                LastName = "Brown",
                Phone = "+1-555-1003",
                UserType = UserType.Admin,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        };

        await context.Users.AddRangeAsync(adminUsers);
        await context.SaveChangesAsync();

        // Create Admin roles for each tenant
        var adminRoles = new List<Admin>
        {
            new Admin
            {
                Id = Guid.NewGuid(),
                UserId = adminUsers[0].Id,
                TenantId = tenants[0].Id,
                AccessLevel = "Full",
                Permissions = "{\"canManageUsers\":true,\"canManageSettings\":true,\"canViewReports\":true}",
                CreatedDate = DateTime.UtcNow
            },
            new Admin
            {
                Id = Guid.NewGuid(),
                UserId = adminUsers[1].Id,
                TenantId = tenants[1].Id,
                AccessLevel = "Full",
                Permissions = "{\"canManageUsers\":true,\"canManageSettings\":true,\"canViewReports\":true}",
                CreatedDate = DateTime.UtcNow
            },
            new Admin
            {
                Id = Guid.NewGuid(),
                UserId = adminUsers[2].Id,
                TenantId = tenants[2].Id,
                AccessLevel = "Standard",
                Permissions = "{\"canManageUsers\":true,\"canManageSettings\":false,\"canViewReports\":true}",
                CreatedDate = DateTime.UtcNow
            }
        };

        await context.Admins.AddRangeAsync(adminRoles);
        await context.SaveChangesAsync();
        logger.LogInformation($"Seeded {adminUsers.Count} admins");
    }

    private static async Task SeedDoctors(ApplicationDbContext context, List<Tenant> tenants, ILogger logger)
    {
        logger.LogInformation("Seeding doctors...");
        var doctorUsers = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "dr.smith@citygeneralhospital.com",
                PasswordHash = HashPassword("Doctor@123"),
                FirstName = "Emily",
                LastName = "Smith",
                Phone = "+1-555-2001",
                UserType = UserType.Doctor,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "dr.johnson@citygeneralhospital.com",
                PasswordHash = HashPassword("Doctor@123"),
                FirstName = "David",
                LastName = "Johnson",
                Phone = "+1-555-2002",
                UserType = UserType.Doctor,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "dr.davis@sunrisemedical.com",
                PasswordHash = HashPassword("Doctor@123"),
                FirstName = "Jennifer",
                LastName = "Davis",
                Phone = "+1-555-2003",
                UserType = UserType.Doctor,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "dr.wilson@sunrisemedical.com",
                PasswordHash = HashPassword("Doctor@123"),
                FirstName = "Robert",
                LastName = "Wilson",
                Phone = "+1-555-2004",
                UserType = UserType.Doctor,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "dr.martinez@greenvalleyclinic.com",
                PasswordHash = HashPassword("Doctor@123"),
                FirstName = "Maria",
                LastName = "Martinez",
                Phone = "+1-555-2005",
                UserType = UserType.Doctor,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "dr.taylor@metrohealth.com",
                PasswordHash = HashPassword("Doctor@123"),
                FirstName = "James",
                LastName = "Taylor",
                Phone = "+1-555-2006",
                UserType = UserType.Doctor,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        };

        await context.Users.AddRangeAsync(doctorUsers);
        await context.SaveChangesAsync();

        // Create Doctor profiles
        var doctors = new List<Doctor>
        {
            new Doctor
            {
                Id = Guid.NewGuid(),
                UserId = doctorUsers[0].Id,
                Specialization = "Cardiology",
                LicenseNumber = "MD-12345",
                Qualification = "MD, FACC",
                ExperienceYears = 15,
                Bio = "Experienced cardiologist specializing in interventional cardiology and heart disease prevention.",
                CreatedDate = DateTime.UtcNow
            },
            new Doctor
            {
                Id = Guid.NewGuid(),
                UserId = doctorUsers[1].Id,
                Specialization = "Pediatrics",
                LicenseNumber = "MD-12346",
                Qualification = "MD, FAAP",
                ExperienceYears = 10,
                Bio = "Dedicated pediatrician with expertise in child development and preventive care.",
                CreatedDate = DateTime.UtcNow
            },
            new Doctor
            {
                Id = Guid.NewGuid(),
                UserId = doctorUsers[2].Id,
                Specialization = "Orthopedics",
                LicenseNumber = "MD-12347",
                Qualification = "MD, MS (Ortho)",
                ExperienceYears = 12,
                Bio = "Orthopedic surgeon specializing in joint replacement and sports medicine.",
                CreatedDate = DateTime.UtcNow
            },
            new Doctor
            {
                Id = Guid.NewGuid(),
                UserId = doctorUsers[3].Id,
                Specialization = "Neurology",
                LicenseNumber = "MD-12348",
                Qualification = "MD, DM (Neurology)",
                ExperienceYears = 8,
                Bio = "Neurologist with focus on stroke management and neurodegenerative diseases.",
                CreatedDate = DateTime.UtcNow
            },
            new Doctor
            {
                Id = Guid.NewGuid(),
                UserId = doctorUsers[4].Id,
                Specialization = "General Practice",
                LicenseNumber = "MD-12349",
                Qualification = "MD",
                ExperienceYears = 6,
                Bio = "General practitioner providing comprehensive primary care services.",
                CreatedDate = DateTime.UtcNow
            },
            new Doctor
            {
                Id = Guid.NewGuid(),
                UserId = doctorUsers[5].Id,
                Specialization = "Dermatology",
                LicenseNumber = "MD-12350",
                Qualification = "MD, DVD",
                ExperienceYears = 9,
                Bio = "Dermatologist specializing in medical and cosmetic dermatology.",
                CreatedDate = DateTime.UtcNow
            }
        };

        await context.Doctors.AddRangeAsync(doctors);
        await context.SaveChangesAsync();

        // Create Doctor-Hospital Associations
        var associations = new List<DoctorHospitalAssociation>
        {
            new DoctorHospitalAssociation
            {
                Id = Guid.NewGuid(),
                DoctorId = doctors[0].Id,
                TenantId = tenants[0].Id,
                ConsultationFee = 150.00m,
                IsAvailableForHomeVisit = true,
                HomeVisitFee = 250.00m,
                JoinDate = DateTime.UtcNow.AddMonths(-6),
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new DoctorHospitalAssociation
            {
                Id = Guid.NewGuid(),
                DoctorId = doctors[1].Id,
                TenantId = tenants[0].Id,
                ConsultationFee = 120.00m,
                IsAvailableForHomeVisit = true,
                HomeVisitFee = 200.00m,
                JoinDate = DateTime.UtcNow.AddMonths(-4),
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new DoctorHospitalAssociation
            {
                Id = Guid.NewGuid(),
                DoctorId = doctors[2].Id,
                TenantId = tenants[1].Id,
                ConsultationFee = 180.00m,
                IsAvailableForHomeVisit = false,
                HomeVisitFee = null,
                JoinDate = DateTime.UtcNow.AddMonths(-3),
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new DoctorHospitalAssociation
            {
                Id = Guid.NewGuid(),
                DoctorId = doctors[3].Id,
                TenantId = tenants[1].Id,
                ConsultationFee = 200.00m,
                IsAvailableForHomeVisit = true,
                HomeVisitFee = 300.00m,
                JoinDate = DateTime.UtcNow.AddMonths(-2),
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new DoctorHospitalAssociation
            {
                Id = Guid.NewGuid(),
                DoctorId = doctors[4].Id,
                TenantId = tenants[2].Id,
                ConsultationFee = 100.00m,
                IsAvailableForHomeVisit = true,
                HomeVisitFee = 150.00m,
                JoinDate = DateTime.UtcNow.AddMonths(-1),
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new DoctorHospitalAssociation
            {
                Id = Guid.NewGuid(),
                DoctorId = doctors[5].Id,
                TenantId = tenants[3].Id,
                ConsultationFee = 140.00m,
                IsAvailableForHomeVisit = false,
                HomeVisitFee = null,
                JoinDate = DateTime.UtcNow.AddMonths(-5),
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        };

        await context.DoctorHospitalAssociations.AddRangeAsync(associations);
        await context.SaveChangesAsync();
        logger.LogInformation($"Seeded {doctorUsers.Count} doctors with hospital associations");
    }

    private static async Task SeedStaff(ApplicationDbContext context, List<Tenant> tenants, ILogger logger)
    {
        logger.LogInformation("Seeding staff...");
        var staffUsers = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "nurse.thompson@citygeneralhospital.com",
                PasswordHash = HashPassword("Staff@123"),
                FirstName = "Linda",
                LastName = "Thompson",
                Phone = "+1-555-3001",
                UserType = UserType.Staff,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "receptionist.garcia@citygeneralhospital.com",
                PasswordHash = HashPassword("Staff@123"),
                FirstName = "Carlos",
                LastName = "Garcia",
                Phone = "+1-555-3002",
                UserType = UserType.Staff,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "nurse.lee@sunrisemedical.com",
                PasswordHash = HashPassword("Staff@123"),
                FirstName = "Amy",
                LastName = "Lee",
                Phone = "+1-555-3003",
                UserType = UserType.Staff,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "lab.tech@sunrisemedical.com",
                PasswordHash = HashPassword("Staff@123"),
                FirstName = "Kevin",
                LastName = "White",
                Phone = "+1-555-3004",
                UserType = UserType.Staff,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "pharmacist@greenvalleyclinic.com",
                PasswordHash = HashPassword("Staff@123"),
                FirstName = "Patricia",
                LastName = "Moore",
                Phone = "+1-555-3005",
                UserType = UserType.Staff,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        };

        await context.Users.AddRangeAsync(staffUsers);
        await context.SaveChangesAsync();

        // Create Staff profiles
        var staff = new List<Staff>
        {
            new Staff
            {
                Id = Guid.NewGuid(),
                UserId = staffUsers[0].Id,
                TenantId = tenants[0].Id,
                Department = "Nursing",
                Role = "Senior Nurse",
                JoinDate = DateTime.UtcNow.AddYears(-3),
                CreatedDate = DateTime.UtcNow
            },
            new Staff
            {
                Id = Guid.NewGuid(),
                UserId = staffUsers[1].Id,
                TenantId = tenants[0].Id,
                Department = "Administration",
                Role = "Front Desk Receptionist",
                JoinDate = DateTime.UtcNow.AddYears(-1),
                CreatedDate = DateTime.UtcNow
            },
            new Staff
            {
                Id = Guid.NewGuid(),
                UserId = staffUsers[2].Id,
                TenantId = tenants[1].Id,
                Department = "Nursing",
                Role = "Registered Nurse",
                JoinDate = DateTime.UtcNow.AddYears(-2),
                CreatedDate = DateTime.UtcNow
            },
            new Staff
            {
                Id = Guid.NewGuid(),
                UserId = staffUsers[3].Id,
                TenantId = tenants[1].Id,
                Department = "Laboratory",
                Role = "Lab Technician",
                JoinDate = DateTime.UtcNow.AddMonths(-8),
                CreatedDate = DateTime.UtcNow
            },
            new Staff
            {
                Id = Guid.NewGuid(),
                UserId = staffUsers[4].Id,
                TenantId = tenants[2].Id,
                Department = "Pharmacy",
                Role = "Pharmacist",
                JoinDate = DateTime.UtcNow.AddMonths(-6),
                CreatedDate = DateTime.UtcNow
            }
        };

        await context.Staff.AddRangeAsync(staff);
        await context.SaveChangesAsync();
        logger.LogInformation($"Seeded {staffUsers.Count} staff members");
    }

    private static async Task SeedPatients(ApplicationDbContext context, ILogger logger)
    {
        logger.LogInformation("Seeding patients...");
        var patientUsers = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "patient1@example.com",
                PasswordHash = HashPassword("Patient@123"),
                FirstName = "Alice",
                LastName = "Cooper",
                Phone = "+1-555-4001",
                UserType = UserType.Patient,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "patient2@example.com",
                PasswordHash = HashPassword("Patient@123"),
                FirstName = "Bob",
                LastName = "Miller",
                Phone = "+1-555-4002",
                UserType = UserType.Patient,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "patient3@example.com",
                PasswordHash = HashPassword("Patient@123"),
                FirstName = "Catherine",
                LastName = "Harris",
                Phone = "+1-555-4003",
                UserType = UserType.Patient,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "patient4@example.com",
                PasswordHash = HashPassword("Patient@123"),
                FirstName = "Daniel",
                LastName = "Clark",
                Phone = "+1-555-4004",
                UserType = UserType.Patient,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "patient5@example.com",
                PasswordHash = HashPassword("Patient@123"),
                FirstName = "Emma",
                LastName = "Lewis",
                Phone = "+1-555-4005",
                UserType = UserType.Patient,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "patient6@example.com",
                PasswordHash = HashPassword("Patient@123"),
                FirstName = "Frank",
                LastName = "Walker",
                Phone = "+1-555-4006",
                UserType = UserType.Patient,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "patient7@example.com",
                PasswordHash = HashPassword("Patient@123"),
                FirstName = "Grace",
                LastName = "Hall",
                Phone = "+1-555-4007",
                UserType = UserType.Patient,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "patient8@example.com",
                PasswordHash = HashPassword("Patient@123"),
                FirstName = "Henry",
                LastName = "Young",
                Phone = "+1-555-4008",
                UserType = UserType.Patient,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        };

        await context.Users.AddRangeAsync(patientUsers);
        await context.SaveChangesAsync();

        // Create Patient profiles
        var patients = new List<Patient>
        {
            new Patient
            {
                Id = Guid.NewGuid(),
                UserId = patientUsers[0].Id,
                DateOfBirth = new DateTime(1985, 5, 15),
                Gender = Gender.Female,
                BloodGroup = "A+",
                Address = "100 Maple Street, New York, NY 10002",
                EmergencyContact = "+1-555-5001",
                MedicalHistory = "Hypertension",
                Allergies = "Penicillin",
                CreatedDate = DateTime.UtcNow
            },
            new Patient
            {
                Id = Guid.NewGuid(),
                UserId = patientUsers[1].Id,
                DateOfBirth = new DateTime(1978, 8, 22),
                Gender = Gender.Male,
                BloodGroup = "O+",
                Address = "200 Cedar Avenue, Los Angeles, CA 90002",
                EmergencyContact = "+1-555-5002",
                MedicalHistory = "Diabetes Type 2",
                Allergies = "None",
                CreatedDate = DateTime.UtcNow
            },
            new Patient
            {
                Id = Guid.NewGuid(),
                UserId = patientUsers[2].Id,
                DateOfBirth = new DateTime(1992, 3, 10),
                Gender = Gender.Female,
                BloodGroup = "B+",
                Address = "300 Birch Lane, Chicago, IL 60602",
                EmergencyContact = "+1-555-5003",
                MedicalHistory = "Asthma",
                Allergies = "Pollen, Dust",
                CreatedDate = DateTime.UtcNow
            },
            new Patient
            {
                Id = Guid.NewGuid(),
                UserId = patientUsers[3].Id,
                DateOfBirth = new DateTime(1965, 11, 30),
                Gender = Gender.Male,
                BloodGroup = "AB+",
                Address = "400 Willow Road, Houston, TX 77002",
                EmergencyContact = "+1-555-5004",
                MedicalHistory = "Heart Disease",
                Allergies = "Sulfa drugs",
                CreatedDate = DateTime.UtcNow
            },
            new Patient
            {
                Id = Guid.NewGuid(),
                UserId = patientUsers[4].Id,
                DateOfBirth = new DateTime(1988, 7, 18),
                Gender = Gender.Female,
                BloodGroup = "O-",
                Address = "500 Spruce Street, Phoenix, AZ 85001",
                EmergencyContact = "+1-555-5005",
                MedicalHistory = "None",
                Allergies = "None",
                CreatedDate = DateTime.UtcNow
            },
            new Patient
            {
                Id = Guid.NewGuid(),
                UserId = patientUsers[5].Id,
                DateOfBirth = new DateTime(1995, 12, 5),
                Gender = Gender.Male,
                BloodGroup = "A-",
                Address = "600 Ash Boulevard, Philadelphia, PA 19101",
                EmergencyContact = "+1-555-5006",
                MedicalHistory = "Migraine",
                Allergies = "Latex",
                CreatedDate = DateTime.UtcNow
            },
            new Patient
            {
                Id = Guid.NewGuid(),
                UserId = patientUsers[6].Id,
                DateOfBirth = new DateTime(1982, 4, 25),
                Gender = Gender.Female,
                BloodGroup = "B-",
                Address = "700 Elm Court, San Antonio, TX 78201",
                EmergencyContact = "+1-555-5007",
                MedicalHistory = "Thyroid disorder",
                Allergies = "Iodine",
                CreatedDate = DateTime.UtcNow
            },
            new Patient
            {
                Id = Guid.NewGuid(),
                UserId = patientUsers[7].Id,
                DateOfBirth = new DateTime(1970, 9, 14),
                Gender = Gender.Male,
                BloodGroup = "AB-",
                Address = "800 Pine Plaza, San Diego, CA 92101",
                EmergencyContact = "+1-555-5008",
                MedicalHistory = "Arthritis",
                Allergies = "Aspirin",
                CreatedDate = DateTime.UtcNow
            }
        };

        await context.Patients.AddRangeAsync(patients);
        await context.SaveChangesAsync();
        logger.LogInformation($"Seeded {patientUsers.Count} patients");
    }

    private static string HashPassword(string password)
    {
        // Using the same hashing method as AuthService
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
