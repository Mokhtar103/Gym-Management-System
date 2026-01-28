using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
        

            builder.HasOne(s => s.Trainer)
                   .WithMany(t => t.Sessions)
                   .HasForeignKey(s => s.TrainerId);

            builder.HasOne(s => s.Category)
                   .WithMany(c => c.Sessions)
                   .HasForeignKey(s => s.CategoryId);

            builder.ToTable(x =>
            {
                x.HasCheckConstraint("Session_CapacityCheck", "Capacity Between 1 and 25");
                x.HasCheckConstraint("Session_DateCheck", "EndDate > StartDate");
            });


        }
    }
}
