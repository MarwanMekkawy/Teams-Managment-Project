using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class userPwAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDEleted",
                table: "Users",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDEleted",
                table: "Teams",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDEleted",
                table: "Tasks",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDEleted",
                table: "Projects",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsDEleted",
                table: "Organizations",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Users",
                newName: "IsDEleted");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Teams",
                newName: "IsDEleted");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Tasks",
                newName: "IsDEleted");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Projects",
                newName: "IsDEleted");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Organizations",
                newName: "IsDEleted");
        }
    }
}
