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
    public class TeamMemberConfig : IEntityTypeConfiguration<TeamMember>
    {            
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.ToTable("TeamMembers");

            builder.HasKey(tm => new { tm.TeamId, tm.UserId });

            builder.HasOne(tm => tm.Team)
                   .WithMany(t => t.Members)
                   .HasForeignKey(tm => tm.TeamId)
                   .OnDelete(DeleteBehavior.Cascade).IsRequired(false);

            builder.HasOne(tm => tm.User)
                   .WithMany(u => u.TeamMemberships)
                   .HasForeignKey(tm => tm.UserId)
                   .OnDelete(DeleteBehavior.Cascade).IsRequired(false);
        }
    }
}
