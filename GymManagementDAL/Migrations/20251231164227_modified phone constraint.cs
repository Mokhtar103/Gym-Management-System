using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementDAL.Migrations
{
    /// <inheritdoc />
    public partial class modifiedphoneconstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "GymUser_PhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "GymUser_EmailCheck1",
                table: "Trainers");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Trainers",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Trainers",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AddCheckConstraint(
                name: "GymUser_PhoneCheck1",
                table: "Trainers",
                sql: "Phone LIKE '01%' and Phone Not Like '%[^0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "GymUser_EmailCheck1",
                table: "Trainers",
                sql: "Email LIKE '_%@_%._%'");

            migrationBuilder.DropCheckConstraint(
                name: "GymUser_PhoneCheck",
                table: "Members");

            migrationBuilder.DropCheckConstraint(
                name: "GymUser_EmailCheck",
                table: "Members");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Members",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Members",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AddCheckConstraint(
               name: "GymUser_PhoneCheck",
               table: "Members",
               sql: "Phone LIKE '01%' and Phone Not Like '%[^0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "GymUser_EmailCheck",
                table: "Members",
                sql: "Email LIKE '_%@_%._%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint("GymUser_PhoneCheck1", "Trainers");
            migrationBuilder.DropCheckConstraint("GymUser_EmailCheck1", "Trainers");
            migrationBuilder.DropCheckConstraint("GymUser_PhoneCheck", "Members");
            migrationBuilder.DropCheckConstraint("GymUser_EmailCheck", "Members");
           
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Trainers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Trainers",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Members",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Members",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddCheckConstraint(
               name: "GymUser_PhoneCheck1",
               table: "Trainers",
               sql: "Phone LIKE '01%' and Phone Not Like '%[^0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "GymUser_EmailCheck1",
                table: "Trainers",
                sql: "Email LIKE '_%@_%._%'");

            migrationBuilder.AddCheckConstraint(
               name: "GymUser_PhoneCheck",
               table: "Members",
               sql: "Phone LIKE '01%' and Phone Not Like '%[^0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "GymUser_EmailCheck",
                table: "Members",
                sql: "Email LIKE '_%@_%._%'");

           
        }
    }
}
