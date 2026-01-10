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
    public class OrganizationConfig : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("Organizations");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // Relationships
            builder.HasMany(o => o.Users)
                   .WithOne(u => u.Organization)
                   .HasForeignKey(u => u.OrganizationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.Teams)
                   .WithOne(t => t.Organization)
                   .HasForeignKey(t => t.OrganizationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
