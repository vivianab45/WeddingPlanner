using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers;

[SessionCheck]//user must be logged in to see any other page
public class WeddingController : Controller
{
    private readonly ILogger<WeddingController> _logger;
    private MyContext _context; 

    public WeddingController(ILogger<WeddingController> logger,  MyContext context)
    {
        _logger = logger;
        _context=context;
    }

    [HttpGet("weddings")]
    public IActionResult Dashboard()
    {
        List <Wedding>  allWeddings=_context.Weddings.Include(w=>w.UserRSVPs).ToList();
        return View(allWeddings);
    }

    [HttpGet("weddings/new")]
    public ViewResult NewWedding()
    {
        return View ();
    }

    [HttpPost("weddings/create")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        if(!ModelState.IsValid)
        {
            return View("NewWedding");
        }
        newWedding.UserId= (int)HttpContext.Session.GetInt32("UUID");
        _context.Add(newWedding);
        _context.SaveChanges();
        return ViewWedding(newWedding.WeddingId);
    }
    [HttpGet("weddings/{id}/view")]
    public IActionResult ViewWedding (int id)
    {
        Wedding? oneWedding=_context.Weddings
                           .Include(w=>w.Creator) 
                           .Include(w=>w.UserRSVPs)
                           .ThenInclude(uwr=>uwr.RSVP)
                           .FirstOrDefault(w=>w.WeddingId==id);  
        if (oneWedding==null)
        {
            return RedirectToAction("Dashboard");
        } 
        return View ("ViewWedding", oneWedding) ;    
    }

    [HttpPost("weddings/{id}/delete")]
    public RedirectToActionResult DeleteWedding( int id)
    {
        Wedding? toDelete= _context.Weddings.SingleOrDefault(w=>w.WeddingId==id);
        if(toDelete !=null)
        {
            _context.Remove(toDelete);
            _context.SaveChanges();
        }
        return RedirectToAction("Dashboard");
    }

    [HttpPost("weddings/{id}/rsvp")]
    public RedirectToActionResult ToggleRSVP (int id)
    {
        int UUID= (int)HttpContext.Session.GetInt32("UUID");
        UserWeddingRSVP existingRSVP= _context.UserWeddingRSVPs.FirstOrDefault(r=>r.WeddingId==id && r.UserId==UUID);
        if(existingRSVP ==null)
        {
            UserWeddingRSVP newRSVP= new()
            {
                UserId=UUID, 
                WeddingId=id
            };
            _context.Add(newRSVP);
        }
        else
        {
            _context.Remove(existingRSVP);
        }
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
