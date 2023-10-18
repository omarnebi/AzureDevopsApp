﻿namespace AzureDevopsApp.Models
{
    public class Sprint
    {
      public int? Id { get; set; }
      public string? Name { get; set; }
      public DateTime? StartDate { get; set; }
      public DateTime? EndDate { get; set; }
      public string? Url { get; set; }
      
    }
}
