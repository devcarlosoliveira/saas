using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Saas.Web.Models;

namespace Saas.Web.Data.Interceptors;

/// <summary>
/// Interceptor para atualizar automaticamente campos de auditoria
/// </summary>
public class AuditInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditFields(DbContext? context)
    {
        if (context == null)
            return;

        var entries = context.ChangeTracker.Entries<BaseEntity>();
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.DataCriacao = now;
                    break;

                case EntityState.Modified:
                    entry.Entity.DataAtualizacao = now;
                    break;
            }

            // Soft delete handling
            if (entry.Entity is AuditableEntity auditableEntity)
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    auditableEntity.Ativo = false;
                    auditableEntity.DataExclusao = now;
                }
            }
        }
    }
}
