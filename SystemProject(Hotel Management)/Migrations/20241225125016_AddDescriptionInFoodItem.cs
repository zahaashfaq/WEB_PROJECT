using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemProject_Hotel_Management_.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionInFoodItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "FoodItems");
        }
    }
}
