using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static void Initialize(DbContext context)
    {
        context.Database.Migrate();

        if (!context.Set<Employee>().Any())
        {
            var employees = new List<Employee>
            {
                new()
                {
                    FirstName = "Rob",
                    LastName = "Truman",
                    Role = "Nurse"
                },
                new()
                {
                    FirstName = "Halina",
                    LastName = "Ostapchuk",
                    Role = "Doctor"
                }
            };

            context.Set<Employee>().AddRange(employees);
            context.SaveChanges();
        }

        if (!context.Set<Shift>().Any())
        {
            var shiftRecords = new List<Shift>
            {
                new()
                {
                    EmployeeId = 1,
                    StartTime = new DateTime(2026, 9, 16, 8, 0, 0),
                    EndTime = new DateTime(2026, 9, 16, 16, 0, 0)
                },
                new()
                {
                    EmployeeId = 2,
                    StartTime = new DateTime(2026, 9, 17, 12, 0, 0),
                    EndTime = new DateTime(2026, 9, 17, 20, 0, 0)
                }
            };

            context.Set<Shift>().AddRange(shiftRecords);
            context.SaveChanges();
        }
    }
}