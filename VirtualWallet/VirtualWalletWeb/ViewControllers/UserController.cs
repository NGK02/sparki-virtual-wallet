﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.Dto.UserDto;
using VirtualWallet.Dto.ViewModels.UserViewModels;
using VirtualWallet.Web.Helper;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IAuthManager authManager;
        private readonly IImageManager imageManager;
        private readonly IAuthManagerMvc authManagerMVC;
        private readonly IReferralService referralService;
        private readonly IWalletService walletService;
        private readonly IEmailSender emailSender;

        public UserController(IUserService userService,
                                IAuthManager authManager,
                                IMapper mapper,
                                IImageManager imageManager,
                                IAuthManagerMvc authManagerMVC,
                                IReferralService referralService,
                                IWalletService walletService,
                                IEmailSender emailSender)
        {
            this.userService = userService;
            this.authManager = authManager;
            this.mapper = mapper;
            this.imageManager = imageManager;
            this.authManagerMVC = authManagerMVC;
            this.referralService = referralService;
            this.walletService = walletService;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult ViewUser(int id)
        {
            try
            {
                if (!authManagerMVC.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }
                if (!authManagerMVC.IsAdmin("roleId") && !authManagerMVC.IsContentCreator("userId", id))
                {
                    this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    this.ViewData["ErrorMessage"] = AuthManagerMvc.notAuthorized;
                    return View("Error");
                }

                var user = userService.GetUserById(id);
                var mappedUser = mapper.Map<GetUserViewModel>(user);
                this.ViewBag.userId = id;
                return View(mappedUser);
            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
            catch (UnauthenticatedOperationException e)
            {
                this.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            var login = new Login();

            return View(login);
        }

        [HttpPost]
        public IActionResult Login(Login filledLoginForm)
        {
            try
            {
                //Влизаме тук ако имаме Null Username or Password!
                if (!this.ModelState.IsValid)
                {
                    return View(filledLoginForm);
                }

                //Защо това се ползва тук? Ако така или иначе ще ползваме този аут мениджър нека всичко да е в него.
                var user = authManager.IsAuthenticated(new string[] { filledLoginForm.Username, filledLoginForm.Password });

                if (!user.IsConfirmed)
                {
                    return View("EmailNotConfirmed", filledLoginForm);
                }

                this.HttpContext.Session.SetString("LoggedUser", filledLoginForm.Username);
                this.HttpContext.Session.SetInt32("userId", user.Id);
                this.HttpContext.Session.SetInt32("roleId", user.RoleId);
                this.HttpContext.Session.SetString("profilePicPath", user.ProfilePicPath ?? "/Assets/pfp.jpg");

                return RedirectToAction("Index", "Home");
            }
            catch (EntityNotFoundException)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = "Invalid username or password!";
                return View(filledLoginForm);
            }
            catch (UnauthenticatedOperationException e)
            {
                this.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = e.Message;
                return View(filledLoginForm);
            }
            
        }

        [HttpGet]
        public IActionResult Logout()
        {
            //Да се използва
            //this.HttpContext.Session.Clear();

            this.HttpContext.Session.Remove("LoggedUser");
            this.HttpContext.Session.Remove("userId");
            this.HttpContext.Session.Remove("roleId");
            this.HttpContext.Session.Remove("profilePicPath");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var registerUser = new RegisterUser();

            return View(registerUser);
        }

        [HttpGet]
        public IActionResult RegisterReferredUser(string token)
        {
            var registerUser = new RegisterUser()
            {
                ReferralToken = token
            };

            return View("Register", registerUser);
        }

        [HttpPost]
        public IActionResult Register(RegisterUser filledForm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(filledForm);
                }

                var user = mapper.Map<User>(filledForm);

                if (filledForm.ProfilePic is null)
                {
                    filledForm.ProfilePic = imageManager.GeneratePlaceholderAvatar(filledForm.FirstName, filledForm.LastName);
                    user.ProfilePicPath = imageManager.UploadGeneratedProfilePicInRoot(filledForm.ProfilePic).Result;
                }
                else
                {
                    user.ProfilePicPath = imageManager.UploadOriginalProfilePicInRoot(filledForm.ProfilePic).Result;
                }

                //EmailSender emailSender = new EmailSender();
                string confirmationToken = EmailSender.GenerateConfirmationToken();
                var expiryTimestamp = DateTime.UtcNow.AddHours(24);

                user.ConfirmationToken = confirmationToken;
                user.ConfirmationTokenExpiry = expiryTimestamp;
                user.IsConfirmed = false;
                userService.CreateUser(user);

                string emailSubject = "Registration Confirmation";
                string toUser = $"{user.FirstName} {user.LastName}";

                string emailMessage = $"Dear {user.FirstName}, please confirm your registration by clicking the link below:\n\n" +
                    $"{Url.Action("ConfirmEmail", "User", new { userId = user.Id, token = confirmationToken }, Request.Scheme)}";

                emailSender.SendEmail(emailSubject, user.Email, toUser, emailMessage).Wait();
                ViewBag.SuccessMessage = "Activation email was sent to your Email. Please activate your account!";

                if (!string.IsNullOrEmpty(filledForm.ReferralToken))
                {
                    var referral = referralService.GetReferralByToken(filledForm.ReferralToken);
                    var referrer = userService.GetUserById(referral.ReferrerId);

                    if (referrer.ReferralCount < 5)
                    {
                        referrer.ReferralCount++;
                        return RedirectToAction("ReceiveBonus", "User", new { referrerId = referral.ReferrerId, referredUserId = user.Id });
                    }
                }
				return View("Successful");
            }
            catch (EmailAlreadyExistException e)
            {
                this.Response.StatusCode = StatusCodes.Status409Conflict;
                this.ViewData["ErrorMessage"] = e.Message;
                return View(filledForm);
            }
            catch (UsernameAlreadyExistException e)
            {
                this.Response.StatusCode = StatusCodes.Status409Conflict;
                this.ViewData["ErrorMessage"] = e.Message;
                return View(filledForm);
            }
            catch (PhoneNumberAlreadyExistException e)
            {
                this.Response.StatusCode = StatusCodes.Status409Conflict;
                this.ViewData["ErrorMessage"] = e.Message;
                return View(filledForm);
            }
            
        }

        [HttpGet]
        public IActionResult ReceiveBonus(int referrerId, int referredUserId)
        {
            try
            {
                walletService.DistributeFundsForReferrals(referrerId, referredUserId, 70, 3);
                ViewBag.SuccessMessage = "Registered successfully!";
                return View("Successful");
            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
           
        }

        [HttpGet]
        public IActionResult ConfirmEmail(string userId, string token)
        {
            try
            {
                var user = userService.GetUserById(int.Parse(userId));

                if (user == null || user.IsConfirmed || token != user.ConfirmationToken)
                {
                    return View("Error");
                }

                if (user.ConfirmationTokenExpiry < DateTime.UtcNow)
                {
                    Login loginForm = new Login
                    {
                        Username = user.Username,
                        Password = user.Password
                    };

                    return View("ConfirmationTokenExpired", loginForm);
                }

                user.IsConfirmed = true;
                userService.ConfirmUser(user, int.Parse(userId));
                ViewBag.SuccessMessage = "Your account has been confirmed. You can now log in.";

                return View("Successful");
            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
            
        }

        [HttpPost]
        public IActionResult ResendConfirmationEmail(Login filledLoginForm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(filledLoginForm);
                }

                var user = userService.GetUserByUsername(filledLoginForm.Username);

                if (user == null)
                {
                    ViewData["ErrorMessage"] = "User was not found!";
                    return View("Error");
                }
                if (user.IsConfirmed)
                {
                    ViewData["ErrorMessage"] = "User already confirmed";
                    return View("Error");
                }

                string confirmationToken = EmailSender.GenerateConfirmationToken();
                var expiryTimestamp = DateTime.UtcNow.AddHours(24);

                user.ConfirmationToken = confirmationToken;
                user.ConfirmationTokenExpiry = expiryTimestamp;
                userService.UpdateUserConfirmationToken(user, filledLoginForm.Username);

                string emailSubject = "Registration Confirmation";
                string toUser = $"{user.FirstName} {user.LastName}";

                string emailMessage = $"Dear {user.FirstName}, please confirm your registration by clicking the link below:\n\n" +
                    $"{Url.Action("ConfirmEmail", "User", new { userId = user.Id, token = confirmationToken }, Request.Scheme)}";

               // EmailSender emailSender = new EmailSender();
                emailSender.SendEmail(emailSubject, user.Email, toUser, emailMessage).Wait();

                ViewBag.SuccessMessage = "Activation email was sent to your email. Please activate your account!";

                return View("Successful");
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
            
        }

        [HttpGet]
        public IActionResult EditUser([FromRoute] int id)
        {
            try
            {

                if (!authManagerMVC.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }
                if (!authManagerMVC.IsAdmin("roleId") && !authManagerMVC.IsContentCreator("userId", id))
                {
                    this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    this.ViewData["ErrorMessage"] = AuthManagerMvc.notAuthorized;
                    return View("Error");
                }
                if (id == 0)
                {
                    this.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    this.ViewData["ErrorMessage"] = "Please provide ID for User!";
                    return View("Error");
                }
                var editUser = new EditUser();
                var user = userService.GetUserById(id);
                editUser.OldProfilePicPath = user.ProfilePicPath;
                return View(editUser);

            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
            catch (Exception e)
            {
                this.Response.StatusCode = StatusCodes.Status500InternalServerError;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult EditUser([FromRoute] int id, EditUser editedUser)
        {
            try
            {
                if (!authManagerMVC.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }
                if (!authManagerMVC.IsAdmin("roleId") && !authManagerMVC.IsContentCreator("userId", id))
                {
                    this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    this.ViewData["ErrorMessage"] = AuthManagerMvc.notAuthorized;
                    return View("Error");
                }
                if (!this.ModelState.IsValid)
                {
                    return View(editedUser);
                }
                if (editedUser.FirstName is null &&
                    editedUser.LastName is null &&
                    editedUser.Password is null &&
                    editedUser.Email is null &&
                    editedUser.PhoneNumber is null &&
                    editedUser.ProfilePic is null)
                {
                    this.ViewData["ErrorMessage"] = "There's nothing filled!";
                    return View(editedUser);
                }
                var newUserValues = mapper.Map<User>(editedUser);

                if (editedUser.ProfilePic is not null)
                {
                    newUserValues.ProfilePicPath = imageManager.UploadOriginalProfilePicInRoot(editedUser.ProfilePic).Result;
                    imageManager.DeleteProfilePicFromRoot(editedUser.OldProfilePicPath);
                }
                userService.UpdateUser(id, newUserValues);
                //TODO Redirect to Successfull page
                return RedirectToAction("Index", "Home");

            }
            catch (EmailAlreadyExistException e)
            {
                this.Response.StatusCode = StatusCodes.Status409Conflict;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
            catch (PhoneNumberAlreadyExistException e)
            {
                this.Response.StatusCode = StatusCodes.Status409Conflict;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
            
        }

        [HttpGet]
        public IActionResult ReferFriend(ReferFriend referFriend)
        {
            if (!authManagerMVC.IsLogged("LoggedUser"))
            {
                return RedirectToAction("Login", "User");
            }

            return View(referFriend);
        }

        [HttpPost]
        public IActionResult ReferFriendFinalize(ReferFriend filledForm)
        {
            try
            {
                if (!authManagerMVC.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                if (!ModelState.IsValid)
                {
                    ViewData["ErrorMessage"] = (filledForm.Email is null ? "Please provide email to refer." : "Please enter valid email!");

                    return View("ReferFriend", filledForm);
                }

                if (userService.EmailExists(filledForm.Email))
                {
                    ViewData["ErrorMessage"] = "Email alredy exist";

                    return View("ReferFriend", filledForm);
                }

                int userId = HttpContext.Session.GetInt32("userId") ?? 0;

               // EmailSender emailSender = new EmailSender();
                string confirmationToken = "";

                bool isUnique = false;

                while (!isUnique)
                {
                    confirmationToken = EmailSender.GenerateConfirmationToken();
                    var existingReferral = referralService.GetReferralByToken(confirmationToken);

                    if (existingReferral == null)
                    {
                        isUnique = true;
                    }
                }

                var expiryTimestamp = DateTime.UtcNow.AddHours(24);

                Referral referral = new Referral()
                {
                    ConfirmationToken = confirmationToken,
                    ConfirmationTokenExpiry = expiryTimestamp,
                    IsConfirmed = false,
                    ReferredEmail = filledForm.Email
                };

                referralService.CreateReferral(userId, referral);
                string emailSubject = "Invitation to Register";

                string emailMessage = $"You've been invited to join our app! Click the following link to register:\n\n" +
                    $"{Url.Action("OpenInvitation", "User", new { token = confirmationToken }, Request.Scheme)}";

                emailSender.SendEmail(emailSubject, filledForm.Email, null, emailMessage).Wait();
                ViewBag.SuccessMessage = "Referal successful";
                return View("Successful");
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
            
        }

        [HttpGet]
        public IActionResult OpenInvitation(string token)
        {
            try
            {
                var referral = referralService.GetReferralByToken(token);

                if (referral.IsConfirmed)
                {
                    return View("Error");
                }

                if (referral.ConfirmationTokenExpiry < DateTime.UtcNow)
                {
                    throw new UnauthenticatedOperationException("The invitation link has expired.");
                }

                return RedirectToAction("RegisterReferredUser", "User", new { token = token });
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
            
        }
    }
}