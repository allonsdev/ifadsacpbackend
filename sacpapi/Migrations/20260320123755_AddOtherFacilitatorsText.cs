using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sacpapi.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherFacilitatorsText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherFacilitatorsText",
                table: "GeneralActivities",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherFacilitatorsText",
                table: "GeneralActivities");
        }
    }
}
