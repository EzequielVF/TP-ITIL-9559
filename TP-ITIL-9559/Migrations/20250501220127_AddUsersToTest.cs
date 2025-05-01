using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_ITIL_9559.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersToTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO public.""User"" (""Id"", ""Email"", ""Password"")
                VALUES (0, 'ezequie@fiuba.com', '1234');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO public.""User"" (""Id"", ""Email"", ""Password"")
                VALUES (1, 'alex@fiuba.com', '1234');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO public.""User"" (""Id"", ""Email"", ""Password"")
                VALUES (2, 'santi@fiuba.com', '1234');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM public.""User""
                WHERE ""Id"" = 0;
            ");

            migrationBuilder.Sql(@"
                DELETE FROM public.""User""
                WHERE ""Id"" = 1;
            ");

            migrationBuilder.Sql(@"
                DELETE FROM public.""User""
                WHERE ""Id"" = 2;
            ");
        }
    }
}
