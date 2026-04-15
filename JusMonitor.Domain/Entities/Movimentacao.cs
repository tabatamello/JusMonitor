using JusMonitor.Domain.Common;
using System.Diagnostics;

namespace JusMonitor.Domain.Entities;

public class Movimentacao : BaseEntity
{
    public string Descricao { get; private set; } = string.Empty;
    public DateTime DataMovimentacao { get; private set; }
    public string? CodigoMovimento { get; private set; }
    public bool NotificacaoEnviada { get; private set; }
    public Guid ProcessoId { get; private set; }
    public Processo Processo { get; private set; } = null!;

    protected Movimentacao() { }

    public static Movimentacao Criar(string descricao, DateTime dataMovimentacao, Guid processoId, string? codigoMovimento = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(descricao);

        return new Movimentacao
        {
            Descricao = descricao,
            DataMovimentacao = dataMovimentacao,
            ProcessoId = processoId,
            CodigoMovimento = codigoMovimento
        };
    }

    public void MarcarNotificacaoEnviada()
    {
        NotificacaoEnviada = true;
        AtualizadoEm = DateTime.UtcNow;
    }
}