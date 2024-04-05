using System.ComponentModel.DataAnnotations;

namespace FilmsInfrastructure.ViewModel
{
    public class AddRoleViewModel
    {
        [Required(ErrorMessage = "Введіть назву ролі.")]
        [Display(Name = "Назва ролі")]
        public string Name { get; set; }
    }
}
