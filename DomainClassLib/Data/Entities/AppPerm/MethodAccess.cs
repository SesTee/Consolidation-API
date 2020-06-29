using System.ComponentModel.DataAnnotations;

namespace DomainClassLib.Data.Entities.AppPerm
{
    public class MethodAccess
    {
        [Required]
        public string AppID { get; set; }

        [Required]
        public string ControllerName { get; set; }

        [Required]
        public string MethodName { get; set; }

        [Required]
        public string AccessLevel { get; set; }
    }
}
