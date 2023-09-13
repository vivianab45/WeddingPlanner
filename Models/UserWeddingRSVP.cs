#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;


public class UserWeddingRSVP
{
    [Key]
    public int UserWeddingRSVPId { get; set; }

    //guestRSVP will be made by:
    public int UserId {get; set;}
    public User? RSVP {get; set;}

    public int WeddingId {get; set;}
    public Wedding? WeddingRSVPed {get; set;}
}