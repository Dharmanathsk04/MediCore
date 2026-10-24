using HealthcareSaaS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthcareSaaS.Infrastructure.Data.Configurations;

public class DoctorHospitalAssociationConfiguration : IEntityTypeConfiguration<DoctorHospitalAssociation>
{
    public void Configure(EntityTypeBuilder<DoctorHospitalAssociation> builder)
    {
        builder.HasKey(dha => dha.Id);

        builder.Property(dha => dha.TenantId)
            .IsRequired();

        builder.HasOne(dha => dha.Doctor)
            .WithMany(d => d.HospitalAssociations)
            .HasForeignKey(dha => dha.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(dha => dha.Tenant)
            .WithMany(t => t.DoctorAssociations)
            .HasForeignKey(dha => dha.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(dha => new { dha.DoctorId, dha.TenantId });

        builder.Property(dha => dha.ConsultationFee)
            .HasColumnType("decimal(18,2)");

        builder.Property(dha => dha.HomeVisitFee)
            .HasColumnType("decimal(18,2)");
    }
}
