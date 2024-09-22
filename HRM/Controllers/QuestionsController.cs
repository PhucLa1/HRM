﻿using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.Briefcase;
using HRM.Services.RecruitmentManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
	[ApiVersion(1)]
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = RoleExtensions.HR_ROLE)]
	public class QuestionsController : ControllerBase
	{
		private readonly IQuestionsService _questionService;
		public QuestionsController(IQuestionsService questionService)
		{
			_questionService = questionService;
		}


		/// <summary>
		/// Get all questions in company
		/// </summary>
		/// <response code="200">Return all the questions in the company in the metadata of api response</response>
		[HttpGet]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<QuestionResult>>))]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(QuestionResponseExample))]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _questionService.GetAllQuestion());
		}


		/// <summary>
		/// Add new question in the company
		/// </summary>
		/// <response code="200">Return the api response</response>
		[HttpPost]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> AddNew([FromBody] QuestionUpsert questionAdd)
		{
			return Ok(await _questionService.AddNewQuestion(questionAdd));
		}


		/// <summary>
		/// Update a question in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpPut]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> UpdateQuestion(int id, [FromBody] QuestionUpsert questionUpdate)
		{
			return Ok(await _questionService.UpdateQuestion(id, questionUpdate));
		}


		/// <summary>
		/// Delete a question in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> RemoveQuestion(int id)
		{
			return Ok(await _questionService.RemoveQuestion(id));
		}
	}
}