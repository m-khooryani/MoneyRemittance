namespace MoneyRemittance.BuildingBlocks.Application.AggregateHistory;

public class AggregateRootHistoryItem
{
    public string Id { get; private set; }
    public string AggregateId { get; private set; }
    public int Version { get; private set; }
    public string EventType { get; private set; }
    public DateTime Datetime { get; private set; }
    public string Type { get; private set; }
    public string Data { get; private set; }

    private AggregateRootHistoryItem()
    {
    }

    private AggregateRootHistoryItem(
        string id,
        string aggregateId,
        int version,
        string eventType,
        DateTime dateTime,
        string type,
        string data)
        : this()
    {
        Id = id;
        AggregateId = aggregateId;
        Version = version;
        EventType = eventType;
        Datetime = dateTime;
        Type = type;
        Data = data;
    }

    public static AggregateRootHistoryItem Create(
        string aggregateId,
        int version,
        string eventType,
        DateTime dateTime,
        string type,
        string data)
    {
        return new AggregateRootHistoryItem(
            Guid.NewGuid().ToString(),
            aggregateId,
            version,
            eventType,
            dateTime,
            type,
            data);
    }
}
