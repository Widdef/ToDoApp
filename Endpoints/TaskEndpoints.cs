using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;
using ToDoApp.DTOs;

namespace ToDoApp.Endpoints;

public static class TasksEndpoints
{
    // metoda rozszerzająca
    public static RouteGroupBuilder MapTasksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks");

        group.MapGet("/", async ([AsParameters] TaskQuery q, TaskDbContext db) =>
        {
            var query = db.Tasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q.Q))
                query = query.Where(t => EF.Functions.Like(t.Title, $"%{q.Q}%"));

            if (q.Completed.HasValue)
                query = query.Where(t => t.IsCompleted == q.Completed.Value);

            if (q.From.HasValue) query = query.Where(t => t.DueDate >= q.From.Value);
            if (q.To.HasValue)   query = query.Where(t => t.DueDate <= q.To.Value);

            var desc = q.Order?.ToLower() == "desc";
            query = q.SortBy?.ToLower() switch
            {
                "title"   => desc ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
                "duedate" => desc ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
                _         => desc ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt)
            };

            var size = Math.Clamp(q.PageSize ?? 20, 1, 100);
            var page = Math.Max(q.Page ?? 1, 1);
            var skip = (page - 1) * size;

            var total = await query.CountAsync();
            var items = await query.Skip(skip).Take(size).ToListAsync();

            return TypedResults.Ok(new { total, items });
        });

        group.MapGet("/{id:int}", async (int id, TaskDbContext db) =>
            await db.Tasks.FindAsync(id) is { } t ? Results.Ok(t) : Results.NotFound());

        group.MapPost("/", async (TaskItem dto, TaskDbContext db) =>
        {
            db.Tasks.Add(dto);
            await db.SaveChangesAsync();
            return Results.Created($"/api/tasks/{dto.Id}", dto);
        });

        group.MapPut("/{id:int}", async (int id, TaskItem dto, TaskDbContext db) =>
        {
            var t = await db.Tasks.FindAsync(id);
            if (t is null) return Results.NotFound();

            // Nadpisujemy wszystkie modyfikowalne pola encji
            t.Title = dto.Title;
            t.IsCompleted = dto.IsCompleted;
            t.DueDate = dto.DueDate;
            t.Description = dto.Description;
            t.Priority = dto.Priority;
            t.Category = dto.Category;
            t.EstimatedTimeMinutes = dto.EstimatedTimeMinutes;

            // UWAGA: zwykle CreatedAt zostawiamy bez zmian
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // --- częściowa aktualizacja (PATCH) ---
        group.MapPatch("/{id:int}", async (int id, UpdateTaskDto dto, TaskDbContext db) =>
        {
            var t = await db.Tasks.FindAsync(id);
            if (t is null) return Results.NotFound();

            if (dto.Title is not null) t.Title = dto.Title;
            if (dto.IsCompleted.HasValue) t.IsCompleted = dto.IsCompleted.Value;
            if (dto.DueDate.HasValue) t.DueDate = dto.DueDate;
            if (dto.Description is not null) t.Description = dto.Description;
            if (dto.Priority.HasValue) t.Priority = dto.Priority.Value;
            if (dto.Category is not null) t.Category = dto.Category;
            if (dto.EstimatedTimeMinutes.HasValue) t.EstimatedTimeMinutes = dto.EstimatedTimeMinutes.Value;

            await db.SaveChangesAsync();
            return Results.Ok(t); // zwracamy zaktualizowany obiekt
        });

        group.MapDelete("/{id:int}", async (int id, TaskDbContext db) =>
        {
            var t = await db.Tasks.FindAsync(id);
            if (t is null) return Results.NotFound();
            db.Tasks.Remove(t);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return group;
    }
}

