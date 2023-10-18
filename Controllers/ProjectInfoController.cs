using AzureDevopsApp.Datas;
using AzureDevopsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AzureDevopsApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectInfoController : ControllerBase
    {
        private readonly string azureDevOpsBaseUrl = "https://dev.azure.com/omarnabi0452/"; // Replace with your organization URL
        private readonly string personalAccessToken = "y3mgebgabyaileyoafd7swklbo6j3ngmjbmgla457rm2kvh3v27q"; // Replace with your PAT
        private readonly string azureDevOpsurlsecurite = "https://vssps.dev.azure.com/omarnabi0452/";
        private readonly AzureDevopsDBContext _context;

        public ProjectInfoController(AzureDevopsDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjectAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(azureDevOpsBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    var response = await httpClient.GetAsync("_apis/projects?api-version=6.0");

                    if (response.IsSuccessStatusCode)
                    {
                        var projectsJson = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<ProjectInfoResponse>(projectsJson);
                        var projectsinfo = data.Value;
                        // Extrayez uniquement les informations nécessaires (id, name, visibility, lastUpdateTime)
                        var selectedProperties = projectsinfo.Select(p => new
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Visibility = p.Visibility,
                            LastUpdateTime = p.LastUpdateTime,
                            Description=p.Description,
                            SourceControlType=p.SourceControlType,
                            TemplateTypeId=p.TemplateTypeId


                        }).ToList();
                     //   _context.ProjectInfos.RemoveRange(_context.ProjectInfos);
                      /* 
                       foreach (var project in selectedProperties)
                        {
                            Models.ProjectInfo projectInfo = new Models.ProjectInfo();
                            projectInfo.Id = project.Id;
                            projectInfo.Name = project.Name;
                            projectInfo.Visibility = project.Visibility;
                            projectInfo.LastUpdateTime = project.LastUpdateTime;
                            projectInfo.Description = project.Description;
                            projectInfo.SourceControlType=project.SourceControlType;
                            projectInfo.TemplateTypeId = project.TemplateTypeId;
                            _context.ProjectInfos.Add(projectInfo);

                        }
                       
                        await _context.SaveChangesAsync();
                        */
                      
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

        [HttpPost]
        public async Task<IActionResult> CreateProjectAsync(Models.ProjectInfo projectInfo )
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
                    var projectData = new
                    {
                        name = projectInfo.Name,
                        description = projectInfo.Description,
                        visibility=projectInfo.Visibility,
                        capabilities = new
                        {
                            versioncontrol = new
                            {
                                sourceControlType =projectInfo.SourceControlType,
                            },
                            processTemplate = new
                            {
                                templateTypeId =projectInfo.TemplateTypeId
                            }
                        }
                    };


                    var projectDataJson = JsonConvert.SerializeObject(projectData);

                    var content = new StringContent(projectDataJson, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("_apis/projects?api-version=7.2-preview.4", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Le projet a été créé avec succès, vous pouvez traiter la réponse ici si nécessaire
                        return Ok("Projet créé avec succès.");
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
        [HttpDelete]
        public async Task<IActionResult> DeleteProjectAsync(string projectId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(azureDevOpsBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    var response = await httpClient.DeleteAsync($"_apis/projects/{projectId}?api-version=7.2-preview.4");

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("Projet supprimé avec succès.");
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
        public async Task<IActionResult> UpdateProjectAsync(string projectId, string newName, string newDescription)
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

                    var response = await httpClient.PatchAsync($"_apis/projects/{projectId}?api-version=7.2-preview.4", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok("Projet mis à jour avec succès.");
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
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectByIdAsync(string projectId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(azureDevOpsBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}")));

                    var response = await httpClient.GetAsync($"_apis/projects/{projectId}?api-version=7.2-preview.4");

                    if (response.IsSuccessStatusCode)
                    {
                        var projectInfoJson = await response.Content.ReadAsStringAsync();
                        // Vous pouvez désérialiser les données JSON ici dans un objet ProjectInfo ou un autre type approprié
                        var projectInfo = JsonConvert.DeserializeObject<Models.ProjectInfo>(projectInfoJson);
                        return Ok(projectInfo); // Retourne les détails du projet
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
