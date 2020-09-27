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

    public class AuthStatusController : ControllerBase
    {
        private readonly ILogger<AuthStatusController> _logger;
        public AuthStatusController(ILogger<AuthStatusController> logger)
        {
            _logger = logger;
        }
       
        [HttpGet]
        [Route("test")]
        public async Task GetTestAsync()
        {
            // simply here to generate traffic and get the headers back
        }
        [Authorize]
        [HttpGet]
        [Route("ping")]
        public async Task GetPingAsync()
        {
            // simply here to generate traffic and get the headers back
        } 
    }
}
