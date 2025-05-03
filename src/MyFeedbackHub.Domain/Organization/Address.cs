namespace MyFeedbackHub.Domain.Organization;

public sealed class Address
{
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public string? ZipCode { get; private set; }
    public string? State { get; private set; }
    public string? StreetLine1 { get; private set; }
    public string? StreetLine2 { get; private set; }

    public Address()
    { }

    public void Update(
        string country,
        string city,
        string zipCode,
        string state,
        string streetLine1,
        string streetLine2)
    {
        Country = country;
        City = city;
        ZipCode = zipCode;
        State = state;
        StreetLine1 = streetLine1;
        StreetLine2 = streetLine2;
    }
}
