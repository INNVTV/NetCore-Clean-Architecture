using Core.Common.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Commands
{
    public class CloseAccountCommand : IRequest<BaseResponse>
    {
        public string Id { get; set; }

        #region Command Authorization Options

        /*-------------------------------------------
        * Authorization Properties (To be managed via 'AuthorizationBehavior' Pipeline Behavior)
        * ------------------------------------------*/
        /*
         
        public CloseAccountCommand()
        {
            Exemption = false;
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
