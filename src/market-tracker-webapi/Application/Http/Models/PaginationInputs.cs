using System.ComponentModel.DataAnnotations;
using market_tracker_webapi.Application.Repository.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace market_tracker_webapi.Application.Http.Models;

public class PaginationInputs
{
    [Range(1, int.MaxValue)] public int Page { get; set; } = 1;

    [Range(1, 100)] public int ItemsPerPage { get; set; } = 20;

    public SortByType? SortBy { get; set; }

    [BindNever] public int Skip => (Page - 1) * ItemsPerPage;
}