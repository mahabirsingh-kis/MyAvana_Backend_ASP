using Microsoft.EntityFrameworkCore;
using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.ExcelImport
{
    public class EFCoreDemoContext : DbContext
    {
        public DbSet<ProductEntity> ProductEntities { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=parlak.cumx0rngyid8.ap-south-1.rds.amazonaws.com;Initial Catalog=Avana_Live;Integrated Security=False;User Id=Parlak;Password=parlak123");
        }
    }
}
