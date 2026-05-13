namespace WorkshopAdmin.Shared.Helpers;

public static class BusinessTimeCalculator
{
    private static readonly TimeOnly WorkStart = new(9, 0);
    private static readonly TimeOnly LunchStart = new(13, 0);
    private static readonly TimeOnly LunchEnd = new(14, 0);
    private static readonly TimeOnly WorkEnd = new(18, 0);

    // Zona horaria del taller
    // Windows: "Central Standard Time (Mexico)"
    // Linux/Docker: "America/Mexico_City"
    private static readonly TimeZoneInfo WorkshopTimeZone =
        GetMexicoTimeZone();

    public static DateTimeOffset CalculateExpectedFinish(
        DateTimeOffset startUtc,
        TimeSpan estimated)
    {
        // Convertimos UTC -> hora local del taller
        var current = TimeZoneInfo.ConvertTime(
            startUtc,
            WorkshopTimeZone);

        var remaining = estimated;

        while (remaining > TimeSpan.Zero)
        {
            current = MoveToWorkingTime(current);

            var blockEnd = GetCurrentBlockEnd(current);

            var available = blockEnd - current;

            if (remaining <= available)
            {
                current = current.Add(remaining);

                // IMPORTANTE:
                // Regresar SIEMPRE UTC
                return current.ToUniversalTime();
            }

            remaining -= available;

            current = blockEnd.AddSeconds(1);
        }

        return current.ToUniversalTime();
    }

    public static TimeSpan CalculateRemaining(
        DateTimeOffset nowUtc,
        DateTimeOffset expectedFinishUtc)
    {
        if (nowUtc >= expectedFinishUtc)
            return TimeSpan.Zero;

        // UTC -> hora local del taller
        var current = TimeZoneInfo.ConvertTime(
            nowUtc,
            WorkshopTimeZone);

        var expectedFinish = TimeZoneInfo.ConvertTime(
            expectedFinishUtc,
            WorkshopTimeZone);

        var remaining = TimeSpan.Zero;

        while (current < expectedFinish)
        {
            current = MoveToWorkingTime(current);

            if (current >= expectedFinish)
                break;

            var blockEnd = GetCurrentBlockEnd(current);

            var end = blockEnd < expectedFinish
                ? blockEnd
                : expectedFinish;

            remaining += end - current;

            current = blockEnd.AddSeconds(1);
        }

        return remaining;
    }

    private static DateTimeOffset MoveToWorkingTime(DateTimeOffset date)
    {
        while (true)
        {
            // Fin de semana
            if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                date = date.Date
                    .AddDays(1)
                    .Add(WorkStart.ToTimeSpan());

                continue;
            }

            var time = TimeOnly.FromDateTime(date.DateTime);

            // Antes del horario laboral
            if (time < WorkStart)
            {
                return new DateTimeOffset(
                    date.Date.Add(WorkStart.ToTimeSpan()),
                    date.Offset);
            }

            // Hora de comida
            if (time >= LunchStart && time < LunchEnd)
            {
                return new DateTimeOffset(
                    date.Date.Add(LunchEnd.ToTimeSpan()),
                    date.Offset);
            }

            // Después del horario laboral
            if (time >= WorkEnd)
            {
                date = date.Date
                    .AddDays(1)
                    .Add(WorkStart.ToTimeSpan());

                continue;
            }

            return date;
        }
    }

    private static DateTimeOffset GetCurrentBlockEnd(
        DateTimeOffset current)
    {
        var time = TimeOnly.FromDateTime(current.DateTime);

        // Bloque mañana
        if (time < LunchStart)
        {
            return new DateTimeOffset(
                current.Date.Add(LunchStart.ToTimeSpan()),
                current.Offset);
        }

        // Bloque tarde
        return new DateTimeOffset(
            current.Date.Add(WorkEnd.ToTimeSpan()),
            current.Offset);
    }

    private static TimeZoneInfo GetMexicoTimeZone()
    {
        try
        {
            // Linux / Docker
            return TimeZoneInfo.FindSystemTimeZoneById(
                "America/Mexico_City");
        }
        catch
        {
            // Windows
            return TimeZoneInfo.FindSystemTimeZoneById(
                "Central Standard Time (Mexico)");
        }
    }
}