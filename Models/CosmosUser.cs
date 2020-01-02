using Microsoft.AspNetCore.Identity;
using System;

namespace Cosmos.Sts.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class CosmosUser : IdentityUser<Guid>
    {
    }
}
