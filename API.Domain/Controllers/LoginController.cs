using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DATA.Domain.Models;
using DATA.Domain.Supervisor;
using DATA.Domain.Views;
using SECURITY.Domain.Global;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace API.Domain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private UserSupervisor _usr;
        private IConfiguration _cfg;

        public LoginController(AppDbContext context, IConfiguration Configuration)
        {
            _context = context;
            _cfg = Configuration;
            _usr = new UserSupervisor(_context);
        }
        [HttpPost]
        public async Task<ActionResult<object>> Login(  [FromBody]ViewLogin lgn,
                                                        [FromServices]SigningConfigurations signingConfigurations)
        {
            if (_usr.ValidUser(lgn))
                return Ok(new Token(_cfg, signingConfigurations).Criar2(_usr.Find(lgn.Usuario)));
            return Unauthorized();
        }
        [Authorize("Bearer")]
        [HttpPost("AlterarSenha/{id}")]
        public async Task<ActionResult<string>> AlterarSenha([FromBody]ViewSenha novaSenha, [FromRoute] int id)
        {
            //Neste ponto deveriamos bloquear a alteracao para usuarios diferentes do logado JWT
            try
            {
                _usr.UpdatePwd(id, novaSenha.NovaSenha);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Erro durante alteracao de senha");
            }
        }

        [Authorize("Bearer")]
        [HttpGet("Verifica")]
        public async Task<ActionResult<string>> Verifica()
        { 
            return Ok("Usuario autenticado");
        }
    }
}