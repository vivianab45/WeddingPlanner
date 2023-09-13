#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models;

public class User
{
    [Key]
    public int UserId {get;set;}
    
    [Required(ErrorMessage = "First name is required!")]
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters in length")]
    public string FirstName {get;set;}

    [Required(ErrorMessage = "Last name is required!")]
    [MinLength(2, ErrorMessage = "Last name must be at least 2 characters in length")]
    public string LastName {get;set;}

    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress]
    [UniqueEmail]
    public string Email {get;set;}

    [Required(ErrorMessage = "Password is required!")]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;


    [Required(ErrorMessage = "Password confirm is required!")]
    [MinLength(8)]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string Confirm {get;set;}

    //nav props
    public List<Wedding> Creations {get;set;}= new ();

    public List<UserWeddingRSVP> WeddingRSVPs {get;set;}= new();
}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
            return new ValidationResult("Email is required!");
        }
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        if(_context.Users.Any(e => e.Email == value.ToString()))
        {
            return new ValidationResult("Email must be unique!");
        } else {
            return ValidationResult.Success;
        }
    }
}
