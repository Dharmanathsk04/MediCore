using HealthcareSaaS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareSaaS.Infrastructure.Data.Configurations;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Appointment)
            .WithOne(a => a.Prescription)
            .HasForeignKey<Prescription>(p => p.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Doctor)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Patient)
            .WithMany(pt => pt.Prescriptions)
            .HasForeignKey(p => p.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Medicines)
            .WithOne(m => m.Prescription)
            .HasForeignKey(m => m.PrescriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
