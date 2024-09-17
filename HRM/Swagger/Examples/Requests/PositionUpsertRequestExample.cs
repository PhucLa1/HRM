﻿using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class PositionUpsertRequestExample : IExamplesProvider<PositionUpsert>
    {
        public PositionUpsert GetExamples()
        {
           return new PositionUpsert { Name = "Software Engineer" };
        }
    }
}