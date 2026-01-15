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
    public class TeamConfig : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Teams");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // Relationships
            builder.HasMany(t => t.Members)
                   .WithOne(tm => tm.Team)
                   .HasForeignKey(tm => tm.TeamId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Projects)
                   .WithOne(p => p.Team)
                   .HasForeignKey(p => p.TeamId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Apply soft delete filter 
            builder.HasQueryFilter(t => !t.IsDeleted);
        }
    }
}
