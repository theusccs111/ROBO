using EspecificacaoAnalise.Servico.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROBO.Dominio.Resource.Request;

namespace EspecificacaoAnalise.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoboController : ControllerBase
    {
        private readonly RoboService _roboService;
        public RoboController(RoboService roboService)
        {
            _roboService = roboService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var resultado = _roboService.Get();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public IActionResult GetById(int Id)
        {
            var resultado = _roboService.GetById(Id);
            return Ok(resultado);
        }

        [HttpPost]
        public IActionResult Post(RoboRequest request)
        {
            var dados = _roboService.Add(request);
            return Ok(dados);
        }

        [HttpPost("Many")]
        public IActionResult PostMany(RoboRequest[] request)
        {
            var dados = _roboService.AddMany(request);
            return Ok(dados);
        }

        [HttpPut]
        public IActionResult Put(RoboRequest request)
        {
            var dados = _roboService.Update(request);
            return Ok(dados);
        }

        [HttpPut("Many")]
        public IActionResult PutMany(RoboRequest[] request)
        {
            var dados = _roboService.UpdateMany(request);
            return Ok(dados);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] RoboRequest request)
        {
            var dados = _roboService.Delete(request);
            return Ok(dados);
        }

        [HttpDelete("Many")]
        public IActionResult DeleteMany([FromBody] RoboRequest[] request)
        {
            var dados = _roboService.DeleteMany(request);
            return Ok(dados);
        }
    }
}