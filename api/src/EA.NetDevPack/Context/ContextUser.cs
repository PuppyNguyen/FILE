using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace EA.NetDevPack.Context
{
    public class ContextUser : IContextUser
    {
        private readonly IHttpContextAccessor _accessor;

        public ContextUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;

            var headers = _accessor.HttpContext.Request.Headers;
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            foreach (var header in headers)
            {
                requestHeaders.Add(header.Key, header.Value);
            }
        }

        public string UserName => _accessor.HttpContext.User.Identity.Name;

        public UserClaims UserClaims {
            get
            {
                var result = new UserClaims();
                var claims = GetClaims();
                result.Sub= claims.First(x=>x.Type==ClaimTypes.NameIdentifier).Value;
                result.Email = claims.First(x => x.Type == ClaimTypes.Email).Value;
                result.Username = claims.First(x => x.Type == ClaimTypes.Name).Value;
                result.Tenant = claims.First(x => x.Type.Equals("tenant")).Value;
                result.Data_Zone = claims.First(x => x.Type.Equals("data_zone")).Value;
                result.FullName = claims.First(x => x.Type.Equals("fullname")).Value;
                return result;
            }
        }

        public string Tenant
        {
            get
            {
                var result = new UserClaims();
                var claims = GetClaims();
                return claims.First(x => x.Type.Equals("tenant")).Value;
            }
        }
        public string Product
        {
            get
            {
                var result = new UserClaims();
                var claims = GetClaims();
                return claims.First(x => x.Type.Equals("product")).Value;
            }
        }
        public Guid GetUserId()
        {
            return IsAutenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string GetUserEmail()
        {
            return IsAutenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public bool IsAutenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsInRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        public IEnumerable<Claim> GetClaims()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public HttpContext GetHttpContext()
        {
            return _accessor.HttpContext;
        }

         
    }
}