using MAPICore.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAPICore.Data.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
               .HasColumnName("Id");

            builder.Property(m => m.UserName)
                .HasColumnName("username");

            builder.Property(m => m.Password)
               .HasColumnName("pwd");

            builder.Property(m => m.EmailId)
               .HasColumnName("emailid");

        }
    }
}
