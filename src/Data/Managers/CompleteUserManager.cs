using Iso.Data.Models.UserModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Iso.Data.Managers;

public class CompleteUserManager(
    IUserStore<User> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<User> passwordHasher,
    IEnumerable<IUserValidator<User>> userValidators,
    IEnumerable<IPasswordValidator<User>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<UserManager<User>> logger)
    : UserManager<User>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer,
        errors, services, logger)
{
    public override async Task<IdentityResult> CreateAsync(User user, string password)
    {
        if (string.IsNullOrWhiteSpace(user.UserName))
        {
            return IdentityResult.Failed(new IdentityError 
            {
                Code = "UserNameRequired",
                Description = "User name is required"
            });
        }

        if (string.IsNullOrWhiteSpace(user.Sso))
        {
            user.Sso = Guid.NewGuid().ToString();
        }
        
        return await base.CreateAsync(user, password);
    }
}