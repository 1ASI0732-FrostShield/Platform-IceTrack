using IceTrackPlatform.API.Assets_Management.Domain.Repositories;
using IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;
using IceTrackPlatform.API.Monitoring.Domain.Model.Commands;
using IceTrackPlatform.API.Monitoring.Domain.Repositories;
using IceTrackPlatform.API.Monitoring.Domain.Services;
using IceTrackPlatform.API.Shared.Domain.Repositories;

namespace IceTrackPlatform.API.Monitoring.Application.Internal.CommandServices;

/// <summary>
/// This class handles commands related to Equipment entities.
/// </summary>
/// <param name="equipmentRepository">The instance of EquipmentRepository</param>
/// <param name="unitOfWork">The instance of UnitOfwork</param>
public class EquipmentCommandService(IEquipmentRepository equipmentRepository,
                                        ISiteRepository siteRepository,
                                        IUnitOfWork unitOfWork)
    : IEquipmentCommandService
{
    private static void ValidateEquipment(string model, string type, string serial, string name)
    {
        if (model != model.Trim())
            throw new Exception("Model cannot start or with spaces");

        if (type != type.Trim())
            throw new Exception("Type cannot start or with spaces");
        
        if (string.IsNullOrWhiteSpace(serial) || serial.Contains(" ") || serial != serial.Trim())
            throw new Exception("Serial cannot be empty or with spaces");

        if (name != name.Trim())
            throw new Exception("Name cannot start or with spaces");
    }
    
    public async Task<Equipment?> Handle(CreateEquipmentCommand command)
    {
        ValidateEquipment(command.Model, command.Type, command.Serial, command.Name);
        
        if (await equipmentRepository.ExistsBySerialAsync(command.Serial))
            throw new Exception("An Equipment with the same Serial already exists");
        
        var equipment = new Equipment(command);
        try
        {
            await equipmentRepository.AddAsync(equipment);
            
            var site = await siteRepository.FindByNameAsync(command.Name);
            if (site is not null)
            {
                site.IncrementCantEquipment();
                siteRepository.Update(site);
            }

            await unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating Equipment: {ex.Message}");
            return null;
        }
        return equipment;
    }
    
    public async Task<bool> Handle(DeleteEquipmentCommand command)
    {
        var equipment = await equipmentRepository.FindByIdAsync(command.EquipmentId);
        if (equipment is null) return false;

        try
        {
            var site = await siteRepository.FindByNameAsync(equipment.Name);
            if (site is not null)
            {
                site.DecrementCantEquipment();
                siteRepository.Update(site);
            }

            equipmentRepository.Remove(equipment);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error deleting Equipment: {e.Message}");
            return false;
        }
    }
    
    public async Task<Equipment?> Handle(UpdateEquipmentCommand command)
    {
        ValidateEquipment(command.Model, command.Type, command.Serial, command.Name);
        
        if (await equipmentRepository.ExistsBySerialAsync(command.Serial))
            throw new Exception("An Equipment with the same Serial already exists");
        
        var equipment = await equipmentRepository.FindByIdAsync(command.EquipmentId);
        
        if (equipment is null) return null;

        try
        {
            equipment.Update(command);
            equipmentRepository.Update(equipment);
            await unitOfWork.CompleteAsync();
            return equipment;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating Equipment: {ex.Message}");
            return null;
        }
    }
}