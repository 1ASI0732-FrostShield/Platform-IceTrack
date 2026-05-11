namespace IceTrackPlatform.API.Technicians.Domain.Model.Aggregates;

using IceTrackPlatform.API.Technicians.Domain.Model.ValueObjects;

public partial class Technician
{
    public int Id { get; }
    public string Name { get; private set; }
    public string Specialty { get; private set; }
    public string Phone { get; private set; }
    public ProviderId ProviderId { get; private set; }

    public Technician()
    {
        Name = string.Empty;
        Specialty = string.Empty;
        Phone = string.Empty;
        ProviderId = new ProviderId();
    }

    public Technician(string name, string specialty, string phone, int providerId)
    {
        Name = name;
        Specialty = specialty;
        Phone = phone;
        ProviderId = new ProviderId(providerId);
    }

    public void Update(string name, string specialty, string phone)
    {
        Name = name;
        Specialty = specialty;
        Phone = phone;
    }
    
    public void SoftDelete()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }
}
