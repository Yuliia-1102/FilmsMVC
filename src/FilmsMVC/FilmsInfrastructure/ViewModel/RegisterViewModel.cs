using System.ComponentModel.DataAnnotations;

namespace FilmsInfrastructure.ViewModel
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Введіть пошту.")]
		[Display(Name = "Email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required(ErrorMessage = "Введіть ім'я.")]
		[MinLength(3), MaxLength(40)]
		[Display(Name = "Ім'я")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Введіть пароль.")]
        [Display(Name = "Пароль")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Введіть пароль ще раз.")]
		[Compare("Password", ErrorMessage = "Паролі не співпадають.")]
		[Display(Name = "Підтвердження паролю")]
		[DataType(DataType.Password)]
		public string PasswordConfirm { get; set; }
	}
}
