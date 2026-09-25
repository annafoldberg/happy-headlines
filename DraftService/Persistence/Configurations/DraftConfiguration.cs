using DraftService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DraftService.Persistence.Configurations;

public sealed class DraftConfiguration : IEntityTypeConfiguration<Draft>
{
    public void Configure(EntityTypeBuilder<Draft> builder)
    {
        builder.ToTable("drafts");

        builder.HasKey(d => d.Id);

        builder.HasIndex(d => d.PublicId).IsUnique();

        builder.Property(d => d.Id).HasColumnName("id");

        builder.Property(d => d.PublicId)
            .HasColumnName("public_id")
            .IsRequired();

        builder.Property(d => d.Author)
            .HasColumnName("author")
            .HasMaxLength(100);

        builder.Property(d => d.Title)
            .HasColumnName("title")
            .HasMaxLength(200);

        builder.Property(d => d.Content).HasColumnName("content");

        builder.Property(d => d.CreationTimestampUtc)
            .HasColumnName("creation_timestamp_utc")
            .IsRequired();

        builder.Property(d => d.LastUpdatedTimestampUtc)
            .HasColumnName("last_updated_timestamp_utc");
    }
}