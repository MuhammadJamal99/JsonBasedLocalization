using System.ComponentModel.DataAnnotations;

namespace JsonBasedLocalization.web.ViewModels;

public sealed class CreateTestViewModel
{
    [Display(Name = "name"), Required(ErrorMessage = "required")]
    public string Name { get; set; }
}
