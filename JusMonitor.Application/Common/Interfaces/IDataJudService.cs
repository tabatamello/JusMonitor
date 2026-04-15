namespace JusMonitor.Application.Common.Interfaces;

public interface IDataJudService
{
    Task<DataJudProcessoResult?> ConsultarProcessoAsync(string numeroProcesso, CancellationToken ct = default);
}

public record DataJudProcessoResult(
    string NumeroProcesso,
    string Tribunal,
    string? Vara,
    string? Assunto,
    IEnumerable<DataJudMovimentacaoResult> Movimentacoes
);

public record DataJudMovimentacaoResult(
    string Descricao,
    DateTime DataMovimentacao,
    string? CodigoMovimento
);