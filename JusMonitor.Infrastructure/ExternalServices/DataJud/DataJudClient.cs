using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using JusMonitor.Application.Common.Interfaces;

namespace JusMonitor.Infrastructure.ExternalServices.DataJud;

public class DataJudClient(HttpClient httpClient) : IDataJudService
{
    private const string IndicesTJSP = "api_publica_tjsp";

    public async Task<DataJudProcessoResult?> ConsultarProcessoAsync(
        string numeroProcesso,
        CancellationToken ct = default)
    {
        var numeroLimpo = numeroProcesso.Replace("-", "").Replace(".", "");

        var query = new
        {
            query = new
            {
                match = new
                {
                    numeroProcesso = numeroLimpo
                }
            }
        };

        var json = JsonSerializer.Serialize(query);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync($"{IndicesTJSP}/_search", content, ct);

        if (!response.IsSuccessStatusCode)
            return null;

        var rawJson = await response.Content.ReadAsStringAsync(ct);

        var result = JsonSerializer.Deserialize<DataJudApiResponse>(rawJson);

        var total = result?.Hits?.Total?.Value ?? 0;
        if (total == 0)
            return null;

        var source = result?.Hits?.Hits?.FirstOrDefault()?.Source;
        if (source is null)
            return null;

        return new DataJudProcessoResult(
            source.NumeroProcesso,
            source.Tribunal,
            source.Vara,
            source.Assunto,
            (source.Movimentos ?? []).Select(m => new DataJudMovimentacaoResult(
                m.Nome,
                m.DataHora,
                m.Codigo)));
    }
}