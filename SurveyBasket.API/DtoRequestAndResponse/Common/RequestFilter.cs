﻿namespace SurveyBasket.API.DtoRequestAndResponse.Common
{
    public record RequestFilter
    {
        
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 5;
        public string? SearchValue { get; init; }
        public string? SortColumn { get; init; }
        public string? SortDirection { get; init; } = "ASC";

    }
}    
