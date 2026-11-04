using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriberService.Entities;

namespace SubscriberService.Persistence.Configurations;

public sealed class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.ToTable("subscribers");

        builder.HasKey(s => s.Id);

        builder.HasIndex(s => s.PublicId).IsUnique();

        builder.Property(s => s.Id).HasColumnName("id");

        builder.Property(s => s.PublicId)
            .HasColumnName("public_id")
            .IsRequired();

        builder.Property(s => s.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.SubscriptionTimestampUtc)
            .HasColumnName("subscription_timestamp_utc")
            .IsRequired();
    }
}