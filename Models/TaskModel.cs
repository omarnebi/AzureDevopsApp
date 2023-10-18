namespace AzureDevopsApp.Models
{
    public class TaskModel
    {
        public string Op { get; set; } = "add";
        public string Path { get; set; }
        public string Value { get; set; }
    }
}
