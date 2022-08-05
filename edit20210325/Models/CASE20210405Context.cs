using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace edit20210325.Models
{
    public partial class CASE20210405Context : DbContext
    {
        public CASE20210405Context()
        {
        }

        public CASE20210405Context(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<SalesOrderModel> SalesOrderModels { get; set; }
        public virtual DbSet<MemberModel> MemberModels { get; set; }
        public virtual DbSet<ManagerModel> ManagerModels { get; set; }
        public virtual DbSet<MemberSaveModel> MemberSaveModels { get; set; }
        public virtual DbSet<MemberSaveLogModel> MemberSaveLogModels { get; set; }
        public virtual DbSet<ConsumerOrderLogModel> ConsumerOrderLogModels { get; set; }
        public virtual DbSet<MemberCashInModel> MemberCashInModels { get; set; }
        public virtual DbSet<VideoFreeModel> VideoFreeModels { get; set; }
        public virtual DbSet<VideoPayModel> VideoPayModels { get; set; }
        public virtual DbSet<DepositOrderLogModel> DepositOrderLogModels { get; set; }
        public virtual DbSet<FileLoadInfoModel> FileLoadInfoModels { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("caseDatabase");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");
            modelBuilder.Entity<MemberCashInModel>().HasKey(table => new
            {
                table.MemberCashInMemberGmail,
                table.MemberCashInOrderID
            });

            modelBuilder.Entity<SalesOrderModel>().HasKey(table => new
            {
                table.SalesOrderMemberGmail,
                table.SalesOrderOrderID,
                table.SalesOrderItem,
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
