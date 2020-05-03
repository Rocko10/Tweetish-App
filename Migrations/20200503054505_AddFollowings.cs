using Microsoft.EntityFrameworkCore.Migrations;

namespace TweetishApp.Migrations
{
    public partial class AddFollowings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FolloweeModel",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolloweeModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FollowerModel",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowerModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "followings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FollowerId = table.Column<string>(nullable: true),
                    FolloweeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_followings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_followings_FolloweeModel_FolloweeId",
                        column: x => x.FolloweeId,
                        principalTable: "FolloweeModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_followings_FollowerModel_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "FollowerModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_followings_FolloweeId",
                table: "followings",
                column: "FolloweeId");

            migrationBuilder.CreateIndex(
                name: "IX_followings_FollowerId",
                table: "followings",
                column: "FollowerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "followings");

            migrationBuilder.DropTable(
                name: "FolloweeModel");

            migrationBuilder.DropTable(
                name: "FollowerModel");
        }
    }
}
