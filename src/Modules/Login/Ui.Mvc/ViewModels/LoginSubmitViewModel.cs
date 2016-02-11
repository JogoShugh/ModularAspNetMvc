using System.ComponentModel.DataAnnotations;
using Core.Infrastructure.Ui.Mvc.ViewModels;

namespace Core.Modules.Authentication.Ui.Mvc.ViewModels
{
    public class LoginSubmitViewModel
    {
        public LoginSubmitViewModel()
        {
            CommandActions = new CommandActionCollection();
        }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [ScaffoldColumn(false)]
        public CommandActionCollection CommandActions { get; set; }
    }
}