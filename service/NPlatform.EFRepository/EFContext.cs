using Microsoft.EntityFrameworkCore;
using NPlatform.Domains.Entity.Sys;

namespace NPlatform.Repositories
{
    public partial class EFContext : DbContext
    {

        public EFContext(DbContextOptions<EFContext> options)
            : base(options)
        {
            
        }

        private readonly string _connectionString;
        private DBProvider DBProvider;
        private int TimeOut;

        public EFContext(string connectionString, DBProvider dbProvider, int timeOut=180)
        {
            _connectionString = connectionString;
            DBProvider = dbProvider;
            TimeOut = timeOut;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch(DBProvider)
            {
                case DBProvider.SqlClient: // 先安装 sqlclient包
                   // optionsBuilder.UseSqlServer(_connectionString);
                    break;
                case DBProvider.MySqlClient:
                    optionsBuilder.UseMySQL(_connectionString);
                    break;
            }
            
        }

        public virtual DbSet<User> Users { get; set; }
        //public virtual DbSet<POCTask> POCTasks { get; set; }
        //public virtual DbSet<POCTaskResource> POCTaskResources { get; set; }
        //public virtual DbSet<TestQuestions> Questions { get; set; }
        //public virtual DbSet<TestResource> Resources { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Testuser>(entity =>
        //    {
        //        entity.ToTable("testuser");

        //        entity.Property(e => e.Id)
        //            .HasMaxLength(200)
        //            .HasColumnName("id")
        //            .HasComment("id");

        //        entity.Property(e => e.Address)
        //            .HasMaxLength(255)
        //            .HasColumnName("address")
        //            .HasComment("地址");

        //        entity.Property(e => e.Password)
        //            .HasMaxLength(255)
        //            .HasColumnName("password")
        //            .HasComment("密码");

        //        entity.Property(e => e.UserName)
        //            .HasMaxLength(255)
        //            .HasColumnName("userName")
        //            .HasComment("用户名");

        //        entity.Property(e => e.Work)
        //            .HasMaxLength(255)
        //            .HasColumnName("work")
        //            .HasComment("工作");
        //    });

        //    OnModelCreatingPartial(modelBuilder);
        //}

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
