using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USERNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PASSWORD = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FIRSTNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LASTNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EMAIL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PHONE_NUMBER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EMPLOYEES_USERS",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TASKS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TITLE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DEADLINE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TASKS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TASKS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEES_X_TASKS",
                columns: table => new
                {
                    EMPLOYEE_ID = table.Column<int>(type: "int", nullable: false),
                    TASK_ID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEES_X_TASKS", x => new { x.TASK_ID, x.EMPLOYEE_ID });
                    table.ForeignKey(
                        name: "FK_EMPLOYEES_X_TASKS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__EMPLOYEES__EMPLO",
                        column: x => x.EMPLOYEE_ID,
                        principalTable: "EMPLOYEES",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__EMPLOYEES__PROJ",
                        column: x => x.TASK_ID,
                        principalTable: "TASKS",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EMPLOYEES_UserId",
                table: "EMPLOYEES",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ_EMAIL",
                table: "EMPLOYEES",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EMPLOYEES_X_TASKS_EMPLOYEE_ID",
                table: "EMPLOYEES_X_TASKS",
                column: "EMPLOYEE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_EMPLOYEES_X_TASKS_UserId",
                table: "EMPLOYEES_X_TASKS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TASKS_UserId",
                table: "TASKS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ_USERNAME",
                table: "USERS",
                column: "USERNAME",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMPLOYEES_X_TASKS");

            migrationBuilder.DropTable(
                name: "EMPLOYEES");

            migrationBuilder.DropTable(
                name: "TASKS");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
