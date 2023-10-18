namespace AzureDevopsApp.Models
{
    public class ProjectInfo
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Visibility { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string? Description { get; set; }

        public string? TemplateTypeId { get; set; }

        public string? SourceControlType { get; set; }


        public ProjectInfo(string? name, string? description)
        {
            Name = name;
            Description = description;
        }

        public ProjectInfo(string? id, string? name, string? visibility, DateTime? lastUpdateTime, string? description, string? templateTypeId, string? sourceControlType) : this(id, name)
        {
            Visibility = visibility;
            LastUpdateTime = lastUpdateTime;
            Description = description;
            TemplateTypeId = templateTypeId;
            SourceControlType = sourceControlType;
        }

        public ProjectInfo()
        {
        }
    }

}
