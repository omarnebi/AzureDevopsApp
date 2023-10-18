using AzureDevopsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;


namespace AzureDevopsApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        private readonly string azureDevOpsBaseUrl = "https://dev.azure.com/omarnabi0452/"; // Replace with your organization URL
        private readonly string personalAccessToken = "y3mgebgabyaileyoafd7swklbo6j3ngmjbmgla457rm2kvh3v27q"; // Replace with your PAT
        private readonly string azureDevOpsurlsecurite = "https://vssps.dev.azure.com/omarnabi0452/";
        private readonly AzureDevopsDBContext _context;
        [HttpGet]
        public async Task<IActionResult> GetAllWorkItemAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(azureDevOpsBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    var response = await httpClient.GetAsync("_apis/work/accountmyworkrecentactivity?api-version=7.1-preview.2");

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
        public async Task<IActionResult> CreateTask([FromBody] List<TaskModel> taskPatch)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    var jsonPatch = new JsonPatchDocument();

                    foreach (var taskOperation in taskPatch)
                    {
                        jsonPatch.Add(taskOperation.Path ,taskOperation.Value);
                    }

                    var jsonPatchContent = new StringContent(JsonConvert.SerializeObject(jsonPatch), Encoding.UTF8, "application/json-patch+json");
                    string uri = "https://dev.azure.com/omarnabi0452/testerobot/_apis/wit/workitems/$Task?api-version=5.1";
                    var response = await client.PatchAsync(uri, jsonPatchContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("Tâche créée avec succès.");
                    }
                    else
                    {
                        return BadRequest("La création de la tâche a échoué.");
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
