
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public enum Status
{
    [Display(Name = "Zgłoszone")]
    Deposited,
    [Display(Name = "Odebrane")]
    Recived,
    [Display(Name = "Oddane")]
    Transfered,
    [Display(Name = "Archiwalne")]
    Archive
}