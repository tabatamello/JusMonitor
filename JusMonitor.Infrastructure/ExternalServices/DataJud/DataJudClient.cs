using System.Net.Http.Json;
using JusMonitor.Application.Common.Interfaces;

namespace JusMonitor.Infrastructure.ExternalServices.DataJud;

public class DataJudClient(HttpClient httpClient) : IDataJudService
{
    public async Task<DataJudProcessoResult?> ConsultarProcessoAsync(string numeroProcesso, CancellationToken ct = default)
    {
        var query = new
        {
            query = new
            {
                match = new { numeroProcesso }
            }
        };

        var response = await httpClient.PostAsJsonAsync(
            "api_publica_tjsp/_search", query, ct);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<DataJudApiResponse>(ct);
        var source = result?.Hits?.Hits?.FirstOrDefault()?.Source;

        if (source is null)
            return null;

        return new DataJudProcessoResult(
            source.NumeroProcesso,
            source.Tribunal,
            null,
            null,
            source.Movimentos.Select(m => new DataJudMovimentacaoResult(
                m.Nome,
                m.DataHora,
                m.Codigo)));
    }
}