using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Extensions.CognitoAuthentication;
using Galaxy.Web.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Amazon.AspNetCore.Identity.Cognito;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Galaxy.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;

        public AccountsController (SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool pool)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _pool = pool;
        }

        public async Task<ActionResult> SignUp()
        {
            var model = new SignUpModel();
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _pool.GetUser(model.Email);
                if(user.Status != null)
                {
                    ModelState.AddModelError("UserExists","user already exsits");
                    return View(model);
                }

                user.Attributes.Add(CognitoAttribute.Name.AttributeName, model.Email);
                var createdUser = await _userManager.CreateAsync(user, model.Password);
                if(createdUser.Succeeded)
                {
                   return RedirectToAction("Confirm");
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Confirm()
        {
            var model = new ConfirmModel();
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Confirm(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user == null)
                {
                    ModelState.AddModelError("NotFound", "A user with email Id not found");
                }
                var result = await (_userManager as CognitoUserManager<CognitoUser>).ConfirmSignUpAsync(user, model.Code, true).ConfigureAwait(false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach( var item in result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }
    }
}
