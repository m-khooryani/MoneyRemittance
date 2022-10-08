namespace MoneyRemittance.BuildingBlocks.Infrastructure.RetryPolicy;

public class PollyConfig
{
    public TimeSpan[] SleepDurations { get; set; }
}
