using JusMonitor.Domain.Common;
using JusMonitor.Domain.Enums;

namespace JusMonitor.Domain.Entities;

public class Processo : BaseEntity
{
    public string NumeroProcesso { get; private set; } = string.Empty;
    public string Tribunal { get; private set; } = string.Empty;
    public string? Vara { get; private set; }
    public string? Assunto { get; private set; }
    public StatusProcesso Status { get; private set; }
    public DateTime? UltimaConsulta { get; private set; }
    public Guid AdvogadoId { get; private set; }
    public Advogado Advogado { get; private set; } = null!;
    public Guid ClienteId { get; private set; }
    public Cliente Cliente { get; private set; } = null!;

    private readonly List<Movimentacao> _movimentacoes = [];
    public IReadOnlyCollection<Movimentacao> Movimentacoes => _movimentacoes.AsReadOnly();

    protected Processo() { }

    public static Processo Criar(string numeroProcesso, string tribunal, Guid advogadoId, Guid clienteId, string? vara = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(numeroProcesso);
        ArgumentException.ThrowIfNullOrWhiteSpace(tribunal);

        return new Processo
        {
            NumeroProcesso = numeroProcesso.Trim(),
            Tribunal = tribunal,
            Vara = vara,
            Status = StatusProcesso.Ativo,
            AdvogadoId = advogadoId,
            ClienteId = clienteId
        };
    }

    public void AdicionarMovimentacao(Movimentacao movimentacao)
    {
        _movimentacoes.Add(movimentacao);
        UltimaConsulta = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void AtualizarStatus(StatusProcesso novoStatus)
    {
        Status = novoStatus;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void RegistrarConsulta()
    {
        UltimaConsulta = DateTime.UtcNow;
    }
}