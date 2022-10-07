using MoneyRemittance.Domain.Transactions;

namespace MoneyRemittance.TestHelpers.Domain;

public class AddressInfoBuilder
{
    private string _phone = Guid.NewGuid().ToString()[30..];
    private string _address = Guid.NewGuid().ToString()[30..];
    private string _country = Guid.NewGuid().ToString()[30..];
    private string _city = Guid.NewGuid().ToString()[30..];
    private string _postalCode = Guid.NewGuid().ToString()[30..];
    private string _state = Guid.NewGuid().ToString()[30..];

    public AddressInfo Build()
    {
        return AddressInfo.Of(_phone, _address, _country, _city, _postalCode, _state);
    }

    public AddressInfoBuilder SetPhone(string phone)
    {
        _phone = phone;
        return this;
    }

    public AddressInfoBuilder SetAddress(string address)
    {
        _address = address;
        return this;
    }

    public AddressInfoBuilder SetCountry(string country)
    {
        _country = country;
        return this;
    }

    public AddressInfoBuilder SetCity(string city)
    {
        _city = city;
        return this;
    }

    public AddressInfoBuilder SetPostalCode(string postalCode)
    {
        _postalCode = postalCode;
        return this;
    }

    public AddressInfoBuilder SetState(string state)
    {
        _state = state;
        return this;
    }
}