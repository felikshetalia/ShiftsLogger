
public class EmployeeService : IEmployeeService
{
    private readonly ShiftsLoggerDbContext _shiftsCtx;

    public EmployeeService(ShiftsLoggerDbContext ctx)
    {
        _shiftsCtx = ctx;
    }

    public Employee CreateAsync(Employee dto)
    {
        var saved = _shiftsCtx.Employees.Add(dto);
        _shiftsCtx.SaveChanges();
        return saved.Entity;
    }

    public void DeleteAsync(int id)
    {
        var saved = _shiftsCtx.Employees.Find(id);
        if (saved == null)
            return;

        _shiftsCtx.Employees.Remove(saved);
        _shiftsCtx.SaveChanges();
    }

    public ICollection<Employee> GetAllAsync()
        => _shiftsCtx.Employees.ToList();

    public Employee? GetByIdAsync(int id)
        => _shiftsCtx.Employees.Find(id);

    public Employee UpdateAsync(int id, Employee dto)
    {
        var saved = _shiftsCtx.Employees.Find(id);
        if (saved == null)
            return null!;

        _shiftsCtx.Entry(saved).CurrentValues.SetValues(dto);
        _shiftsCtx.SaveChanges();
        return saved;
    }
}