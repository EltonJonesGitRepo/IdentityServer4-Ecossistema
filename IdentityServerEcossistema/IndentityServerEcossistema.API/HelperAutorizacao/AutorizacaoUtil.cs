using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace IndentityServerEcossistema.API
{
    public static class AutorizacaoUtil
    {
        public static bool EscopoPermitido(HttpContext httpContext, string escopo)
        {
            var header = httpContext.Request.Headers["Authorization"];
            string stringValues = header[0].ToString().Replace("Bearer", string.Empty).Trim();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(stringValues) as JwtSecurityToken;

            List<string> escopeRecebidos = token.Claims.Where(x => x.Type == "scope").Select(x => x.Value).ToList();

            return escopeRecebidos.Any(x => x == escopo);
        }
    }
}
