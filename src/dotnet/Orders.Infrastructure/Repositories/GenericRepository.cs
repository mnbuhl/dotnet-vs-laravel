﻿using Microsoft.EntityFrameworkCore;
using Orders.Application.Interfaces;
using Orders.Domain.Models;
using Orders.Infrastructure.Data;

namespace Orders.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T?> Get(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetWithSpecification(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> List()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> ListWithSpecification(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<bool> Create(T entity)
    {
        _context.Set<T>().Add(entity);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Detached;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Set<T>().Update(entity);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);

        if (entity == null)
            return false;

        _context.Set<T>().Remove(entity);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<int> Count(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
}