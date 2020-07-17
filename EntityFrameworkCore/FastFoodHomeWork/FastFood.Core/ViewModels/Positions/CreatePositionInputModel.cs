namespace FastFood.Core.ViewModels.Positions
{
    using System.ComponentModel.DataAnnotations;

    public class CreatePositionInputModel
    {
        [Required(ErrorMessage = "Please insert a new position name!")]
        [StringLength(30, MinimumLength = 3,ErrorMessage= "Minimum length 3 and maximum length is 20 symbols!")]
        public string PositionName { get; set; }
    }
}
