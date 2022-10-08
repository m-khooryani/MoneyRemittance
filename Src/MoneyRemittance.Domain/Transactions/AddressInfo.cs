using MoneyRemittance.BuildingBlocks.Domain;
using Newtonsoft.Json;

namespace MoneyRemittance.Domain.Transactions;

public class AddressInfo : ValueObject
{
    public string Phone { get; }
    public string Address { get; }
    public string Country { get; }
    public string City { get; }
    public string PostalCode { get; }
    public string State { get; }

    [JsonConstructor]
    private AddressInfo(
        string phone, 
        string address, 
        string country, 
        string city, 
        string postalCode, 
        string state)
    {
        Phone = phone;
        Address = address;
        Country = country;
        City = city;
        PostalCode = postalCode;
        State = state;
    }

    public static AddressInfo Of(
        string phone,
        string address,
        string country,
        string city,
        string postalCode,
        string state)
    {
        return new AddressInfo(phone, address, country, city, postalCode, state);
    }
}