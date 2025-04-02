using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddMealIngredientRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Meals_MealId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_MealMealPlan_MealPlan_MealPlansId",
                table: "MealMealPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPlan_AspNetUsers_UserId",
                table: "MealPlan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealPlan",
                table: "MealPlan");

            migrationBuilder.RenameTable(
                name: "MealPlan",
                newName: "MealPlans");

            migrationBuilder.RenameIndex(
                name: "IX_MealPlan_UserId",
                table: "MealPlans",
                newName: "IX_MealPlans_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "MealId",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealPlans",
                table: "MealPlans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Meals_MealId",
                table: "Ingredients",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealMealPlan_MealPlans_MealPlansId",
                table: "MealMealPlan",
                column: "MealPlansId",
                principalTable: "MealPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_AspNetUsers_UserId",
                table: "MealPlans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Meals_MealId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_MealMealPlan_MealPlans_MealPlansId",
                table: "MealMealPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_AspNetUsers_UserId",
                table: "MealPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealPlans",
                table: "MealPlans");

            migrationBuilder.RenameTable(
                name: "MealPlans",
                newName: "MealPlan");

            migrationBuilder.RenameIndex(
                name: "IX_MealPlans_UserId",
                table: "MealPlan",
                newName: "IX_MealPlan_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "MealId",
                table: "Ingredients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealPlan",
                table: "MealPlan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Meals_MealId",
                table: "Ingredients",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealMealPlan_MealPlan_MealPlansId",
                table: "MealMealPlan",
                column: "MealPlansId",
                principalTable: "MealPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlan_AspNetUsers_UserId",
                table: "MealPlan",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
