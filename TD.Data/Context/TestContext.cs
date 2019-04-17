using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.Data.Entity;
using TD.Data.Map;
using TD.IRepository;
using TD.IRepository.IRepositorycom;

namespace TD.Data.Context
{
  public  class TestContext:EntityContext<SMSqlDbObject>
    {
        public TestContext() : base("test") {


        }
        static TestContext()
        {
            Database.SetInitializer<TestContext>(null);
        }
        private static string dbName="Test";
        public DbSet<testtable> testtable { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new testtableMap(dbName));
        }
    }
}
