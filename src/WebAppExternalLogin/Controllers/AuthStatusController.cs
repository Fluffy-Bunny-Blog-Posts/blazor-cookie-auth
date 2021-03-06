﻿using Microsoft.AspNetCore.Authorization;
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
       
        [Authorize]
        [HttpGet]
        [Route("check")]
        public async Task GetCheckAsync()
        {
        } 
    }
}
