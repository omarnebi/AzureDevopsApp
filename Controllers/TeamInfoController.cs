using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.Core.WebApi;
using Newtonsoft.Json;
using AzureDevopsApp.Models;
using AzureDevopsApp.Datas;
using System.Text;

namespace AzureDevopsApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeamInfoController : ControllerBase
    {
        private readonly string azureDevOpsBaseUrl = "https://dev.azure.com/omarnabi0452/"; // Replace with your organization URL
        private readonly string personalAccessToken = "y3mgebgabyaileyoafd7swklbo6j3ngmjbmgla457rm2kvh3v27q"; // Replace with your PAT
        private readonly string azureDevOpsurlsecurite = "https://vssps.dev.azure.com/omarnabi0452/";
  
        [HttpGet]
        public async Task<IActionResult> GetAllTeam()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(azureDevOpsBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    var response = await httpClient.GetAsync("_apis/teams?api-version=7.2-preview.3");

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
        public async Task<IActionResult> CreateteamAsync(Team team,string projectId)
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
                    var teamData = new
                    {
                        name=team.name,
                        description=team.description

                    };

                    var projectDataJson = JsonConvert.SerializeObject(teamData);

                    var content = new StringContent(projectDataJson, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync($"_apis/projects/{projectId}/teams?api-version=7.2-preview.3", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Le projet a été créé avec succès, vous pouvez traiter la réponse ici si nécessaire
                        return Ok("team créé avec succès.");
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
        [HttpPatch]
        public async Task<IActionResult> UpdateProjectAsync(string projectId, string teamId, string newName, string newDescription)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(azureDevOpsBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    // Créez un objet JSON pour représenter les données de mise à jour du projet
                    var updateData = new
                    {
                        name = newName, // Nouveau nom du projet
                        description = newDescription // Nouvelle description du projet
                                                     // Vous pouvez également inclure d'autres propriétés à mettre à jour ici si nécessaire
                    };

                    var updateDataJson = JsonConvert.SerializeObject(updateData);

                    var content = new StringContent(updateDataJson, Encoding.UTF8, "application/json");

                    var response = await httpClient.PatchAsync($"_apis/projects/{projectId}/teams/{teamId}?api-version=7.2-preview.3", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("team mis à jour avec succès.");
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
        /*
         [HttpGet("teamProject")]
         public async Task<IActionResult> GetTeamProject()
         {
             try
             {
                 using (var httpClient = new HttpClient())
                 {
                     httpClient.BaseAddress = new Uri(azureDevOpsBaseUrl);
                     httpClient.DefaultRequestHeaders.Accept.Clear();
                     httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                     httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                     var response = await httpClient.GetAsync("_apis/teams?api-version=7.2-preview.3");

                     if (response.IsSuccessStatusCode)
                     {
                         var projectsJson = await response.Content.ReadAsStringAsync();

                         var data = JsonConvert.DeserializeObject<TeamResponse>(projectsJson);
                         // Extract the names of projects and default team names
                         var teams = data.value;
                         var selectedProperties = teams.Select(team => new
                         {
                             Id = team.id,
                             Name = team.name,
                             ProjectName = team.projectName
                         });
                         return Ok(selectedProperties);
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
        */
        [HttpDelete]
        public async Task<IActionResult> DeleteTeamAsync(string projectId, string teamId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(azureDevOpsBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    var response = await httpClient.DeleteAsync($"_apis/projects/{projectId}/teams/{teamId}?api-version=7.2-preview.3");

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("team supprimé avec succès.");
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

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetTeamByIdAsync(string projectId, string teamId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(azureDevOpsBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    var response = await httpClient.GetAsync($"_apis/projects/{projectId}/teams/{teamId}?api-version=7.2-preview.3");

                    if (response.IsSuccessStatusCode)
                    {
                        var teamInfoJson = await response.Content.ReadAsStringAsync();
                        // Vous pouvez désérialiser les données JSON ici dans un objet ProjectInfo ou un autre type approprié
                        var teamInfo = JsonConvert.DeserializeObject<Models.Team>(teamInfoJson);
                        return Ok(teamInfo); // Retourne les détails du projet
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




