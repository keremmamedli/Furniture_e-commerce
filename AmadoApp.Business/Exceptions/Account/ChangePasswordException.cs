using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Exceptions.Account
{
	public class ChangePasswordException : Exception
	{
		public string ParamName { get; set; }
		public ChangePasswordException(string? message, string? paramName = null) : base(message)
		{
			ParamName = paramName ?? string.Empty;
		}
	}
}