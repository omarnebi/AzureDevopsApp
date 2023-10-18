using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace AzureDevopsApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersInfoController : ControllerBase
    {
        private readonly string azureDevOpsBaseUrl = "https://dev.azure.com/omarnabi0452/"; // Replace with your organization URL
        private readonly string personalAccessToken = "y3mgebgabyaileyoafd7swklbo6j3ngmjbmgla457rm2kvh3v27q"; // Replace with your PAT
        private readonly string azureDevOpsurlsecurite = "https://vssps.dev.azure.com/omarnabi0452/";
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(azureDevOpsurlsecurite);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    var response = await httpClient.GetAsync("_apis/graph/users?api-version=7.2-preview.1");

                    if (response.IsSuccessStatusCode)
                    {
                        var projectsJson = await response.Content.ReadAsStringAsync();


                        return Ok(projectsJson);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
