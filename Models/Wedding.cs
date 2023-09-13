#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models;

public class Wedding
{
    [Key]
    public int WeddingId { get; set; }

    [Required(ErrorMessage ="Wedder One Required")]       
    [Display(Name="Wedder One Name:")] 
    public string WedderOneName { get; set; }
    
    [Required(ErrorMessage ="Wedder Two Required")]       
    [Display(Name="Wedder Two Name:")] 
    public string WedderTwoName { get; set; }

    [Required]
    [DataType(DataType.Date)] 
    [FutureDate]
    public DateTime Date {get;set;}

    [Required]
    public string Address {get;set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    //A user can create a wedding
    public int UserId { get; set; }
    public User? Creator {get; set; }

    public List<UserWeddingRSVP> UserRSVPs { get; set; } = new();
}

//validation: DOB must be in the past
public class FutureDateAttribute: ValidationAttribute
{
    protected override ValidationResult IsValid (object value, ValidationContext validationContext)
    {
        if (((DateTime)value) < DateTime.Now)
        {
            return new ValidationResult ("Date must be in the future");
        } else{
            return ValidationResult.Success;
        }
    }
}