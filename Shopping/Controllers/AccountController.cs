using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Enums;
using Shopping.Helpers;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
		private readonly DataContext _context;
		private readonly IComboxHelper _comboxHelper;

		public AccountController(IUserHelper userHelper,DataContext context, IComboxHelper comboxHelper)
        {
            _userHelper = userHelper;
			_context = context;
			_comboxHelper = comboxHelper;
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

		public async Task<IActionResult> Register()
		{
			AddUserViewModel model = new AddUserViewModel
			{
				Id = Guid.Empty.ToString(),
				Countries = await _comboxHelper.GetComboCountriesAsync(),
				States = await _comboxHelper.GetComboStatesAsync(0),
				Cities = await _comboxHelper.GetComboCitiesAsync(0),
				UserType = UserType.User,
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(AddUserViewModel model)
		{
			if (ModelState.IsValid) // con esta linea se valida todas las anotation de cada uno de las propiedades
			{
				Guid imageId = Guid.Empty;

				//if (model.ImageFile != null)
				//{
				//	imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
				//}

				model.ImageId = imageId;
				User user = await _userHelper.AddUserAsync(model);
				if (user == null)
				{
					ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
					return View(model);
				}

				LoginViewModel loginViewModel = new LoginViewModel
				{
					Password = model.Password,
					RememberMe = false,
					Username = model.Username
				};

				var result2 = await _userHelper.LoginAsync(loginViewModel); // logueamos si creamos el usuario

				if (result2.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
			}

			return View(model);
		}

        public JsonResult GetStates(int countryId)
        {
            Country country = _context.Countries
                .Include(c => c.States)
                .FirstOrDefault(c => c.Id == countryId);
            if (country == null)
            {
                return null;
            }

            return Json(country.States.OrderBy(d => d.Name));
        }

        public JsonResult GetCities(int stateId)
        {
            State state = _context.States
                .Include(s => s.Cities)
                .FirstOrDefault(s => s.Id == stateId);
            if (state == null)
            {
                return null;
            }

            return Json(state.Cities.OrderBy(c => c.Name));
        }


    }
}
