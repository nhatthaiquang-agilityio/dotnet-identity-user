using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityUsers.Migrations
{
    public partial class UserConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserConnections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ConnectionId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConnections", x => x.Id);
                });

            // migrationBuilder.AlterColumn<string>(
            //     name: "Name",
            //     table: "AspNetUserTokens",
            //     maxLength: 128,
            //     nullable: false,
            //     oldClrType: typeof(string));

            // migrationBuilder.AlterColumn<string>(
            //     name: "LoginProvider",
            //     table: "AspNetUserTokens",
            //     maxLength: 128,
            //     nullable: false,
            //     oldClrType: typeof(string));

            // migrationBuilder.AlterColumn<string>(
            //     name: "ProviderKey",
            //     table: "AspNetUserLogins",
            //     maxLength: 128,
            //     nullable: false,
            //     oldClrType: typeof(string));

            // migrationBuilder.AlterColumn<string>(
            //     name: "LoginProvider",
            //     table: "AspNetUserLogins",
            //     maxLength: 128,
            //     nullable: false,
            //     oldClrType: typeof(string));


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserConnections");

            // migrationBuilder.AlterColumn<string>(
            //     name: "Name",
            //     table: "AspNetUserTokens",
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldMaxLength: 128);

            // migrationBuilder.AlterColumn<string>(
            //     name: "LoginProvider",
            //     table: "AspNetUserTokens",
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldMaxLength: 128);

            // migrationBuilder.AlterColumn<string>(
            //     name: "ProviderKey",
            //     table: "AspNetUserLogins",
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldMaxLength: 128);

            // migrationBuilder.AlterColumn<string>(
            //     name: "LoginProvider",
            //     table: "AspNetUserLogins",
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldMaxLength: 128);
        }
    }
}
