using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualWallet.DataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

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
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Balances",
                columns: table => new
                {
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => new { x.CurrencyId, x.WalletId });
                    table.ForeignKey(
                        name: "FK_Balances_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Balances_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    ProfilePicPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                        name: "FK_Users_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "USD" },
                    { 2, "EUR" },
                    { 3, "BGN" },
                    { 4, "JPY" },
                    { 5, "CFH" }
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
                table: "Wallets",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "IsDeleted", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5709), null, false, 1 },
                    { 2, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5714), null, false, 2 },
                    { 3, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5717), null, false, 3 },
                    { 4, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5719), null, false, 4 },
                    { 5, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5721), null, false, 5 },
                    { 6, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5724), null, false, 6 },
                    { 7, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5726), null, false, 7 },
                    { 8, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5728), null, false, 8 },
                    { 9, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5730), null, false, 9 }
                });

            migrationBuilder.InsertData(
                table: "Balances",
                columns: new[] { "CurrencyId", "WalletId", "Amount" },
                values: new object[,]
                {
                    { 1, 1, 10000m },
                    { 1, 4, 10200m },
                    { 1, 6, 102004m },
                    { 2, 2, 10000m },
                    { 2, 5, 102400m },
                    { 3, 3, 10000m },
                    { 3, 7, 50200m },
                    { 4, 9, 102000000m },
                    { 5, 8, 1023230m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Email", "FirstName", "IsDeleted", "LastName", "Password", "PhoneNumber", "ProfilePicPath", "RoleId", "Username", "WalletId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5643), null, "gosho@gmail.com", "Georgi", false, "Georgiev", "MTIz", null, null, 2, "goshoXx123", 1 },
                    { 2, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5682), null, "Barekov@gmail.com", "Nikolai", false, "Barekov", "MTIz", null, null, 2, "BarekaXx123", 2 },
                    { 3, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5686), null, "Uzunkata@gmail.com", "Shtilian", false, "Uzunov", "MTIz", null, null, 2, "Uzunkata", 3 },
                    { 4, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5689), null, "Cvetan@gmail.com", "Vladislav", false, "Cvetanov", "MTIz", null, null, 2, "Cvete123", 4 },
                    { 5, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5691), null, "Kostev@gmail.com", "Kosta", false, "Kostev", "MTIz", null, null, 2, "BrainDamage123", 5 },
                    { 6, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5695), null, "Admin@gmail.com", "Admin", false, "Adminov", "MTIz", null, null, 3, "Admin", 6 },
                    { 7, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5698), null, "Andrea@gmail.com", "Andrea", false, "Paynera", "MTIz", null, null, 2, "TopAndreika", 7 },
                    { 8, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5701), null, "Emanuela@gmail.com", "Emanuela", false, "Paynera", "MTIz", null, null, 2, "TopEmanuelka", 8 },
                    { 9, new DateTime(2023, 7, 19, 15, 49, 23, 658, DateTimeKind.Local).AddTicks(5703), null, "Katrin@gmail.com", "Katrin", false, "lilova", "MTIz", null, null, 2, "Katrin", 9 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Balances_WalletId",
                table: "Balances",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CurrencyId",
                table: "Transactions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecipientId",
                table: "Transactions",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SenderId",
                table: "Transactions",
                column: "SenderId");

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
                name: "Balances");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}
