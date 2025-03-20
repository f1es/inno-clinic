using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEndTimeToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Appointments",
                newName: "EndTime");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "BeginTime",
                table: "Appointments",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeginTime",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Appointments",
                newName: "Time");
        }
    }
}
