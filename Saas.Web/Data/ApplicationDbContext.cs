using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Saas.Web.Models;

namespace Saas.Web.Data;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>,
        IDataProtectionKeyContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public DbSet<Documento> Documentos { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Processamento> Processamentos { get; set; }
    public DbSet<ProcessamentoTagIdentificada> ProcessamentoTagIdentificadas { get; set; }
    public DbSet<ProcessamentoTagSelecionada> ProcessamentoTagSelecionadas { get; set; }
    public DbSet<PromptTemplate> PromptTemplates { get; set; }
    public DbSet<PromptExecucao> PromptExecucoes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Renomear todas as tabelas do Identity
        builder.Entity<ApplicationUser>().ToTable("AppUsers");
        builder.Entity<ApplicationRole>().ToTable("AppRoles");
        builder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens");
        // Renomear a tabela do DataProtectionKey
        builder.Entity<DataProtectionKey>().ToTable("AppDataProtectionKeys");

        // Configurações de chave composta
        builder
            .Entity<ProcessamentoTagIdentificada>()
            .HasKey(pti => new { pti.ProcessamentoId, pti.TagId });

        builder
            .Entity<ProcessamentoTagSelecionada>()
            .HasKey(pts => new { pts.ProcessamentoId, pts.TagId });

        // Índices para performance
        builder.Entity<ApplicationUser>().HasIndex(u => u.Email).IsUnique();

        builder.Entity<Tag>().HasIndex(t => t.Nome).IsUnique();
        builder.Entity<Tag>().HasIndex(t => t.Codigo).IsUnique();

        builder.Entity<PromptTemplate>().HasIndex(pt => pt.Nome).IsUnique();
        builder.Entity<PromptTemplate>().HasIndex(pt => pt.Ativo);

        builder.Entity<Processamento>().HasIndex(p => new { p.Tipo, p.DataProcessamento });
        builder.Entity<Processamento>().HasIndex(p => p.Status);

        builder.Entity<Documento>().HasIndex(d => d.UsuarioId);
        builder.Entity<Documento>().HasIndex(d => d.Status);
        builder.Entity<Documento>().HasIndex(d => d.DataCriacao);

        builder.Entity<PromptExecucao>().HasIndex(pe => pe.ProcessamentoId);
        builder.Entity<PromptExecucao>().HasIndex(pe => pe.DataExecucao);

        // Relacionamentos
        // Usuario 1:N Documento
        builder
            .Entity<Documento>()
            .HasOne(d => d.Usuario)
            .WithMany(u => u.Documentos)
            .HasForeignKey(d => d.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Documento 1:N Processamento
        builder
            .Entity<Processamento>()
            .HasOne(p => p.Documento)
            .WithMany(d => d.Processamentos)
            .HasForeignKey(p => p.DocumentoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Self-referência Processamento
        builder
            .Entity<Processamento>()
            .HasOne(p => p.ProcessamentoAnterior)
            .WithMany(p => p.ProcessamentosPosteriores)
            .HasForeignKey(p => p.ProcessamentoAnteriorId)
            .OnDelete(DeleteBehavior.Restrict);

        // PromptTemplate -> Usuario (opcional)
        builder
            .Entity<PromptTemplate>()
            .HasOne(pt => pt.UsuarioCriador)
            .WithMany(u => u.TemplatesCriados)
            .HasForeignKey(pt => pt.CriadoPor)
            .OnDelete(DeleteBehavior.SetNull);

        // PromptExecucao -> Processamento e PromptTemplate
        builder
            .Entity<PromptExecucao>()
            .HasOne(pe => pe.Processamento)
            .WithMany(p => p.PromptExecucoes)
            .HasForeignKey(pe => pe.ProcessamentoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Entity<PromptExecucao>()
            .HasOne(pe => pe.PromptTemplate)
            .WithMany(pt => pt.Execucoes)
            .HasForeignKey(pe => pe.PromptTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        // Valor padrão para Status
        builder
            .Entity<Documento>()
            .Property(d => d.Status)
            .HasDefaultValue(Models.Enums.DocumentoStatus.Processando);

        builder
            .Entity<Processamento>()
            .Property(p => p.Status)
            .HasDefaultValue(Models.Enums.ProcessamentoStatus.Pendente);

        builder.Entity<Processamento>().Property(p => p.Tipo).HasConversion<string>();

        builder.Entity<Documento>().Property(d => d.Status).HasConversion<string>();

        builder.Entity<Processamento>().Property(p => p.Status).HasConversion<string>();

        builder
            .Entity<ProcessamentoTagSelecionada>()
            .Property(pts => pts.AcaoUsuario)
            .HasConversion<string>();
    }
}
