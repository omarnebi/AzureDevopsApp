using AzureDevopsApp.Models;

namespace AzureDevopsApp.Datas
{
    public class TeamResponse
    {
        public List<Team> value { get; set; }
        public int count { get; set; }
    }
}
