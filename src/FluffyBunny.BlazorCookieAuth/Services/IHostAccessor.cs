using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluffyBunny.BlazorCookieAuth.Services
{
    public interface IHostAccessor
    {
        public IWebAssemblyHostEnvironment HostEnvironment { get;   }
        public Uri BaseAddress { get;   }
    }
}
