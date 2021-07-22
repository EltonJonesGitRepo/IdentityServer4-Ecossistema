using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace MvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();            

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = "https://localhost:5001";

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";

                    options.SaveTokens = true;
                    
                    
                    //Mapeando a claim utilizada para autorizacao
                    options.TokenValidationParameters.NameClaimType = "role";


                    //Evento para casos de erro: 
                    //Ex.: Na tela do Login no IS4 quando o usuário não finaliza o Login clicando no botão cancelar.
                    //Isto ocasiona erro ao retornar para o Sistema WEB.
                    options.Events = new OpenIdConnectEvents
                    {
                        OnRemoteFailure = context => {
                            context.Response.Redirect("/");
                            context.HandleResponse();

                            return Task.FromResult(0);
                        }
                    };
                });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");                
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //Necessario para fazer o redirecionamento pós login de volta para a aplicacao Web
            app.UseAuthentication();
            
            app.UseRouting();


            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });






            //Faz com que qualquer requisicao a controllers seja automaticamente exigida autenticacao no IS4

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapDefaultControllerRoute()
            //        .RequireAuthorization();
            //});
        }
    }
}
