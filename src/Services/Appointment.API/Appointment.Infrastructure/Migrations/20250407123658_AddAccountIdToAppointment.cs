using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountIdToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Appointments",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Appointments");
        }
    }
}
