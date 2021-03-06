﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rhisis.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Database.Entities.Configuration
{
    public class ItemStorageTypeConfiguration : IEntityTypeConfiguration<DbItemStorageType>
    {
        public void Configure(EntityTypeBuilder<DbItemStorageType> builder)
        {
            builder.ToTable("ItemStorageTypes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(20).HasColumnType("VARCHAR(20)");

            SeedData(builder);
        }

        private void SeedData(EntityTypeBuilder<DbItemStorageType> builder)
        {
            IEnumerable<DbAttribute> initialValues = Enum.GetValues(typeof(ItemStorageType))
                   .Cast<ItemStorageType>()
                   .Select(x => new DbAttribute
                   {
                       Id = (int)x,
                       Name = x.ToString()
                   });

            builder.HasData(initialValues);
        }
    }
}
