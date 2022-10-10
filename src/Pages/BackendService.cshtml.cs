using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace dotnet_helloworld.Pages
{
    public class BackendServiceModel : PageModel
    {
        private readonly ILogger<BackendServiceModel> _logger;
        [BindProperty] public string ApiResponse { get; set; }

        private IConfiguration _configuration;

        public BackendServiceModel(ILogger<BackendServiceModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
            string backendServiceUrl = "";
            // flag to check if running locally in a container (not in k8s)
            bool standAloneContainer = true;
            //var appConfig = _configuration.GetSection("EnvironmentConfig").Get<EnvironmentConfig>();
            var backendConfig = _configuration["AppConfig:BackendServiceUrl"];
            // if running locally in a container which is not in k8s & no configmaps configured
            if (backendConfig != null)
            {
                backendServiceUrl = backendConfig;
                standAloneContainer = false;
            }

            // If running in kubernetes
            if (standAloneContainer == false)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = httpClient.GetAsync(backendServiceUrl))
                    {
                        try
                        {
                            ViewData["BackendUrl"] = backendServiceUrl;
                            ApiResponse = response.Result.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception ex)
                        {
                            ApiResponse = ex.Message;
                        }
                    }
                }
            }
        }
    }
}
