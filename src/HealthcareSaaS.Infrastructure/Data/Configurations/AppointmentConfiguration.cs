using HealthcareSaaS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareSaaS.Infrastructure.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.TimeSlot)
            .WithOne(t => t.Appointment)
            .HasForeignKey<Appointment>(a => a.TimeSlotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Prescription)
            .WithOne(p => p.Appointment)
            .HasForeignKey<Prescription>(p => p.AppointmentId);

        builder.HasIndex(a => a.AppointmentDate);
        builder.HasIndex(a => new { a.DoctorId, a.AppointmentDate });
        builder.HasIndex(a => new { a.PatientId, a.AppointmentDate });
    }
}
