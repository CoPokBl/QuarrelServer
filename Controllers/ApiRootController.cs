using Microsoft.AspNetCore.Mvc;
using PisscordServer.Objects.ControllerClasses;

namespace PisscordServer.Controllers {
    
    [ApiController]
    [Route("/api")]
    public class ApiRootController : ApiController {

        [HttpGet]
        public IActionResult Get() {
            return Ok("Pisscord Server");
        }

    }
    
}