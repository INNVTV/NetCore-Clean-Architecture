using Core.Application.Accounts.Models;
using Core.Application.Accounts.Queries;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Accounts.Commands
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        private readonly IMediator _mediator;

        public CreateAccountValidator(IMediator mediator)
        {
            _mediator = mediator;

            RuleFor(x => x.Name).NotEmpty().WithMessage("Please specify an account name.");
            RuleFor(x => x.Name).Length(2, 40).WithMessage("Account name must be bewtween 2-40 characters in length.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter a valid email address.");
            RuleFor(x => x.Name).Must(BeAValidName).WithMessage(x => $"{x.Name} is a reserved name.");
            RuleFor(x => x.Name).Must(NotExist).WithMessage(x => $"{x.Name} already exists.");         
        }

        private bool NotExist(string name)
        {
            //=========================================================================
            // VALIDATE ACCOUNT NAME IS UNIQUE (Via MediatR Query)
            //=========================================================================
            // Note: "NameKey" is transformed from "Name" and is used as a both a unique id as well as for pretty routes/urls
            // Note: Consider using both "Name and ""NameKey" as UniqueKeys on your DocumentDB collection.
            //-------------------------------------------------------------------------
            // Note: Once these contraints are in place you could remove this manual check
            //  - however this process does ensure no exceptions are thrown and a cleaner response message is sent to the user.
            //----------------------------------------------------------------------------

            var accountDetailsQuery = new GetAccountDetailsQuery { NameKey = Common.Transformations.NameKey.Transform(name)};
            var accountDetails =  _mediator.Send(accountDetailsQuery);

            if (accountDetails.Result.Account != null)
            {
                return false;
            }

            return true;
        }

        private bool BeAValidName(string name)
        {
            // custom code validating logic goes here
            foreach(string reservedName in ReservedAccountNames)
            {
                if(reservedName == Common.Transformations.NameKey.Transform(name))
                {
                    return false;
                }
            }

            return true;
        }

        private static readonly ReadOnlyCollection<string> ReservedAccountNames = new ReadOnlyCollection<string>(new[]
        {
            #region Reserved Account Name
            
            //a
            "admin",
            "account",
            "accounts",
            "accountname",
            "accountnamekey",
            "active",
            "api",

            //b


            //c
            "category",
            "categorykey",
            "categorization",
            "categorizations",

            //d
            "dateCreated",
            "documenttype",
            "default",

            //e
            
            //f
            "filepath",
            "ftp",
            "fullyqualifiedname",

            //g

            //h

            //i
            "id",
            "image",
            "images",

            //j

            //k

            //l
            "locationpath",
            "locations",

            //m
            "metadata",
            "locationmetadata",

            //n
            "name",
            "namekey",
            "null",

            //o
            "orderid",
            "order",
            "orderby",
            "ordering",

            //p
            "properties",
            "property",
            "predefined",
            "productid",
            
            //q

            //r

            //s
            "selflink",
            "search",

            "sort",
            "sorting",
            "sort-by",
            "sortby",

            "subcategory",
            "subcategorykey",
            "subcategorization",

            "subsubcategory",
            "subsubcategorykey",
            "subsubcategorization",

            "subsubsubcategory",
            "subsubsubcategorykey",
            "subsubsubcategorization",

            //t
            "tags",
            "tag",
            "thumbnails",
            "thumbnail",
            "title",

            //u


            //v
            "visible"


            //w


            //x

            //y

            //z

            #endregion

        });
    }
}
