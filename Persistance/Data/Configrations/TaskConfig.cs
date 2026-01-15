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
    public class TaskConfig : IEntityTypeConfiguration<Domain.Entities.TaskEntity>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.TaskEntity> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(t => t.Status)
                   .IsRequired();

            builder.HasOne(t => t.Assignee)
                   .WithMany(u => u.AssignedTasks)
                   .HasForeignKey(t => t.AssigneeId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Apply soft delete filter 
            builder.HasQueryFilter(t => !t.IsDeleted);
        }
    }
}
