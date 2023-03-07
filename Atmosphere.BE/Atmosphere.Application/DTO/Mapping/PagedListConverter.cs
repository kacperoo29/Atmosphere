using Atmosphere.Core;
using AutoMapper;

namespace Atmosphere.Application.DTO.Mapping;

public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
{
    public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
    {
        return new PagedList<TDestination>
        {
            CurrentPage = source.CurrentPage,
            TotalPages = source.TotalPages,
            PageSize = source.PageSize,
            TotalCount = source.TotalCount,
            Items = context.Mapper.Map<IEnumerable<TDestination>>(source.Items)
        };
    }
}