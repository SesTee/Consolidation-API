using System.ComponentModel.DataAnnotations;

namespace DomainClassLib.Data.Entities.AppPerm
{
    public class ControllerAccess
    {
        [Required]
        public string AppID { get; set; }

        [Required]
        public string ControllerName { get; set; }

        [Required]
        public string AccessLevel { get; set; }
    }
}
