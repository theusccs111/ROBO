using EspecificacaoAnalise.Servico.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROBO.Dominio.Resource.Request;

namespace EspecificacaoAnalise.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoboController : ControllerBase
    {
        private readonly RoboService _roboService;
        public RoboController(RoboService roboService)
        {
            _roboService = roboService;
        }

        [HttpPost("Iniciar")]
        public IActionResult Iniciar()
        {
            var response = _roboService.Iniciar();
            return Ok(response);
        }

        [HttpGet("obter-robo")]
        [Authorize]
        public IActionResult ObterRobo()
        {
            var user = _roboService.ObterRobo();
            return Ok(user);
        }

        [HttpPut("Mover")]
        [Authorize]
        public IActionResult Mover(RoboRequest request)
        {
            var user = _roboService.Mover(request);
            return Ok(user);
        }
    }
}