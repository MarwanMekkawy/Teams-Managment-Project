using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Security
{
    public interface IJwtTokenService
    {
        string CreateToken(User user);
        (bool IsValid, ClaimsPrincipal? Principal) ValidateToken(string token);
    }
}
