using HealthcareSaaS.Application.Common.Interfaces;
using HealthcareSaaS.Domain.Common;
using HealthcareSaaS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthcareSaaS.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ITenantService _tenantService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService)
        : base(options)
    {
        _tenantService = tenantService;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<DoctorHospitalAssociation> DoctorHospitalAssociations => Set<DoctorHospitalAssociation>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<TimeSlot> TimeSlots => Set<TimeSlot>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<ServiceUnit> ServiceUnits => Set<ServiceUnit>();
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
    public DbSet<LabTest> LabTests => Set<LabTest>();
    public DbSet<PharmacyOrder> PharmacyOrders => Set<PharmacyOrder>();
    public DbSet<PharmacyOrderItem> PharmacyOrderItems => Set<PharmacyOrderItem>();
    public DbSet<ImagingStudy> ImagingStudies => Set<ImagingStudy>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Global query filter for soft delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var property = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var filterExpression = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(false)),
                    parameter);
                
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filterExpression);
            }
        }

        // Global query filter for multi-tenancy
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseTenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(SetTenantFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                    .MakeGenericMethod(entityType.ClrType);
                
                method.Invoke(this, new object[] { modelBuilder });
            }
        }
    }

    private void SetTenantFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseTenantEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => 
            _tenantService.GetTenantId() == Guid.Empty || e.TenantId == _tenantService.GetTenantId());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                    break;
            }

            if (entry.Entity is BaseTenantEntity tenantEntity && entry.State == EntityState.Added)
            {
                // Only set TenantId if it's not already set (Guid.Empty means not set)
                if (tenantEntity.TenantId == Guid.Empty)
                {
                    var currentTenantId = _tenantService.GetTenantId();
                    if (currentTenantId != Guid.Empty)
                    {
                        tenantEntity.TenantId = currentTenantId;
                    }
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
