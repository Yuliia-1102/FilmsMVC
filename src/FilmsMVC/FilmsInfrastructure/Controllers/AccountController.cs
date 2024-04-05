using FilmsDomain.Model;
using FilmsInfrastructure.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FilmsInfrastructure.Models;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FilmsInfrastructure.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
        private readonly DbfilmsContext _filmsDbContext;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, DbfilmsContext filmsDbContext)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            _filmsDbContext = filmsDbContext;

        }
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Name = model.Name };
                List<string> roles = new List<string>();
                roles.Add("user");

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, roles);
                    await _signInManager.SignInAsync(user, false);

                    Customer c = new Customer { Email = user.Email, Name = user.Name };
                    _filmsDbContext.Add(c);
                    await _filmsDbContext.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //if (ModelState.IsValid)
            //{

            var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильний логін чи (та) пароль");
                }
            //}
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }

}
