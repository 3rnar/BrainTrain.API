using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using BrainTrain.Core.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrainTrain.API.Controllers
{
    [Authorize]
    [Route("api/Account")]
    public class AccountController// : ApiController
    {
        //private const string LocalLoginProvider = "Local";
        //private ApplicationUserManager _userManager;
        //private BrainTrainContext db = new BrainTrainContext();
        //public string ClientHost { get; set; } = "http://89.250.87.52:8055";

        //public AccountController()
        //{
        //}

        //public AccountController(ApplicationUserManager userManager,
        //    ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        //{            
        //    UserManager = userManager;
        //    AccessTokenFormat = accessTokenFormat;
        //}

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}

        //public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        //[HttpGet]
        //[Route("Managers")]
        //public IEnumerable<ManagersWithIdViewModel> GetManagers()
        //{
        //    var users = db.Users.Where(u => u.Roles.Any(r => r.RoleId == "4") || u.Roles.Any(r => r.RoleId == "2") || u.Roles.Any(r => r.RoleId == "1")).
        //        Select(u => new ManagersWithIdViewModel { Id = u.Id, UserName = u.UserName }).ToList();

        //    return users;
        //}

        //// GET api/Account/UserInfo
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        //[Route("UserInfo")]
        //public UserInfoViewModel GetUserInfo()
        //{
        //    ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

        //    return new UserInfoViewModel
        //    {
        //        Email = User.Identity.GetUserName(),
        //        HasRegistered = externalLogin == null,
        //        LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
        //    };
        //}

        //// POST api/Account/Logout
        //[Route("Logout")]
        //public IActionResult Logout()
        //{
        //    Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
        //    return Ok();
        //}

        //// GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        //[Route("ManageInfo")]
        //public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        //{
        //    IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

        //    if (user == null)
        //    {
        //        return null;
        //    }

        //    List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

        //    foreach (IdentityUserLogin linkedAccount in user.Logins)
        //    {
        //        logins.Add(new UserLoginInfoViewModel
        //        {
        //            LoginProvider = linkedAccount.LoginProvider,
        //            ProviderKey = linkedAccount.ProviderKey
        //        });
        //    }

        //    if (user.PasswordHash != null)
        //    {
        //        logins.Add(new UserLoginInfoViewModel
        //        {
        //            LoginProvider = LocalLoginProvider,
        //            ProviderKey = user.UserName,
        //        });
        //    }

        //    return new ManageInfoViewModel
        //    {
        //        LocalLoginProvider = LocalLoginProvider,
        //        Email = user.UserName,
        //        Logins = logins,
        //        ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
        //    };
        //}

        //// POST api/Account/ChangePassword
        //[Route("ChangePassword")]
        //public async Task<IActionResult> ChangePassword(ChangePasswordBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
        //        model.NewPassword);
            
        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    return Ok();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("ResetPassword")]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var user = await UserManager.FindByNameAsync(model.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return Ok();
        //    }
        //    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}

        //// POST api/Account/SetPassword
        //[Route("SetPassword")]
        //public async Task<IActionResult> SetPassword(SetPasswordBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    return Ok();
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("ForgetPassword")]
        //public async Task<IActionResult> ForgetPassword(string email)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = await UserManager.FindByEmailAsync(email);
        //    if (user == null)
        //        return BadRequest();

        //    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

        //    EmailService emailService = new EmailService();
        //    emailService.Send(new IdentityMessage
        //    {
        //        Destination = user.Email,
        //        Subject = "Сброс пароля",
        //        Body = $"Пожалуйста, сбросьте старый пароль, перейдя по этой  <a href=\"{ClientHost}/#/reset-password-page/{HttpUtility.UrlEncode(code)}\">ссылке</a>"
        //    });
        //    return Ok();

        //}

        //// POST api/Account/AddExternalLogin
        //[Route("AddExternalLogin")]
        //public async Task<IActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        //    AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

        //    if (ticket == null || ticket.Identity == null || (ticket.Properties != null
        //        && ticket.Properties.ExpiresUtc.HasValue
        //        && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
        //    {
        //        return BadRequest("External login failure.");
        //    }

        //    ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

        //    if (externalData == null)
        //    {
        //        return BadRequest("The external login is already associated with an account.");
        //    }

        //    IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
        //        new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    return Ok();
        //}

        //// POST api/Account/RemoveLogin
        //[Route("RemoveLogin")]
        //public async Task<IActionResult> RemoveLogin(RemoveLoginBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    IdentityResult result;

        //    if (model.LoginProvider == LocalLoginProvider)
        //    {
        //        result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
        //    }
        //    else
        //    {
        //        result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
        //            new UserLoginInfo(model.LoginProvider, model.ProviderKey));
        //    }

        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    return Ok();
        //}

        //// GET api/Account/ExternalLogin
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        //[AllowAnonymous]
        //[Route("ExternalLogin", Name = "ExternalLogin")]
        //public async Task<IActionResult> GetExternalLogin(string provider, string error = null)
        //{
        //    if (error != null)
        //    {
        //        return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
        //    }

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return new ChallengeResult(provider, this);
        //    }

        //    ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

        //    if (externalLogin == null)
        //    {
        //        return InternalServerError();
        //    }

        //    if (externalLogin.LoginProvider != provider)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //        return new ChallengeResult(provider, this);
        //    }

        //    ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
        //        externalLogin.ProviderKey));

        //    bool hasRegistered = user != null;

        //    if (hasRegistered)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
        //         ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
        //            OAuthDefaults.AuthenticationType);
        //        ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
        //            CookieAuthenticationDefaults.AuthenticationType);

        //        AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.Id, user.UserName, user.AvatarUrl ?? "", user.FirstName ?? "", user.LastName ?? "");
        //       Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
        //    }
        //    else
        //    {
        //        IEnumerable<Claim> claims = externalLogin.GetClaims();
        //        ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
        //        Authentication.SignIn(identity);
        //    }

        //    return Ok();
        //}

        //// GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        //[AllowAnonymous]
        //[Route("ExternalLogins")]
        //public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        //{
        //    IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
        //    List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

        //    string state;

        //    if (generateState)
        //    {
        //        const int strengthInBits = 256;
        //        state = RandomOAuthStateGenerator.Generate(strengthInBits);
        //    }
        //    else
        //    {
        //        state = null;
        //    }

        //    foreach (AuthenticationDescription description in descriptions)
        //    {
        //        ExternalLoginViewModel login = new ExternalLoginViewModel
        //        {
        //            Name = description.Caption,
        //            Url = Url.Route("ExternalLogin", new
        //            {
        //                provider = description.AuthenticationType,
        //                response_type = "token",
        //                client_id = Startup.PublicClientId,
        //                redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
        //                state = state
        //            }),
        //            State = state
        //        };
        //        logins.Add(login);
        //    }

        //    return logins;
        //}

        //// POST api/Account/Register
        //[AllowAnonymous]
        //[Route("Register")]
        //public async Task<IActionResult> Register(RegisterBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = new ApplicationUser() { UserName = model.Email, Email = model.Email, BirthDate = DateTime.Now, LastVisitDate = DateTime.Now, RegistrationDate = DateTime.Now, LevelId = 1};
            
        //    try
        //    {
        //        IdentityResult result = await UserManager.CreateAsync(user, model.Password);

        //        await UserManager.AddToRoleAsync(user.Id, "Обычный пользователь");             

        //        if (!result.Succeeded)
        //        {
        //            return GetErrorResult(result);
        //        }

        //        db.UserRatings.Add(new UserRatings { Rating = 0, UserId = user.Id });
        //        await db.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }


        //    return Ok();
        //}

        //// POST api/Account/RegisterExternal
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        //[Route("RegisterExternal")]
        //public async Task<IActionResult> RegisterExternal(RegisterExternalBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var info = await Authentication.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return InternalServerError();
        //    }

        //    var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

        //    IdentityResult result = await UserManager.CreateAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result); 
        //    }
        //    return Ok();
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && _userManager != null)
        //    {
        //        _userManager.Dispose();
        //        _userManager = null;
        //    }

        //    base.Dispose(disposing);
        //}

        //#region Helpers

        //private IAuthenticationManager Authentication
        //{
        //    get { return Request.GetOwinContext().Authentication; }
        //}

        //private IActionResult GetErrorResult(IdentityResult result)
        //{
        //    if (result == null)
        //    {
        //        return InternalServerError();
        //    }

        //    if (!result.Succeeded)
        //    {
        //        if (result.Errors != null)
        //        {
        //            foreach (string error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error);
        //            }
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            // No ModelState errors are available to send, so just return an empty BadRequest.
        //            return BadRequest();
        //        }

        //        return BadRequest(ModelState);
        //    }

        //    return null;
        //}

        //private class ExternalLoginData
        //{
        //    public string LoginProvider { get; set; }
        //    public string ProviderKey { get; set; }
        //    public string UserName { get; set; }

        //    public IList<Claim> GetClaims()
        //    {
        //        IList<Claim> claims = new List<Claim>();
        //        claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

        //        if (UserName != null)
        //        {
        //            claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
        //        }

        //        return claims;
        //    }

        //    public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
        //    {
        //        if (identity == null)
        //        {
        //            return null;
        //        }

        //        Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

        //        if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
        //            || String.IsNullOrEmpty(providerKeyClaim.Value))
        //        {
        //            return null;
        //        }

        //        if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
        //        {
        //            return null;
        //        }

        //        return new ExternalLoginData
        //        {
        //            LoginProvider = providerKeyClaim.Issuer,
        //            ProviderKey = providerKeyClaim.Value,
        //            UserName = identity.FindFirstValue(ClaimTypes.Name)
        //        };
        //    }
        //}

        //private static class RandomOAuthStateGenerator
        //{
        //    private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

        //    public static string Generate(int strengthInBits)
        //    {
        //        const int bitsPerByte = 8;

        //        if (strengthInBits % bitsPerByte != 0)
        //        {
        //            throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
        //        }

        //        int strengthInBytes = strengthInBits / bitsPerByte;

        //        byte[] data = new byte[strengthInBytes];
        //        _random.GetBytes(data);
        //        return HttpServerUtility.UrlTokenEncode(data);
        //    }
        //}

        //#endregion
    }
}
