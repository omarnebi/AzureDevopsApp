using AzureDevopsApp.Datas;
using AzureDevopsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AzureDevopsApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SprintController : ControllerBase
    {
        private readonly string azureDevOpsBaseUrl = "https://dev.azure.com/omarnabi0452/"; // Replace with your organization URL
        private readonly string personalAccessToken = "y3mgebgabyaileyoafd7swklbo6j3ngmjbmgla457rm2kvh3v27q"; // Replace with your PAT
        private readonly string azureDevOpsurlsecurite = "https://vssps.dev.azure.com/omarnabi0452/";
        private readonly AzureDevopsDBContext _context;

        [HttpGet]
        public async Task<IActionResult> GetAllSprintAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(azureDevOpsBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    var response = await httpClient.GetAsync("testerobot/_apis/work/teamsettings/iterations?api-version=7.1-preview.1");

                    if (response.IsSuccessStatusCode)
                    {
                        var projectsJson = await response.Content.ReadAsStringAsync();


                        // Extract the names of projects and default team names


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

        [HttpPost]
        public async Task<IActionResult> CreateSprintAsync(Sprint sprint, string projectname)
        {

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(azureDevOpsBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    // Créez un objet JSON pour représenter les données du projet à insérer
                    var sprintData = new
                    {
                        name = sprint.Name,
                        attributes = new
                        {
                            startDate=sprint.StartDate,
                            finishDate=sprint.EndDate
                        }

                    };

                    var projectDataJson = JsonConvert.SerializeObject(sprintData);

                    var content = new StringContent(projectDataJson, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync($"{projectname}/_apis/work/teamsettings/iterations?api-version=7.1-preview.1", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Le projet a été créé avec succès, vous pouvez traiter la réponse ici si nécessaire
                        return Ok("sprint créé avec succès.");
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
