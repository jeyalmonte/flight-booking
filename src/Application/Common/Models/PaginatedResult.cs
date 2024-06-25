﻿using Microsoft.EntityFrameworkCore;

namespace Application.Common.Models;

public readonly record struct PaginatedResult<T>
{
	public IReadOnlyCollection<T> Items { get; }
	public int PageNumber { get; }
	public int TotalPages { get; }
	public int TotalCount { get; }

	private PaginatedResult(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
	{
		PageNumber = pageNumber;
		TotalPages = (int)Math.Ceiling(count / (double)pageSize);
		TotalCount = count;
		Items = items;
	}

	public bool HasPreviousPage => PageNumber > 1;
	public bool HasNextPage => PageNumber < TotalPages;


	public static async Task<PaginatedResult<TSource>> CreateAsync<TSource>(IQueryable<TSource> source, int pageNumber, int pageSize,
		CancellationToken cancellationToken = default)
	{
		var count = await source
			.CountAsync(cancellationToken);

		var items = await source
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new PaginatedResult<TSource>(items, count, pageNumber, pageSize);
	}

}

