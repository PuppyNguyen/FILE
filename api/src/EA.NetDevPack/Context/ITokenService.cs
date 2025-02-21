﻿ 
namespace EA.NetDevPack.Context
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, UserClaims user);
        //string GenerateJSONWebToken(string key, string issuer, UserDTO user);
        bool IsTokenValid(string key, string issuer, string token);
    }
}
