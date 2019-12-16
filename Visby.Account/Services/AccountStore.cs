using IdentityServer4.Validation;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4;
using System.Collections.Generic;

namespace Karenia.Visby.Account.Services
{
    public class AccountStore : IResourceOwnerPasswordValidator
    {
        public AccountService _accountservice { get; set; }

        public AccountStore(AccountService accountService)
        {
            _accountservice = accountService;
        }


        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var result = await _accountservice.FindLoginInfo(context.UserName);
            if (result == null || result.EncryptedPassword != context.Password)
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant,
                    "Username and password do not match");
                return;
            }
            else
            {
                context.Result = new GrantValidationResult(
                    subject: result.Email,
                    authenticationMethod: "custom",
                    claims: new Claim[]
                    {
                        new Claim("Name", result.Email),
                        new Claim("Role", result.Type.ToString())
                    }
                );
            }
        }
    }
}