using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IndentityServerEcossistema.IS4.InMemory
{
    public class ProfileService : IProfileService
    {        
        //protected UserManager<TestUser> _userManager;

        //public ProfileService(UserManager<TestUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //IMPORTANTE: CONCLUSOES
            /*
                Para que esta classe seja "ativada" é necessário adicionar a injecao de dependencia
                na classe Startup.cs no metodo "ConfigureServers como a seguir:                    
                        services.AddScoped<IProfileService, ProfileService>();

                É necessário utilizar esta classe para seja possível adicionar(embutir) as Claims do usuário
                no conteúdo do AccesToken. Assim é possível passar o AccessToken para 
                as APIs (nao mais o id_token como é feito hoje no SIOP).
                
                Interessante também que neste metodo é necessário buscar os dados do usuário em repositório.
                Neste caso de testes com Utilização de Usuarios em memória fiz a obtenção diretamente da classe
                TestUsers. Mas no contexto real normalmente é utilizado o UserManager que é recebido via DI 
                (vide construtor e variaveis comentadas acima).

                Na region abaixo deixo o conteudo do que imagino que seria o correto (ainda nao testei)
            */

            #region conteudo que imagino ser o correto

            /*
            public class IdentityProfileService : IProfileService
    {

        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, UserManager<ApplicationUser> userManager)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new ArgumentException("");
            }

            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();

            //Add more claims like this
            //claims.Add(new System.Security.Claims.Claim("MyProfileID", user.Id));

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }

            */

            #endregion conteudo que imagino ser o correto




            string sub = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

            if(sub != null)
            {
                var usuario = TestUsers.Users.FirstOrDefault(x => x.SubjectId == sub);
    
                var userClaims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName, usuario.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value),
                    new Claim(JwtClaimTypes.FamilyName, usuario.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value),
                    new Claim(JwtClaimTypes.Email, usuario.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value),
                    new Claim(JwtClaimTypes.Role, usuario.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Role)?.Value),              
                };

                context.IssuedClaims.AddRange(userClaims);



            } 
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }
}
