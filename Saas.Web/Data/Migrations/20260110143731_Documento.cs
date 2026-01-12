using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saas.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class Documento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<string>(type: "TEXT", nullable: false),
                    TextoOriginal = table.Column<string>(type: "TEXT", nullable: false),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(
                        type: "TEXT",
                        nullable: false,
                        defaultValue: "Processando"
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documentos_AppUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "PromptTemplates",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    Template = table.Column<string>(type: "TEXT", nullable: false),
                    Parametros = table.Column<string>(type: "TEXT", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    CriadoPor = table.Column<string>(type: "TEXT", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromptTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromptTemplates_AppUsers_CriadoPor",
                        column: x => x.CriadoPor,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Processamentos",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocumentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    DataProcessamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TextoResultante = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(
                        type: "TEXT",
                        nullable: false,
                        defaultValue: "Pendente"
                    ),
                    ProcessamentoAnteriorId = table.Column<int>(type: "INTEGER", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processamentos_Documentos_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "Documentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_Processamentos_Processamentos_ProcessamentoAnteriorId",
                        column: x => x.ProcessamentoAnteriorId,
                        principalTable: "Processamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "ProcessamentoTagIdentificada",
                columns: table => new
                {
                    ProcessamentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false),
                    Confianca = table.Column<decimal>(type: "TEXT", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_ProcessamentoTagIdentificada",
                        x => new { x.ProcessamentoId, x.TagId }
                    );
                    table.ForeignKey(
                        name: "FK_ProcessamentoTagIdentificada_Processamentos_ProcessamentoId",
                        column: x => x.ProcessamentoId,
                        principalTable: "Processamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_ProcessamentoTagIdentificada_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "ProcessamentoTagSelecionada",
                columns: table => new
                {
                    ProcessamentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false),
                    AcaoUsuario = table.Column<string>(type: "TEXT", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_ProcessamentoTagSelecionada",
                        x => new { x.ProcessamentoId, x.TagId }
                    );
                    table.ForeignKey(
                        name: "FK_ProcessamentoTagSelecionada_Processamentos_ProcessamentoId",
                        column: x => x.ProcessamentoId,
                        principalTable: "Processamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_ProcessamentoTagSelecionada_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "PromptExecucoes",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProcessamentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PromptTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    PromptEnviado = table.Column<string>(type: "TEXT", nullable: false),
                    RespostaIa = table.Column<string>(type: "TEXT", nullable: false),
                    Metadados = table.Column<string>(type: "TEXT", nullable: false),
                    DataExecucao = table.Column<DateTime>(type: "TEXT", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromptExecucoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromptExecucoes_Processamentos_ProcessamentoId",
                        column: x => x.ProcessamentoId,
                        principalTable: "Processamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_PromptExecucoes_PromptTemplates_PromptTemplateId",
                        column: x => x.PromptTemplateId,
                        principalTable: "PromptTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers",
                column: "Email",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_DataCriacao",
                table: "Documentos",
                column: "DataCriacao"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_Status",
                table: "Documentos",
                column: "Status"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_UsuarioId",
                table: "Documentos",
                column: "UsuarioId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Processamentos_DocumentoId",
                table: "Processamentos",
                column: "DocumentoId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Processamentos_ProcessamentoAnteriorId",
                table: "Processamentos",
                column: "ProcessamentoAnteriorId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Processamentos_Status",
                table: "Processamentos",
                column: "Status"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Processamentos_Tipo_DataProcessamento",
                table: "Processamentos",
                columns: new[] { "Tipo", "DataProcessamento" }
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProcessamentoTagIdentificada_TagId",
                table: "ProcessamentoTagIdentificada",
                column: "TagId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProcessamentoTagSelecionada_TagId",
                table: "ProcessamentoTagSelecionada",
                column: "TagId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_PromptExecucoes_DataExecucao",
                table: "PromptExecucoes",
                column: "DataExecucao"
            );

            migrationBuilder.CreateIndex(
                name: "IX_PromptExecucoes_ProcessamentoId",
                table: "PromptExecucoes",
                column: "ProcessamentoId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_PromptExecucoes_PromptTemplateId",
                table: "PromptExecucoes",
                column: "PromptTemplateId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_PromptTemplates_Ativo",
                table: "PromptTemplates",
                column: "Ativo"
            );

            migrationBuilder.CreateIndex(
                name: "IX_PromptTemplates_CriadoPor",
                table: "PromptTemplates",
                column: "CriadoPor"
            );

            migrationBuilder.CreateIndex(
                name: "IX_PromptTemplates_Nome",
                table: "PromptTemplates",
                column: "Nome",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Codigo",
                table: "Tags",
                column: "Codigo",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Nome",
                table: "Tags",
                column: "Nome",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ProcessamentoTagIdentificada");

            migrationBuilder.DropTable(name: "ProcessamentoTagSelecionada");

            migrationBuilder.DropTable(name: "PromptExecucoes");

            migrationBuilder.DropTable(name: "Tags");

            migrationBuilder.DropTable(name: "Processamentos");

            migrationBuilder.DropTable(name: "PromptTemplates");

            migrationBuilder.DropTable(name: "Documentos");

            migrationBuilder.DropIndex(name: "IX_AppUsers_Email", table: "AppUsers");
        }
    }
}
