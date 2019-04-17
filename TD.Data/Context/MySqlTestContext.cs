using System.Data.Entity;
using TD.Data.Entity;
using TD.Data.Map;
using TD.IRepository;
using TD.IRepository.IRepositorycom;

namespace TD.Data.Context
{
    public class MySqlTestContext : EntityContext<MySqlDbObject>
    {
        public MySqlTestContext() : base("Mytest")
        {


        }
        static MySqlTestContext()
        {
            Database.SetInitializer<MySqlTestContext>(null);
        }
        private static string dbName = "Test";
        public DbSet<testtable> testtable { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new testtableMap(dbName));
        }
    }
}
