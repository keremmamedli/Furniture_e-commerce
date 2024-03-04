using AmadoApp.Business.Exceptions.Account;
using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Implementations;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.AccountVMs;
using AmadoApp.Business.ViewModels.PageVMs;
using AmadoApp.Core.Entities.Account;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace AmadoApp.MVC.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;
		private readonly LinkGenerator _linkGenerator;
		private readonly IHttpContextAccessor _http;

		public AccountController(IAccountService accountService, LinkGenerator linkGenerator, IHttpContextAccessor http)
		{
			_accountService = accountService;
			_linkGenerator = linkGenerator;
			_http = http;
		}

		[HttpGet]
		public async Task<IActionResult> Register()
		{
			if (!User.Identity.IsAuthenticated)
			{
				await CreateRoles();
				return View();
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM vm)
		{
			try
			{
				var result = await _accountService.Register(vm);

				var token = result[0];
				var pincode = result[1];
				var userId = result[2];

				string url = _linkGenerator.GetUriByAction(_http.HttpContext, action: "ConfirmEmail", controller: "Account",
				values: new
				{
					token,
					userId,
					pincode
				});

				return Redirect(url);
			}
			catch (UserRegistrationException ex)
			{
				ModelState.AddModelError(ex.ParamName, ex.Message);

				return View(vm);
			}
			catch (ObjectParamsNullException ex)
			{
				ModelState.AddModelError(ex.ParamName, ex.Message);

				return View(vm);
			}
			catch (UsedEmailException ex)
			{
				ModelState.AddModelError(ex.ParamName, ex.Message);

				return View(vm);
			}
		}

		[HttpGet]
		public async Task<IActionResult> Login()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return View();
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginVM vm, string? returnUrl)
		{
			try
			{
				await _accountService.Login(vm);

				if (returnUrl is not null) return Redirect(returnUrl);

				return RedirectToAction("Index", "Home");
			}
			catch (UserNotFoundException ex)
			{
				ModelState.AddModelError(ex.ParamName, ex.Message);

				return View(vm);
			}
			catch (ObjectParamsNullException ex)
			{
				ModelState.AddModelError(ex.ParamName, ex.Message);

				return View(vm);
			}
		}

		[HttpGet]
		public async Task<IActionResult> CreateRoles()
		{
			await _accountService.CreateRoles();

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			if (User.Identity.IsAuthenticated)
			{
				await _accountService.Logout();

				return RedirectToAction(nameof(Login));
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmEmail()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ConfirmEmail(ConfirmEmailVM vm, string userId, string token, string pincode)
		{
			ViewBag.Success = true;

			if (await _accountService.ConfirmEmailAddress(vm: vm, userId: userId, token: token, pincode: pincode))
			{
				return RedirectToAction(nameof(Login));
			}
			else
			{
				ViewBag.Success = false;
			}

			return View(vm);
		}

		[HttpGet]
		public IActionResult ChangePasswordMailSendPage()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(string email)
		{

			if (email == null)
			{
				ModelState.Clear();

				ModelState.AddModelError("", "Please, enter your email address.");

				return View();
			}

			var currentUser = await _accountService.GetUserByEmailAddressAsync(email);

			if (currentUser == null)
			{
				ModelState.Clear();

				ModelState.AddModelError("", "Entered email address is not valid!");

				return View();
			}

			string url = _linkGenerator.GetUriByAction(_http.HttpContext, action: "ChangePassword", controller: "Account",
						values: new
						{
							email,
						});

			SendMessageService.SendUrlMessageAsync(currentUser, url);

			return RedirectToAction(nameof(ChangePasswordMailSendPage));

		}

		[HttpGet]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordVM vm, string email)
		{
			try
			{
				ChangePasswordVMValidator validations = new ChangePasswordVMValidator();
				var validationResult = await validations.ValidateAsync(vm);

				if (!validationResult.IsValid)
				{
					ModelState.Clear();

					validationResult.Errors.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));

					return View(vm);
				}

				if (await _accountService.ChangePasswordAsync(vm, email))
				{
					return RedirectToAction(nameof(Login));
				}
				else
				{
					return View(vm);
				}
			}
			catch (ChangePasswordException ex)
			{
				var errorMessages = ex.Message.Split('\n');

				foreach (var errorMessage in errorMessages)
				{
					ModelState.AddModelError("", errorMessage.Trim());
				}

				return View(vm);
			}
		}


		[HttpPost]
		public async Task<IActionResult> Subscription(HomeVM vm, string? returnUrl)
		{
			try
			{
				await _accountService.Subscription(vm.SubscribeVM);

				if (returnUrl is not null) return Redirect(returnUrl);

				return RedirectToAction("Index", "Home");
			}
			catch (UsedEmailException ex)
			{
				ModelState.AddModelError(ex.ParamName, ex.Message);

				if (returnUrl is not null) return Redirect(returnUrl);

				return RedirectToAction("Index", "Home", vm);
			}
		}
	}
}