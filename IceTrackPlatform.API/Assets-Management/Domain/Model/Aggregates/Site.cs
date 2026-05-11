using IceTrackPlatform.API.Assets_Management.Domain.Model.Commands;

namespace IceTrackPlatform.API.Assets_Management.Domain.Model.Aggregates;

public partial class Site : SiteAudit
{
    protected Site()
    {
        Name = string.Empty;
        Address = string.Empty;
        ContactName = string.Empty;
        Phone = string.Empty;
        CantEquipment = 0;
    }

    protected Site(string name, string address, string contactName, string phone)
    {
        Name = name;
        Address = address;
        ContactName = contactName;
        Phone = phone;
        CantEquipment = 0;
    }
    
    /// <summary>
    ///   Constructor for Site aggregate
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Address"></param>
    /// <param name="ContactName"></param>
    /// <param name="Phone"></param>
    public Site(CreateSiteCommand command)
    {
        Name = command.Name;
        Address = command.Address;
        ContactName = command.ContactName;
        Phone = command.Phone;
        CantEquipment = 0;
    }
    
    public void UpdateInformation(string name, string address, string contactName, string phone)
    {
        Name = name;
        Address = address;
        ContactName = contactName;
        Phone = phone;
    }
    
    public void IncrementCantEquipment()
    {
        CantEquipment++;
    }
    
    public void DecrementCantEquipment()
    {
        if (CantEquipment > 0) CantEquipment--;
    }
    
    public void SoftDelete()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }
    
    public int Id { get; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public string ContactName { get; private set; }
    public string Phone { get; private set; }
    
    public int CantEquipment { get; private set; }
}