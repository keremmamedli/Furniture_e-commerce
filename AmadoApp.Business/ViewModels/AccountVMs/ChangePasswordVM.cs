using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.ViewModels.AccountVMs
{
	public class ChangePasswordVM
	{
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }
		[DataType(DataType.Password), Compare(nameof(NewPassword))]
		public string ConfirmPassword { get; set; }
	}
	public class ChangePasswordVMValidator : AbstractValidator<ChangePasswordVM>
	{
		public ChangePasswordVMValidator()
		{

			RuleFor(x => x.NewPassword)
					.NotEmpty()
					.WithMessage("Please enter your new password.");

			RuleFor(x => x.ConfirmPassword)
				.NotEmpty()
				.WithMessage("Please enter your confirm password.")
				.Equal(x => x.NewPassword)
				.WithMessage("Password and Confirm Password do not match.");
		}
	}
}