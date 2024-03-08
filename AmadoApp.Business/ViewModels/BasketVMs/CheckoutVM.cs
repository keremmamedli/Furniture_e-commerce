using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.ViewModels.BasketVMs
{
    public class CheckoutVM
    {
        public string FullName { get; set; }
        public string CardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string CVV { get; set; }
    }

    public class CheckoutVMValidator : AbstractValidator<CheckoutVM>
    {
        public CheckoutVMValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Please enter your full name.");

            RuleFor(x => x.CardNumber)
                .NotEmpty()
                .WithMessage("Please enter your card number.");

            RuleFor(x => x.Month)
                .NotEmpty()
                .WithMessage("Please select the card expiration month.");

            RuleFor(x => x.Year)
                .NotEmpty()
                .WithMessage("Please select the card expiration year.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Please enter your phone number");

            RuleFor(x => x.CVV)
                .NotEmpty()
                .WithMessage("Please enter your card CVV.");
        }
    }

}
