using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfanityService.Entities;

namespace ProfanityService.Persistence.Configurations;

public sealed class ProfanityConfiguration : IEntityTypeConfiguration<Profanity>
{
    public void Configure(EntityTypeBuilder<Profanity> builder)
    {
        builder.ToTable("profanities");

        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.Term).IsUnique();

        builder.Property(p => p.Id).HasColumnName("id");

        builder.Property(p => p.Term)
            .HasColumnName("term")
            .IsRequired()
            .HasMaxLength(100);
    }
}