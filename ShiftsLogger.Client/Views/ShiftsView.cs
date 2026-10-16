using System.Globalization;
using Spectre.Console;

public sealed class ShiftsView
{
    private static readonly string[] DateTimeFormats =
    [
        "yyyy-MM-dd HH:mm",
        "yyyy-MM-ddTHH:mm",
        "yyyy-MM-ddTHH:mm:ss",
        "O"
    ];

    public void DisplayShifts(IReadOnlyCollection<ClientShift> shifts)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[green]Shifts[/]");

        if (shifts.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No shifts found.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("ID")
            .AddColumn("Employee ID")
            .AddColumn("Start time")
            .AddColumn("End time");

        foreach (var shift in shifts.OrderBy(shift => shift.Id))
        {
            table.AddRow(
                shift.Id.ToString(CultureInfo.InvariantCulture),
                shift.EmployeeId.ToString(CultureInfo.InvariantCulture),
                shift.StartTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                shift.EndTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
        }

        AnsiConsole.Write(table);
    }

    public ManagementMenuOption DisplayManagementMenu()
        => AnsiConsole.Prompt(
            new SelectionPrompt<ManagementMenuOption>()
                .Title("[green]Choose a shift action[/]")
                .AddChoices(Enum.GetValues<ManagementMenuOption>())
                .UseConverter(FormatMenuOption));

    public int PromptShiftId(string action)
        => AnsiConsole.Prompt(
            new TextPrompt<int>($"Enter the shift ID to {Markup.Escape(action)}:")
                .Validate(id => id > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error("The ID must be greater than zero.")));

    public ShiftWriteRequest PromptShiftDetails(ClientShift? currentShift = null)
    {
        if (currentShift is not null)
        {
            AnsiConsole.MarkupLine(
                "[grey]Current values are pre-filled. Press Enter to keep a value, or edit it.[/]");
        }

        var employeeId = currentShift is null
            ? PromptEmployeeId()
            : AnsiConsole.Prompt(
                new TextPrompt<int>("Enter the employee ID:")
                    .DefaultValue(currentShift.EmployeeId)
                    .EditableDefaultValue(true)
                    .Validate(id => id > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("The ID must be greater than zero.")));

        while (true)
        {
            var startTime = PromptDateTime(
                "Enter the shift start time (yyyy-MM-dd HH:mm):",
                currentShift?.StartTime);
            var endTime = PromptDateTime(
                "Enter the shift end time (yyyy-MM-dd HH:mm):",
                currentShift?.EndTime);

            if (startTime <= endTime)
                return new ShiftWriteRequest(employeeId, startTime, endTime);

            AnsiConsole.MarkupLine("[red]Start time must not be later than end time. Please enter both times again.[/]");
        }
    }

    private static int PromptEmployeeId()
        => AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the employee ID:")
                .Validate(id => id > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error("The ID must be greater than zero.")));

    public void DisplayMessage(string message)
        => AnsiConsole.MarkupLine(Markup.Escape(message));

    public void WaitForInput()
    {
        AnsiConsole.MarkupLine("\n[grey]Press any key to continue.[/]");
        AnsiConsole.Console.Input.ReadKey(true);
    }

    private static DateTime PromptDateTime(string prompt, DateTime? currentValue = null)
    {
        while (true)
        {
            var textPrompt = new TextPrompt<string>(prompt);
            if (currentValue.HasValue)
            {
                textPrompt
                    .DefaultValue(currentValue.Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture))
                    .EditableDefaultValue(true)
                    .AllowEmpty();
            }

            var input = AnsiConsole.Prompt(textPrompt);
            if (string.IsNullOrWhiteSpace(input) && currentValue.HasValue)
                return currentValue.Value;

            if (DateTime.TryParseExact(
                input,
                DateTimeFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind,
                out var value))
            {
                return value;
            }

            AnsiConsole.MarkupLine(
                "[red]Enter a valid date and time, for example 2026-10-14 09:30.[/]");
        }
    }

    private static string FormatMenuOption(ManagementMenuOption option)
        => option switch
        {
            ManagementMenuOption.Add => "Enter a new shift",
            ManagementMenuOption.Update => "Update an existing shift",
            ManagementMenuOption.Delete => "Delete a shift",
            ManagementMenuOption.Back => "Back",
            _ => option.ToString()
        };
}
