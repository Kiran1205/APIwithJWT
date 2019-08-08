using MAPICore.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAPICore.Data.EntityConfiguration
{
    public class ApplicationInsightConfiguration : IEntityTypeConfiguration<ApplicationInsight>
    {
        public void Configure(EntityTypeBuilder<ApplicationInsight> builder)
        {
            builder.ToTable("appinsight");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnName("Id");

            builder.Property(m => m.Type)
               .HasColumnName("type");

            builder.Property(m => m.Message)
               .HasColumnName("message");

            builder.Property(m => m.CreatedDate)
               .HasColumnName("createddate");

            builder.Property(m => m.IpAddress)
              .HasColumnName("ipaddress");

            builder.Property(m => m.UserId)
              .HasColumnName("userid");

        }
    }
}
