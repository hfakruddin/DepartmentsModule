using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskModules.Migrations
{
    public partial class AddStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetParentDepartments')
            BEGIN
                EXEC('
                    CREATE PROCEDURE [dbo].[GetParentDepartments]
	                @DepartmentID INT
                    As
                    Begin

                        Declare @ID int ;
                        Set @ID = @DepartmentID;

                        WITH DepartmentCTE AS
                        (
                             Select DepartmentId, DepartmentName, ParentDeptID, DepartmentLogo
                             From Department
                             Where DepartmentId = @ID
    
                             UNION ALL
    
                             Select Department.DepartmentId , Department.DepartmentName,
                                     Department.ParentDeptID, Department.DepartmentLogo
                             From Department
                             JOIN DepartmentCTE
                             ON Department.DepartmentId = DepartmentCTE.ParentDeptID
                        )

                        Select D1.DepartmentId, D1.DepartmentName, D1.ParentDeptID, ISNULL(D2.DepartmentName, ''No Parent Department'') as ParentDepartment, D1.DepartmentLogo
                        From DepartmentCTE D1
                        LEFT Join DepartmentCTE D2
                        ON D1.ParentDeptID = D2.DepartmentId

                    End')
            END");

            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetSubDepartments')
            BEGIN
                EXEC('
                        CREATE PROCEDURE [dbo].[GetSubDepartments]
	                    @DepartmentID INT
                        AS
                        BEGIN

                            DECLARE @ID int;
                            SET @ID = @DepartmentID;

                            WITH DepartmentCTE AS (
                                -- Anchor member definition
                                SELECT DepartmentId, DepartmentName, ParentDeptID, DepartmentLogo
                                FROM Department
                                WHERE DepartmentId = @ID
    
                                UNION ALL
    
                                -- Recursive member definition
                                SELECT d.DepartmentId, d.DepartmentName, d.ParentDeptID, d.DepartmentLogo
                                FROM Department d
                                INNER JOIN DepartmentCTE cte ON d.ParentDeptID = cte.DepartmentId
                            )

                            -- Statement that executes the CTE
                            SELECT D1.DepartmentId, D1.DepartmentName, D1.ParentDeptID , ISNULL(D2.DepartmentName, ''No Sub Department'') AS SubDepartment, D1.DepartmentLogo
                            FROM DepartmentCTE D1
                            LEFT JOIN Department D2 ON D1.DepartmentId = D2.ParentDeptID;

                        END')
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
