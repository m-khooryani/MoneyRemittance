namespace MoneyRemittance.BuildingBlocks.Extensions.Hosting;

internal class PlainMessageJsonResolver : IJsonMessageResolver
{
    public string Resolve(string messageText)
    {
        return messageText;
    }
}
