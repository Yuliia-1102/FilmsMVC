﻿using System.ComponentModel.DataAnnotations;

namespace FilmsInfrastructure.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть E-mail.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введіть пароль.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
