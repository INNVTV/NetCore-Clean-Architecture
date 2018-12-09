using Core.Domain.Models.Entity.InsertModels;
using Core.Domain.Models.Entity.InsertModels.Validators;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Managers
{
    public class EntityManager
    {
        public ValidationResult InsertEntity(EntityInsertModel entity)
        {
            EntityValidator validator = new EntityValidator();
            ValidationResult result = validator.Validate(entity);

            return result;
        }
    }
}
