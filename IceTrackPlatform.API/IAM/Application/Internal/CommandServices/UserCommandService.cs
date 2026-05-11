using IceTrackPlatform.API.IAM.Application.Internal.OutboundServices;
using IceTrackPlatform.API.IAM.Domain.Model.Aggregates;
using IceTrackPlatform.API.IAM.Domain.Model.Commands;
using IceTrackPlatform.API.IAM.Domain.Model.ValueObjects;
using IceTrackPlatform.API.IAM.Domain.Repositories;
using IceTrackPlatform.API.IAM.Domain.Services;
using IceTrackPlatform.API.Shared.Domain.Repositories;

namespace IceTrackPlatform.API.IAM.Application.Internal.CommandServices;

/// <summary>
///     Handles user-related commands such as sign-in and sign-up.
/// </summary>
public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork)
    : IUserCommandService
{
    /// <summary>
    ///     Authenticate a user using the provided credentials.
    /// </summary>
    /// <param name="command">The sign-in command containing username and password.</param>
    /// <returns>A tuple with the authenticated <see cref="User" /> and the generated JWT token.</returns>
    /// <exception cref="Exception">Thrown when credentials are invalid.</exception>
    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByUsernameAsync(command.Username);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");

        var token = tokenService.GenerateToken(user);

        return (user, token);
    }
    
    /// <summary>
    ///     Create a new user account.
    /// </summary>
    /// <param name="command">The sign-up command with username, password, and role.</param> 
    /// <returns>A completed <see cref="Task" /> when the operation succeeds.</returns>
    /// <exception cref="Exception">Thrown when the username is already taken or creation fails.</exception>
    public async Task Handle(SignUpCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Username))
            throw new Exception("Username cannot be empty");
        
        if (string.IsNullOrWhiteSpace(command.Password))
            throw new Exception("Password cannot be empty");
        
        if (userRepository.ExistsByUsername(command.Username))
            throw new Exception($"Username {command.Username} is already taken");
        
        if (userRepository.ExistsByUsername(command.Username))
            throw new Exception($"Username {command.Username} is already taken");
        
        if (string.IsNullOrWhiteSpace(command.Username))
            throw new Exception("Username cannot be empty or contain only spaces");
        
        if (command.Username.Contains(' '))
            throw new Exception("Username cannot contain spaces");

        if (command.Username != command.Username.Trim())
            throw new Exception("Username cannot start or end with spaces");

        PasswordPolicyValidator.Validate(command.Password);
        
        var hashedPassword = hashingService.HashPassword(command.Password);
        
        var user = new User(command.Username, hashedPassword, command.Role); 
        try
        {
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while creating user: {e.Message}");
        }
    }
}