﻿using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class FaceRegisResult
    {
        public string? Url { get; set; }
        public StatusFaceTurn StatusFaceTurn { get; set; }
        public string? Descriptor { get; set; }
    }
}