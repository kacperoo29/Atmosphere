using System.ComponentModel.DataAnnotations;

namespace Atmosphere.Core;

public class PagedList<T>
{
    [Required]
    public int CurrentPage { get; init; }

    [Required]
    public int TotalPages { get; init; }

    [Required]
    public int PageSize { get; init; }

    [Required]
    public int TotalCount { get; init; }

    [Required]
    public IEnumerable<T> Items { get; init; }



    [Required]
    public bool HasPrevious => CurrentPage > 1;

    [Required]
    public bool HasNext => CurrentPage < TotalPages;
}
