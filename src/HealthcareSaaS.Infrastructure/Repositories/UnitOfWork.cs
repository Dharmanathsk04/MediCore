using HealthcareSaaS.Application.Common.Interfaces;
using HealthcareSaaS.Domain.Entities;
using HealthcareSaaS.Infrastructure.Data;

namespace HealthcareSaaS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Tenants = new Repository<Tenant>(_context);
        Users = new Repository<User>(_context);
        Patients = new Repository<Patient>(_context);
        Doctors = new Repository<Doctor>(_context);
        DoctorHospitalAssociations = new Repository<DoctorHospitalAssociation>(_context);
        Staff = new Repository<Staff>(_context);
        Admins = new Repository<Admin>(_context);
        TimeSlots = new Repository<TimeSlot>(_context);
        Appointments = new Repository<Appointment>(_context);
        Prescriptions = new Repository<Prescription>(_context);
        Medicines = new Repository<Medicine>(_context);
        ServiceUnits = new Repository<ServiceUnit>(_context);
        ServiceRequests = new Repository<ServiceRequest>(_context);
        LabTests = new Repository<LabTest>(_context);
        PharmacyOrders = new Repository<PharmacyOrder>(_context);
        ImagingStudies = new Repository<ImagingStudy>(_context);
    }

    public IRepository<Tenant> Tenants { get; }
    public IRepository<User> Users { get; }
    public IRepository<Patient> Patients { get; }
    public IRepository<Doctor> Doctors { get; }
    public IRepository<DoctorHospitalAssociation> DoctorHospitalAssociations { get; }
    public IRepository<Staff> Staff { get; }
    public IRepository<Admin> Admins { get; }
    public IRepository<TimeSlot> TimeSlots { get; }
    public IRepository<Appointment> Appointments { get; }
    public IRepository<Prescription> Prescriptions { get; }
    public IRepository<Medicine> Medicines { get; }
    public IRepository<ServiceUnit> ServiceUnits { get; }
    public IRepository<ServiceRequest> ServiceRequests { get; }
    public IRepository<LabTest> LabTests { get; }
    public IRepository<PharmacyOrder> PharmacyOrders { get; }
    public IRepository<ImagingStudy> ImagingStudies { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
