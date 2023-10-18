using AzureDevopsApp.Models;
namespace AzureDevopsApp.Datas
{
    public class ProjectInfoResponse
    {
        public int Count { get; set; }
        public List<ProjectInfo> Value { get; set; }
    }
}
