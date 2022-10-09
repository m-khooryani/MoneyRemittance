namespace MoneyRemittance.BuildingBlocks.Extensions.Hosting;

public interface IJsonMessageResolver
{
    string Resolve(string messageText);
}
