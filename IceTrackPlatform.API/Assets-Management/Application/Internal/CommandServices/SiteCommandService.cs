using IceTrackPlatform.API.Assets_Management.Domain.Model.Aggregates;
using IceTrackPlatform.API.Assets_Management.Domain.Model.Commands;
using IceTrackPlatform.API.Assets_Management.Domain.Repositories;
using IceTrackPlatform.API.Assets_Management.Domain.Services;
using IceTrackPlatform.API.Shared.Domain.Repositories;

namespace IceTrackPlatform.API.Assets_Management.Application.Internal.CommandServices;

/// <summary>
/// This class handles commands related to Site entities.
/// </summary>
/// <param name="siteRepository">The instance of SiteRepository</param>
/// <param name="unitOfWork">The instance of UnitOfwork</param>
public class SiteCommandService(ISiteRepository siteRepository, 
                                    IUnitOfWork unitOfWork)
    : ISiteCommandService
{
    private void ValidateSite(string name, string address, string contactName, string phone)
    {
        if (name != name.Trim())
            throw new Exception("Name cannot be empty or with spaces");

        if (address != address.Trim())
            throw new Exception("Address cannot be empty or with spaces");

        if (contactName != contactName.Trim())
            throw new Exception("ContactName cannot be empty or with spaces");

        if (string.IsNullOrWhiteSpace(phone) || phone.Contains(" ") || phone != phone.Trim())
            throw new Exception("Phone cannot be empty or with spaces");
        
        if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\d{9}$"))
            throw new Exception("Phone must contain exactly 9 digits");
    }
    
    public async Task<Site?> Handle(CreateSiteCommand command)
    {
        ValidateSite(command.Name, command.Address, command.ContactName, command.Phone);
        
        if (await siteRepository.ExistsByNameAsync(command.Name))
            throw new Exception("A site with the same Name already exists");

        if (await siteRepository.ExistsByAddressAsync(command.Address))
            throw new Exception("A site with the same Address already exists");

        if (await siteRepository.ExistsByPhoneAsync(command.Phone))
            throw new Exception("A site with the same Phone already exists");
        
        var site = new Site(command);
        try
        {
            await siteRepository.AddAsync(site);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            return null;
        }
        return site;
    }
    
    public async Task<bool> Handle(DeleteSiteCommand command)
    {
        var site = await siteRepository.FindByIdAsync(command.SiteId);
        if (site is null) return false;
        try
        {
            siteRepository.Remove(site);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error deleting Site: {e.Message}");
            return false;
        }
    }

    public async Task<Site?> Handle(UpdateSiteCommand command)
    {
        ValidateSite(command.Name, command.Address, command.ContactName, command.Phone);
        
        if (await siteRepository.ExistsByNameAsync(command.Name, command.SiteId))
            throw new Exception("A site with the same Name already exists");

        if (await siteRepository.ExistsByAddressAsync(command.Address, command.SiteId))
            throw new Exception("A site with the same Address already exists");

        if (await siteRepository.ExistsByPhoneAsync(command.Phone, command.SiteId))
            throw new Exception("A site with the same Phone already exists");
        
        var site = await siteRepository.FindByIdAsync(command.SiteId);
        if (site is null) return null;

        site.UpdateInformation(
            command.Name,
            command.Address,
            command.ContactName,
            command.Phone
        );

        try
        {
            siteRepository.Update(site);
            await unitOfWork.CompleteAsync();
            return site;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error updating Site: {e.Message}");
            return null;
        }
    }
}