using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebAppExternalLogin.Pages
{
    public class BlazorHostModel : PageModel
    {
        private readonly ILogger<BlazorHostModel> _logger;

        public BlazorHostModel(ILogger<BlazorHostModel> logger)
        {
            _logger = logger;
        }
        public string BlazorId;

        public void OnGet(string id)
        {
            BlazorId = id;
        }
    }
}
