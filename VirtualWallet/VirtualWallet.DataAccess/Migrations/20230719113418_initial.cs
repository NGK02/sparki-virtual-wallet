using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualWallet.DataAccess.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EUR = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    BGN = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    USD = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    GBP = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    CHF = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ProfilePicPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Wallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallet",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Blocked" },
                    { 2, "User" },
                    { 3, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Wallet",
                columns: new[] { "Id", "BGN", "CHF", "EUR", "GBP", "USD", "UserId" },
                values: new object[,]
                {
                    { 1, 0m, 0m, 0m, 0m, 0m, 1 },
                    { 2, 0m, 0m, 0m, 0m, 0m, 2 },
                    { 3, 0m, 0m, 0m, 0m, 0m, 3 },
                    { 4, 0m, 0m, 0m, 0m, 0m, 4 },
                    { 5, 0m, 0m, 0m, 0m, 0m, 5 },
                    { 6, 0m, 0m, 0m, 0m, 0m, 6 },
                    { 7, 0m, 0m, 0m, 0m, 0m, 7 },
                    { 8, 0m, 0m, 0m, 0m, 0m, 8 },
                    { 9, 0m, 0m, 0m, 0m, 0m, 9 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "ProfilePicPath", "RoleId", "Username", "WalletId" },
                values: new object[,]
                {
                    { 1, "gosho@gmail.com", "Georgi", "Georgiev", "MTIz", null, null, 2, "goshoXx123", 1 },
                    { 2, "Barekov@gmail.com", "Nikolai", "Barekov", "MTIz", null, null, 2, "BarekaXx123", 2 },
                    { 3, "Uzunkata@gmail.com", "Shtilian", "Uzunov", "MTIz", null, null, 2, "Uzunkata", 3 },
                    { 4, "Cvetan@gmail.com", "Vladislav", "Cvetanov", "MTIz", null, null, 2, "Cvete123", 4 },
                    { 5, "Kostev@gmail.com", "Kosta", "Kostev", "MTIz", null, null, 2, "BrainDamage123", 5 },
                    { 6, "Admin@gmail.com", "Admin", "Adminov", "MTIz", null, null, 3, "Admin", 6 },
                    { 7, "Andrea@gmail.com", "Andrea", "Paynera", "MTIz", null, null, 2, "TopAndreika", 7 },
                    { 8, "Emanuela@gmail.com", "Emanuela", "Paynera", "MTIz", null, null, 2, "TopEmanuelka", 8 },
                    { 9, "Katrin@gmail.com", "Katrin", "lilova", "MTIz", null, null, 2, "Katrin", 9 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_WalletId",
                table: "Users",
                column: "WalletId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Wallet");
        }
    }
}
