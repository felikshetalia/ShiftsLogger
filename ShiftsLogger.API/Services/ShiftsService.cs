
using Microsoft.EntityFrameworkCore;

public class ShiftsService : IShiftsService
{
    private readonly ShiftsLoggerDbContext _shiftsCtx;
    public ShiftsService(ShiftsLoggerDbContext ctx)
    {
        _shiftsCtx = ctx;
    }
    public async Task<Shift> CreateAsync(Shift dto)
    {
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

    public async Task<Shift> UpdateAsync(int id, Shift dto)
    {
        var saved = await _shiftsCtx.Shifts.FindAsync(id);
        if (saved == null)
            return null!;

        // _shiftsCtx.Entry(saved).CurrentValues.SetValues(dto);
        saved.EmployeeId = dto.EmployeeId;
        saved.StartTime = dto.StartTime;
        saved.EndTime = dto.EndTime;
        await _shiftsCtx.SaveChangesAsync();

        return saved;
    }
}