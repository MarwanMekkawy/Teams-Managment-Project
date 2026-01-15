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
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(u => u.Role)
                   .IsRequired();

            builder.HasMany(u => u.TeamMemberships)
                   .WithOne(tm => tm.User)
                   .HasForeignKey(tm => tm.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.AssignedTasks)
                   .WithOne(t => t.Assignee)
                   .HasForeignKey(t => t.AssigneeId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Apply soft delete filter 
            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }
}
