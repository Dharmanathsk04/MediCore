using HealthcareSaaS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareSaaS.Infrastructure.Data.Configurations;

public class ServiceRequestConfiguration : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.HasKey(sr => sr.Id);

        builder.HasOne(sr => sr.Appointment)
            .WithMany(a => a.ServiceRequests)
            .HasForeignKey(sr => sr.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sr => sr.Patient)
            .WithMany(p => p.ServiceRequests)
            .HasForeignKey(sr => sr.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sr => sr.ServiceUnit)
            .WithMany(su => su.ServiceRequests)
            .HasForeignKey(sr => sr.ServiceUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sr => sr.LabTest)
            .WithOne(lt => lt.ServiceRequest)
            .HasForeignKey<LabTest>(lt => lt.ServiceRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sr => sr.PharmacyOrder)
            .WithOne(po => po.ServiceRequest)
            .HasForeignKey<PharmacyOrder>(po => po.ServiceRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sr => sr.ImagingStudy)
            .WithOne(im => im.ServiceRequest)
            .HasForeignKey<ImagingStudy>(im => im.ServiceRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
