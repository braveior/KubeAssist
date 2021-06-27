using Braveior.KubeAssist.Services;
using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Braveior.KubeAssist.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //Adds the dependency injection for the LoginService and Singleton instance for HttpClient
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddHttpClient<IKubernetesService, KubernetesService>();

            //Adds the Mud Blazor support to Blazor
            builder.Services.AddMudServices();
            //Adds the Fluxor State Management support to Blazor
            builder.Services.AddFluxor(o => o
            .ScanAssemblies(typeof(Program).Assembly).UseRouting());
            await builder.Build().RunAsync();
        }
    }
}
