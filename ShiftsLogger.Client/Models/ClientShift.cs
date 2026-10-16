public sealed class ClientShift
{
    public int Id { get; init; }
    public int EmployeeId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
}

public sealed record ShiftWriteRequest(int EmployeeID, DateTime StartTime, DateTime EndTime);
