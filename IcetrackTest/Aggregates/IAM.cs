using IceTrackPlatform.API.IAM.Domain.Model.Aggregates;
using IceTrackPlatform.API.IAM.Domain.Model.ValueObjects;

namespace IceTrackPlatform.Tests.Aggregates;

[TestClass]
public class IAM
{
    [TestMethod]
    public void Constructor_WithValidData_ShouldCreateUser()
    {
        var user = new User(
            username: "owner@test.com",
            passwordHash: "hashed-password",
            role: Roles.Owner
        );

        Assert.AreEqual("owner@test.com", user.Username);
        Assert.AreEqual("hashed-password", user.PasswordHash);
        Assert.AreEqual(Roles.Owner, user.Role);
    }

    [TestMethod]
    public void EmptyConstructor_WhenCalled_ShouldCreateDefaultOwnerUser()
    {
        var user = new User();

        Assert.AreEqual(string.Empty, user.Username);
        Assert.AreEqual(string.Empty, user.PasswordHash);
        Assert.AreEqual(Roles.Owner, user.Role);
    }

    [TestMethod]
    public void UpdateUsername_WithNewUsername_ShouldChangeUsername()
    {
        var user = new User("old@test.com", "hashed-password", Roles.Owner);

        user.UpdateUsername("new@test.com");

        Assert.AreEqual("new@test.com", user.Username);
    }

    [TestMethod]
    public void UpdateUsername_WhenCalled_ShouldReturnSameUserInstance()
    {
        var user = new User("old@test.com", "hashed-password", Roles.Owner);

        var updatedUser = user.UpdateUsername("new@test.com");

        Assert.AreSame(user, updatedUser);
    }

    [TestMethod]
    public void UpdatePasswordHash_WithNewPasswordHash_ShouldChangePasswordHash()
    {
        var user = new User("owner@test.com", "old-hash", Roles.Provider);

        user.UpdatePasswordHash("new-hash");

        Assert.AreEqual("new-hash", user.PasswordHash);
    }

    [TestMethod]
    public void UpdatePasswordHash_WhenCalled_ShouldReturnSameUserInstance()
    {
        var user = new User("owner@test.com", "old-hash", Roles.Provider);

        var updatedUser = user.UpdatePasswordHash("new-hash");

        Assert.AreSame(user, updatedUser);
    }
}