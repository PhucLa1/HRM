﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Results
{
	public class QuestionResult
	{
		public int Id { get; set; }
		public int TestId { get; set; }
		public string? QuestionText { get; set; }
		public double Point { get; set; }
	}
}