using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers;


public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private MyContext _context; 

    public UserController(ILogger<UserController> logger,  MyContext context)
    {
        _logger = logger;
        _context=context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    // [HttpGet("users/success")]
    // public IActionResult Success()
    // {
    //     return View("Success");
    // }
    ///process registration form
    [HttpPost("users/register")]
    public IActionResult CreateUser(User newUser)
    {
        if(!ModelState.IsValid)
        {
            return View("Index");
        }
        PasswordHasher <User> hasher= new();
        //hasher is a variable we created
        newUser.Password=hasher.HashPassword(newUser, newUser.Password);
        _context.Add(newUser);
        _context.SaveChanges();

        //store UserId into Session
        HttpContext.Session.SetInt32("UUID", newUser.UserId);
        HttpContext.Session.SetString("LoggedUser", newUser.FirstName);
        //if logged in,
        return RedirectToAction ("Dashboard", "Wedding");//update this once we have another controller("File","Controller")
    }

    ///Login
    [HttpPost("users/login")]
    public IActionResult LoginUser (LogUser logAttempt)
    {
        if (!ModelState.IsValid)
        {
            return View ("Index");
        }
        User? dbUser = _context.Users.FirstOrDefault (u=>u.Email ==logAttempt.LogEmail);
        if (dbUser==null)
        {
            //user not in db
            // ModelState.AddModelError ("LogEmail", "Email Not Found");
            ModelState.AddModelError("LogPassword", "Invalid Credentials");
            return View("Index");
        }
        /// password check
        PasswordHasher <LogUser> hasher= new();
        PasswordVerificationResult pwCompareResult= hasher.VerifyHashedPassword(logAttempt, dbUser.Password, logAttempt.LogPassword);
        if (pwCompareResult ==0)
        {
            ModelState.AddModelError("LogPassowrd", "Invalid Credentials");
            return View ("Index");
        }
        //session
        HttpContext.Session.SetInt32("UUID", dbUser.UserId);
        HttpContext.Session.SetString("LoggedUser", dbUser.FirstName);
        return RedirectToAction ("Dashboard", "Wedding"); //Update this once we have another controller
    }

    //Logout
    [HttpPost("users/logout")]
    public IActionResult LogOut ()
    {
        //if we went to clear everyything
        // HttpContext.Session.Clear();
        HttpContext.Session.Remove("UUID");
        HttpContext.Session.Remove("LoggedUser");
        return RedirectToAction ("Index");

    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
