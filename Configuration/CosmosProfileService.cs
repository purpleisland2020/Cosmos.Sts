using Cosmos.Sts.Data;
using Cosmos.Sts.Models;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cosmos.Sts.Configuration
{
    public class CosmosProfileService : IProfileService
    {
        private readonly CosmosContext _context;
        private readonly UserManager<CosmosUser> _userManager;

        public CosmosProfileService(UserManager<CosmosUser> userManager, CosmosContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
            
            if (user == null)
            {
                throw new ArgumentException($"Could not find user with the given sub: {context.Subject.GetSubjectId()}");
            }

            var userData = _context.Users.FirstOrDefault(x => x.Id == user.Id);

            var claims = new Claim[]
             {
                 //Todo: Added claims -- In the below case we might not need since we already have userID
                  new Claim("username",userData?.UserName)
             };
            context.IssuedClaims = claims.ToList();
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
