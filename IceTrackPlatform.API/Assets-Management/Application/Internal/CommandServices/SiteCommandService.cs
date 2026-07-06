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
/// <param name="logger">The logger instance</param>
public class SiteCommandService(ISiteRepository siteRepository, 
                                    IUnitOfWork unitOfWork,
                                    ILogger<SiteCommandService> logger)
    : ISiteCommandService
{
    private void ValidateSite(string name, string address, string contactName, string phone)
    {
        if (name != name.Trim())
        {
            logger.LogWarning("Validation failed: name '{Name}' has leading/trailing spaces", name);
            throw new Exception("Name cannot be empty or with spaces");
        }

        if (address != address.Trim())
        {
            logger.LogWarning("Validation failed: address '{Address}' has leading/trailing spaces", address);
            throw new Exception("Address cannot be empty or with spaces");
        }

        if (contactName != contactName.Trim())
        {
            logger.LogWarning("Validation failed: contactName '{ContactName}' has leading/trailing spaces", contactName);
            throw new Exception("ContactName cannot be empty or with spaces");
        }

        if (string.IsNullOrWhiteSpace(phone) || phone.Contains(" ") || phone != phone.Trim())
        {
            logger.LogWarning("Validation failed: phone '{Phone}' is empty or contains spaces", phone);
            throw new Exception("Phone cannot be empty or with spaces");
        }
        
        if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\d{9}$"))
        {
            logger.LogWarning("Validation failed: phone '{Phone}' does not match 9-digit pattern", phone);
            throw new Exception("Phone must contain exactly 9 digits");
        }
    }
    
    public async Task<Site?> Handle(CreateSiteCommand command)
    {
        logger.LogInformation("Creating site: Name='{Name}', Address='{Address}', ContactName='{ContactName}', Phone='{Phone}'",
            command.Name, command.Address, command.ContactName, command.Phone);

        ValidateSite(command.Name, command.Address, command.ContactName, command.Phone);
        
        if (await siteRepository.ExistsByNameAsync(command.Name))
        {
            logger.LogWarning("Duplicate name: '{Name}' already exists", command.Name);
            throw new Exception("A site with the same Name already exists");
        }

        if (await siteRepository.ExistsByAddressAsync(command.Address))
        {
            logger.LogWarning("Duplicate address: '{Address}' already exists", command.Address);
            throw new Exception("A site with the same Address already exists");
        }

        if (await siteRepository.ExistsByPhoneAsync(command.Phone))
        {
            logger.LogWarning("Duplicate phone: '{Phone}' already exists", command.Phone);
            throw new Exception("A site with the same Phone already exists");
        }
        
        var site = new Site(command);
        try
        {
            await siteRepository.AddAsync(site);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database error while saving site: Name='{Name}'", command.Name);
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
            site.SoftDelete();
            siteRepository.Update(site);
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