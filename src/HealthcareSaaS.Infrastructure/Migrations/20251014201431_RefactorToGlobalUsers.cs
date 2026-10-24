using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthcareSaaS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorToGlobalUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email_TenantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TenantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Staff_UserId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Admins_UserId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConsultationFee",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "HomeVisitFee",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "IsAvailableForHomeVisit",
                table: "Doctors");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Staff",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Admins",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DoctorHospitalAssociations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsultationFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailableForHomeVisit = table.Column<bool>(type: "bit", nullable: false),
                    HomeVisitFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorHospitalAssociations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorHospitalAssociations_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorHospitalAssociations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staff_TenantId",
                table: "Staff",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UserId",
                table: "Staff",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_TenantId",
                table: "Admins",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorHospitalAssociations_DoctorId_TenantId",
                table: "DoctorHospitalAssociations",
                columns: new[] { "DoctorId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorHospitalAssociations_TenantId",
                table: "DoctorHospitalAssociations",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Tenants_TenantId",
                table: "Admins",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Tenants_TenantId",
                table: "Staff",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Tenants_TenantId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Tenants_TenantId",
                table: "Staff");

            migrationBuilder.DropTable(
                name: "DoctorHospitalAssociations");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Staff_TenantId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_UserId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Admins_TenantId",
                table: "Admins");

            migrationBuilder.DropIndex(
                name: "IX_Admins_UserId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Admins");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "ConsultationFee",
                table: "Doctors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HomeVisitFee",
                table: "Doctors",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableForHomeVisit",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_TenantId",
                table: "Users",
                columns: new[] { "Email", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId",
                table: "Users",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UserId",
                table: "Staff",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
