using EA.NetDevPack.Context;
using Microsoft.AspNetCore.Authentication;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace EA.Gateway.FILE.Handlers
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public TokenHandler(IConfiguration config, ITokenService tokenService, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _config = config;
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
           /* var downstreamRoute = _accessor.HttpContext.Items?.DownstreamRouteHolder()?.Route?.DownstreamRoute?.FirstOrDefault();
            if (downstreamRoute != null)
            {
                var key = downstreamRoute.Key;
            }*/
            var auth = request.Headers.Authorization; 
            var jwt = auth.Parameter;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            var sub = token.Payload.Sub; 

            var tokeclaims = token.Payload.Claims;
            var validUser = new UserClaims();
            validUser.Username = tokeclaims.Where(x => x.Type == "preferred_username").Select(x => x.Value).First();
            validUser.FullName = tokeclaims.Where(x => x.Type == "name").Select(x => x.Value).First();
            validUser.Tenant = tokeclaims.Where(x => x.Type == "tenant").Select(x => x.Value).First();
            validUser.Data_Zone = tokeclaims.Where(x => x.Type == "data_zone").Select(x => x.Value).First();
            validUser.Product = "pim";
            validUser.Email = tokeclaims.Where(x => x.Type == "email").Select(x => x.Value).First();
            validUser.Email_Verified = bool.Parse(tokeclaims.Where(x => x.Type == "email_verified").Select(x => x.Value).First());
            validUser.Sub = sub;
            validUser.Exp = token.Payload.Exp;
            var generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(),  validUser);
            //  request.Headers.Add("Authorization", "Bearer " + generatedToken); 
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", generatedToken);
            request.Headers.Remove("AccessToken");
            return await base.SendAsync(request, cancellationToken);
        }
       
    }
}
