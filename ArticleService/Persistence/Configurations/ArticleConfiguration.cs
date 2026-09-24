using ArticleService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleService.Persistence.Configurations;

public sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");

        builder.HasKey(a => a.Id);

        builder.HasIndex(a => a.PublicId).IsUnique();

        builder.Property(a => a.Id).HasColumnName("id");

        builder.Property(a => a.PublicId)
            .HasColumnName("public_id")
            .IsRequired();

        builder.Property(a => a.Author)
            .HasColumnName("author")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(a => a.PublicationTimestampUtc)
            .HasColumnName("publication_timestamp_utc")
            .IsRequired();

        builder.Property(a => a.LastUpdatedTimestampUtc)
            .HasColumnName("last_updated_timestamp_utc");
    }
}