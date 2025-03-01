﻿using System.Linq.Expressions;
using Orders.Application.Helpers;
using Orders.Application.Interfaces;
using Orders.Domain.Models;

namespace Orders.Application.Specifications;

public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
{
    protected BaseSpecification(Expression<Func<T, bool>>? criteria = null)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>>? Criteria { get; }
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    public List<string> ThenIncludes { get; } = new List<string>();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPaginationEnabled { get; private set; }

    protected void AddInclude(Expression<Func<T, object?>> include)
    {
        Includes.Add(include!);
    }

    // Can only be done with string path when using ThenInclude to avoid dependency on EF Core
    protected void AddThenInclude(string thenIncludePath)
    {
        ThenIncludes.Add(thenIncludePath);
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderBy)
    {
        OrderBy = orderBy;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescending)
    {
        OrderByDescending = orderByDescending;
    }

    protected void ApplyPagination(int size, int index)
    {
        var paging = PaginationOffset.Calculate(size, index);

        Skip = paging.Item1;
        Take = paging.Item2;
        IsPaginationEnabled = true;
    }
}