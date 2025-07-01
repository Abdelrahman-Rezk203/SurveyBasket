using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.API.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class DelColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Votes_PollId_Id",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_VoteAnswers_QestionId_VoteId",
                table: "VoteAnswers");

            migrationBuilder.DropIndex(
                name: "IX_VoteAnswers_QuestionId",
                table: "VoteAnswers");

            migrationBuilder.DropColumn(
                name: "QestionId",
                table: "VoteAnswers");

            migrationBuilder.CreateIndex(
                name: "IX_VoteAnswers_QuestionId_VoteId",
                table: "VoteAnswers",
                columns: new[] { "QuestionId", "VoteId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VoteAnswers_QuestionId_VoteId",
                table: "VoteAnswers");

            migrationBuilder.AddColumn<int>(
                name: "QestionId",
                table: "VoteAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PollId_Id",
                table: "Votes",
                columns: new[] { "PollId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_VoteAnswers_QestionId_VoteId",
                table: "VoteAnswers",
                columns: new[] { "QestionId", "VoteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoteAnswers_QuestionId",
                table: "VoteAnswers",
                column: "QuestionId");
        }
    }
}
