using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluffyBunny.BlazorCookieAuth.Services.Default
{
    class HostAccessor : IHostAccessor
    {
        public IWebAssemblyHostEnvironment HostEnvironment { get; internal set; }

        public Uri BaseAddress { get; internal set; }
    }
}
