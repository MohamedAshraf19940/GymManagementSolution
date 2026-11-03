using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.DATA.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);

            builder.Property(x => x.Email)
                  .HasColumnType("varchar")
                  .HasMaxLength(70);

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();

            //_%@_%._%
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email like '_%@_%._%'");
                tb.HasCheckConstraint("PhoneCheck", "Phone like '010%' or Phone like '011%' or Phone like '012%' or Phone like '015%' ");
            });
            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(x => x.Street)
                       .HasColumnName("Street")
                       .HasColumnType("varchar")
                       .HasMaxLength(30);

                address.Property(x => x.City)
                       .HasColumnName("City")
                       .HasColumnType("varchar")
                       .HasMaxLength(30);
            });
        }
    }
}
