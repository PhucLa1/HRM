using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Results
{
	public class WebResult
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? WebApi { get; set; }
	}
}
