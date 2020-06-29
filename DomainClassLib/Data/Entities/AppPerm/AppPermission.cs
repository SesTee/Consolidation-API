using System.ComponentModel.DataAnnotations;

namespace DomainClassLib.Data.Entities.AppPerm
{
    public class AppPermission
    {
        [Required]
        public string AppID { get; set; }

        [Required]
        public string AppServerIP { get; set; }

        [Required]
        public string AppName { get; set; }

        [Required]
        public string AccessLevel { get; set; }
    }
}
