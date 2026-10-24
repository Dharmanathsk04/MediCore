using HealthcareSaaS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthcareSaaS.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<User> Users { get; }
    DbSet<Patient> Patients { get; }
    DbSet<Doctor> Doctors { get; }
    DbSet<Staff> Staff { get; }
    DbSet<Admin> Admins { get; }
    DbSet<TimeSlot> TimeSlots { get; }
    DbSet<Appointment> Appointments { get; }
    DbSet<Prescription> Prescriptions { get; }
    DbSet<Medicine> Medicines { get; }
    DbSet<ServiceUnit> ServiceUnits { get; }
    DbSet<ServiceRequest> ServiceRequests { get; }
    DbSet<LabTest> LabTests { get; }
    DbSet<PharmacyOrder> PharmacyOrders { get; }
    DbSet<PharmacyOrderItem> PharmacyOrderItems { get; }
    DbSet<ImagingStudy> ImagingStudies { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
