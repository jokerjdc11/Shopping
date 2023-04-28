using Microsoft.AspNetCore.Mvc;
using Shopping.Helpers;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        public AccountController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        //Tiene dos metodos un get y un post uno muestra la vista y el post lleva el resultado
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) // validamos si esta logueado
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel()); // se le muestra la vista para iniciar sesion
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) // Que el modelo sea valido
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded) // si se logueo lo enviamos al controlador home
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> NotAuthorized()
        {
            return View();
        }


    }
}
