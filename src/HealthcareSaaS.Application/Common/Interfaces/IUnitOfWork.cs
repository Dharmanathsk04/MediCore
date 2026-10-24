using HealthcareSaaS.Domain.Entities;

namespace HealthcareSaaS.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Tenant> Tenants { get; }
    IRepository<User> Users { get; }
    IRepository<Patient> Patients { get; }
    IRepository<Doctor> Doctors { get; }
    IRepository<DoctorHospitalAssociation> DoctorHospitalAssociations { get; }
    IRepository<Staff> Staff { get; }
    IRepository<Admin> Admins { get; }
    IRepository<TimeSlot> TimeSlots { get; }
    IRepository<Appointment> Appointments { get; }
    IRepository<Prescription> Prescriptions { get; }
    IRepository<Medicine> Medicines { get; }
    IRepository<ServiceUnit> ServiceUnits { get; }
    IRepository<ServiceRequest> ServiceRequests { get; }
    IRepository<LabTest> LabTests { get; }
    IRepository<PharmacyOrder> PharmacyOrders { get; }
    IRepository<ImagingStudy> ImagingStudies { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
