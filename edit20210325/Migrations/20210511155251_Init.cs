using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace edit20210325.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumerOrderLogModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberCashInCash = table.Column<int>(type: "int", nullable: false),
                    MemberCashInDays = table.Column<int>(type: "int", nullable: false),
                    MemberCashInCounts = table.Column<int>(type: "int", nullable: false),
                    SalesOrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberCashInRemarks1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberCashInRemarks2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberCashInCrtDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumerOrderLogModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositOrderLogModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberCashInCash = table.Column<int>(type: "int", nullable: false),
                    CashInId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberCashInRemarks1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberCashInRemarks2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberCashInCrtDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositOrderLogModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileLoadInfoModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MmberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileMac = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUpDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileLoadInfoModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManagerModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManagerUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberCashInModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberCashInId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberCashInCash = table.Column<int>(type: "int", nullable: false),
                    MemberCashInName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberCashInRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberCashInSpace1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberCashInSpace2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberCashInSpace3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberCashInCrtDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberCashInChgDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberCashInModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberGmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberIden = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberRemarks1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberRemarks2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberSpace1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberSpace2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberCreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberSaveLogModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberSaveId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberSaveCash = table.Column<int>(type: "int", nullable: false),
                    MemberSaveRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberSaveCreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberSaveLogModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberSaveModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberSaveId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberSaveCash = table.Column<int>(type: "int", nullable: false),
                    MemberSaveRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberSaveSpace1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberSaveSpace2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberSaveCreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberSaveChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberSaveModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCharge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCounts = table.Column<int>(type: "int", nullable: false),
                    ProductUnitDays = table.Column<int>(type: "int", nullable: false),
                    ProductUnitPrice = table.Column<int>(type: "int", nullable: false),
                    ProductRemarks1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductRemarks2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductDays = table.Column<int>(type: "int", nullable: false),
                    ProductCreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoFreeModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    videoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    videoId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    videoTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    videoIntro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    videoCount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    videoRemark1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    videoRemark2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoFreeModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoPayModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    videoImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    videoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    videoTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    videoIntro = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoPayModels", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumerOrderLogModels");

            migrationBuilder.DropTable(
                name: "DepositOrderLogModels");

            migrationBuilder.DropTable(
                name: "FileLoadInfoModels");

            migrationBuilder.DropTable(
                name: "ManagerModels");

            migrationBuilder.DropTable(
                name: "MemberCashInModels");

            migrationBuilder.DropTable(
                name: "MemberModels");

            migrationBuilder.DropTable(
                name: "MemberSaveLogModels");

            migrationBuilder.DropTable(
                name: "MemberSaveModels");

            migrationBuilder.DropTable(
                name: "SalesOrderModels");

            migrationBuilder.DropTable(
                name: "VideoFreeModels");

            migrationBuilder.DropTable(
                name: "VideoPayModels");
        }
    }
}
