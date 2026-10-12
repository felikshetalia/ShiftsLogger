
using Microsoft.EntityFrameworkCore;

public class ShiftsService : IShiftsService
{
    private readonly ShiftsLoggerDbContext _shiftsCtx;
    public ShiftsService(ShiftsLoggerDbContext ctx)
    {
        _shiftsCtx = ctx;
    }
    public async Task<Shift?> CreateAsync(Shift dto)
    {
        var employee = await _shiftsCtx.Employees.FindAsync(dto.EmployeeId);

        if (employee == null || !employee.isActive)
        {
            // reject operation
            return null;
        }
        var saved = _shiftsCtx.Shifts.Add(dto);
        await _shiftsCtx.SaveChangesAsync();
        return saved.Entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var saved = await _shiftsCtx.Shifts.FindAsync(id);
        if (saved == null)
        {
            // throw new NotFound
            return false;
        }

        _shiftsCtx.Shifts.Remove(saved);
        await _shiftsCtx.SaveChangesAsync();
        return true;
    }

    public async Task<ICollection<Shift>> GetAllAsync()
        => await _shiftsCtx.Shifts.ToListAsync();

    public async Task<Shift?> GetByIdAsync(int id)
        => await _shiftsCtx.Shifts.FindAsync(id);

    public async Task<Shift?> UpdateAsync(int id, Shift newData)
    {
        var saved = await _shiftsCtx.Shifts.FindAsync(id);
        if (saved == null)
            return null;

        var employee = await _shiftsCtx.Employees.FindAsync(newData.EmployeeId);

        if (employee == null || !employee.isActive)
            return null;

        // _shiftsCtx.Entry(saved).CurrentValues.SetValues(dto);
        saved.EmployeeId = newData.EmployeeId;
        saved.StartTime = newData.StartTime;
        saved.EndTime = newData.EndTime;
        await _shiftsCtx.SaveChangesAsync();

        return saved;
    }
}