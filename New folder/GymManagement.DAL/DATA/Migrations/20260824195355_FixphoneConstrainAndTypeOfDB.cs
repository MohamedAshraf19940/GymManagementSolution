using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.DAL.DATA.Migrations
{
    /// <inheritdoc />
    public partial class FixphoneConstrainAndTypeOfDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "HealthRecords",
                newName: "Weight");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Trainers",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Members",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers",
                sql: "Phone like '010%' or Phone like '011%' or Phone like '012%' or Phone like '015%' ");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck",
                table: "Members",
                sql: "Phone like '010%' or Phone like '011%' or Phone like '012%' or Phone like '015%' ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "HealthRecords",
                newName: "Width");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Trainers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Members",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers",
                sql: "Phone like '010' or Phone like '011' or Phone like '012' or Phone like '015' ");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck",
                table: "Members",
                sql: "Phone like '010' or Phone like '011' or Phone like '012' or Phone like '015' ");
        }
    }
}
