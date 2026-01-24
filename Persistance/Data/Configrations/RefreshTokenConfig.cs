using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Data.Configrations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.TokenHash)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.HasIndex(rt => rt.TokenHash)
                   .IsUnique();

            builder.Property(rt => rt.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(rt => rt.ExpiresAt)
                   .IsRequired();

            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade).IsRequired(false);
        }
    }
}
