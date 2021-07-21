using IndentityServerEcossistema.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace IndentityServerEcossistema.API.Controllers
{

    [Route("doughnuts")]
    [Authorize]
    [ApiController]
    public class DoughnutsController : Controller
    {
        private readonly List<Doughnut> Doughnuts = new List<Doughnut>()
        {
            new Doughnut
            {
                Name = "Holey Moley",
                Filling = "None",
                Iced = true,
                Price = 1.99
            },
            new Doughnut
            {
                Name = "Berry Nice",
                Filling = "Raspberry",
                Iced = false,
                Price = 2.99
            },
            new Doughnut
            {
                Name = "Chip Off The Old Choc",
                Filling = "Chocolate",
                Iced = false,
                Price = 2.99
            },
        };

        [HttpGet]
        [Authorize(Roles = "Admin")]        
        public async Task<IActionResult> Get()
        {
            return Ok(Doughnuts);
        }


        [HttpGet]     
        [Route("endpoint-console")]
        public async Task<IActionResult> GetConsoleApplication()
        {
            if (!AutorizacaoUtil.EscopoPermitido(this.HttpContext, "console-cliente"))
                return Unauthorized("Escopo nao autorizado");                
            
            return Ok(Doughnuts);
        }
    }
}
