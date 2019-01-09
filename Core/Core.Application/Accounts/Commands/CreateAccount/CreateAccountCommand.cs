using Core.Application.Accounts.Commands.CreateAccount;
using Core.Application.Accounts.Models;
using Core.Common.Response;
using MediatR;

namespace Core.Application.Accounts.Commands
{
    public class CreateAccountCommand : IRequest<CreateAccountCommandResponse>
    {
        public string Name { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }


        #region Command Authorization Options

        /*------------------------------------------------------------------------------------------
        * Authorization Properties (To be managed via 'AuthorizationBehavior' Pipeline Behavior)
        * ----------------------------------------------------------------------------------------*/
        
        /*
        public CreateAccountCommand()
        {
            Exemption = true;
            MinimumRole = FromRolesList.Role;
        }
        
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string UserType { get; set; } //<-- Account/Platform
        private string MinimumRole { get; set; } //<-- Minumum Role to run function
        private bool Exemption { get; set; } //<-- Allows for exemptions
            
         */

        #endregion

    }
}
