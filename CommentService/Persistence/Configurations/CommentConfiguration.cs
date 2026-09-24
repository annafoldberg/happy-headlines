using CommentService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommentService.Persistence.Configurations;

public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");

        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.PublicId).IsUnique();

        builder.Property(c => c.Id).HasColumnName("id");

        builder.Property(c => c.PublicId)
            .HasColumnName("public_id")
            .IsRequired();

        builder.Property(c => c.ArticleId)
            .HasColumnName("article_id")
            .IsRequired();

        builder.Property(c => c.Author)
            .HasColumnName("author")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Content)
            .HasColumnName("content")
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.CreationTimestampUtc)
            .HasColumnName("creation_timestamp_utc")
            .IsRequired();

        builder.Property(a => a.LastUpdatedTimestampUtc)
            .HasColumnName("last_updated_timestamp_utc");
    }
}