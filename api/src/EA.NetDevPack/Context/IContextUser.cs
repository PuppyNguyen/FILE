using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace EA.NetDevPack.Context
{
    public interface IContextUser
    {
        UserClaims UserClaims { get; } 
        string UserName { get; }
        string Tenant { get; }
        string Product { get; }
        Guid GetUserId();
        string GetUserEmail();
        bool IsAutenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaims();
        HttpContext GetHttpContext();
        
    }
}