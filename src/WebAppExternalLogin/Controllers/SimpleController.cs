using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppExternalLogin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]

    public class SimpleController : ControllerBase
    {
        private readonly ILogger<SimpleController> _logger;
        public SimpleController(ILogger<SimpleController> logger)
        {
            _logger = logger;
        }
       
        [HttpGet]
        [Route("ping")]
        public async Task GetPingAsync()
        {
        } 
    }
}
