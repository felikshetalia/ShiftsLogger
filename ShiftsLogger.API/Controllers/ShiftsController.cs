using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly IShiftsService _shiftsService;

    public ShiftsController(IShiftsService shiftsService)
    {
        _shiftsService = shiftsService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<ShiftResponse>>> GetAll()
    {
        var shifts = await _shiftsService.GetAllAsync();
        return Ok(shifts.Select(ToResponse).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ShiftResponse>> GetById(int id)
    {
        var shift = await _shiftsService.GetByIdAsync(id);
        return shift is null ? NotFound() : Ok(ToResponse(shift));
    }

    [HttpPost]
    public async Task<ActionResult<ShiftResponse>> Create(CreateShiftDTO dto)
    {
        var shift = new Shift
        {
            EmployeeId = dto.EmployeeID,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        var createdShift = await _shiftsService.CreateAsync(shift);
        if (createdShift is null)
            return BadRequest("The employee does not exist or is not active.");

        var response = ToResponse(createdShift);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ShiftResponse>> Update(int id, UpdateShiftDTO dto)
    {
        var shift = new Shift
        {
            EmployeeId = dto.EmployeeID,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        var updatedShift = await _shiftsService.UpdateAsync(id, shift);
        return updatedShift is null ? NotFound() : Ok(ToResponse(updatedShift));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _shiftsService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private static ShiftResponse ToResponse(Shift shift)
        => new(shift.Id, shift.EmployeeId, shift.StartTime, shift.EndTime);
}
