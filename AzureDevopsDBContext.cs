using AzureDevopsApp.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace AzureDevopsApp
{
    public partial class AzureDevopsDBContext : DbContext
    {
        public AzureDevopsDBContext()
        {
        }

        public AzureDevopsDBContext(DbContextOptions<AzureDevopsDBContext> options)
            : base(options)
        {
        }

        // Déclarez ici DbSet<Projet> et d'autres DbSet pour vos entités
        public virtual DbSet<ProjectInfo> ProjectInfos { get; set; }
    }

}
