﻿namespace HRM.Repositories.Dtos.Results
{
    public class QuestionResult
	{
		public int Id { get; set; }
		public int TestId { get; set; }
		public string? TestName { get; set; }
		public string? QuestionText { get; set; }
	}
}
