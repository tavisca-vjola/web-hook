using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_Cache.DataAccessLayer.Model;
using FluentValidation;

namespace Api_Cache.ServicesLayer
{
    public class ManagerValidationThroughFluent:AbstractValidator<Managers>
    {
        public ManagerValidationThroughFluent()
        {
            RuleFor(Manager => Manager).Must(Manager => Manager.Mid >= 0).WithMessage("Manager id cannot be Zero");
            RuleFor(Manager => Manager.MName).NotNull().WithMessage("No Name Entered").DependentRules(() =>
            {
                RuleFor(Manager => Manager).Cascade(CascadeMode.StopOnFirstFailure).NotNull()
                .Must(Manager => Manager.MName.Length > 0 && Manager.MName.All(X => char.IsLetter(X) || X == '.' || X ==' '
                  )).WithMessage("Name should Contain Only Alphabets");
            });
            RuleFor(Manager => Manager).Must(Manager => Manager.Mage >= 0).WithMessage("Manager Age cannot be Zero");
            RuleFor(Manager => Manager.Employee).NotNull().WithMessage("No Employee Record Found ").DependentRules(() =>
                  {
                      RuleFor(Manager => Manager).Cascade(CascadeMode.StopOnFirstFailure).NotNull()
                      .Must(Manager => Manager.Employee.Eid > 0).WithMessage("Eid cannot be zero");
                  });
            RuleFor(Manager => Manager.Employee).NotNull().WithMessage("No Employee Record Found ").DependentRules(() =>
            {
                RuleFor(Manager => Manager).Cascade(CascadeMode.StopOnFirstFailure).NotNull()
                .Must(Manager => Manager.Employee.Ename.Length > 0&& Manager.Employee.Ename.All(X => char.IsLetter(X) || X == '.' || X == ' '
                  )).WithMessage("Name should Contain Only Alphabets");
        });
            RuleFor(Manager => Manager.Employee).NotNull().WithMessage("No Employee Record Found ").DependentRules(() =>
            {
                RuleFor(Manager => Manager).Cascade(CascadeMode.StopOnFirstFailure).NotNull()
                .Must(Manager => Manager.Employee.Eage > 0).WithMessage("Eage cannot be zero");
            });
        }
    }
}
