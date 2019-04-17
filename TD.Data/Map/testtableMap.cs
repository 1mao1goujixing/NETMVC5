using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.Data.Entity;

namespace TD.Data.Map
{
   internal class testtableMap: EntityTypeConfiguration<testtable>
    {

        public testtableMap(string dbName) {

            ToTable("testtable", dbName);
            HasKey(table => table.Id);
            Property(table => table.Id).HasColumnName("id").IsRequired();

            Property(table => table.Name).HasColumnName("Name");
            Property(table => table.Pwd).HasColumnName("Pwd");
            Property(table => table.Remark).HasColumnName("Remark");
            Property(table => table.Home).HasColumnName("Home");
            Property(table => table.Gender).HasColumnName("Gender");

        }
    }
}
