using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;

namespace MinimalAPI.Estudandes;

public static class EstudantesRoutes
{
    public static void AddEstudantesRoutes(this WebApplication app)
    {
        var estudantesRoutes = app.MapGroup("estudantes");

        // get estudante by id
        estudantesRoutes.MapGet("{id:guid}", async (Guid id, AppDbContext context, CancellationToken ct) =>
        {
            var estudante = await context.Estudantes.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (estudante is null)
                return Results.NotFound();

            return Results.Ok(new EstudanteDto(estudante.Id, estudante.Nome));
        });
        
        // get all estudantes
        estudantesRoutes.MapGet("", async (AppDbContext context, CancellationToken ct) => 
            await context.Estudantes
                .Where(x => x.Ativo)
                .Select(x => new EstudanteDto(x.Id, x.Nome))
                .ToListAsync(ct));

        // create estudante
        estudantesRoutes.MapPost("", async (AddEstudanteRequest request, AppDbContext context, CancellationToken ct) =>
        {
            if (await context.Estudantes.AnyAsync(x => x.Nome == request.Nome, ct))
                return Results.Conflict($"Já existe um estudante com o nome {request.Nome}");
            
            var newEstudante = new Estudante(request.Nome);

            await context.Estudantes.AddAsync(newEstudante, ct);
            await context.SaveChangesAsync(ct);

            return Results.Ok(new EstudanteDto(newEstudante.Id, newEstudante.Nome));
        });

        estudantesRoutes.MapPut("{id:guid}",
        async (Guid id, UpdateEstudanteRequest request, AppDbContext context, CancellationToken ct) =>
        {
            var estudante = await context.Estudantes.SingleOrDefaultAsync(x => x.Id == id, ct);

            if (estudante is null)
                return Results.NotFound();
            
            estudante.UpdateNome(request.Nome);
            await context.SaveChangesAsync(ct);

            return Results.Ok(new EstudanteDto(estudante.Id, estudante.Nome));
        });

        estudantesRoutes.MapDelete("{id:guid}", async (Guid id, AppDbContext context, CancellationToken ct) =>
        {
            var estudante = await context.Estudantes.SingleOrDefaultAsync(x => x.Id == id, ct);

            if (estudante is null)
                return Results.NotFound();

            estudante.Disable();
            await context.SaveChangesAsync(ct);

            return Results.Ok();
        });
    }
}