using System.Text.Json.Serialization;

namespace JusMonitor.Infrastructure.ExternalServices.DataJud;

public record DataJudApiResponse(
    [property: JsonPropertyName("hits")] DataJudHits Hits
);

public record DataJudHits(
    [property: JsonPropertyName("hits")] IEnumerable<DataJudHit> Hits
);

public record DataJudHit(
    [property: JsonPropertyName("_source")] DataJudSource Source
);

public record DataJudSource(
    [property: JsonPropertyName("numeroProcesso")] string NumeroProcesso,
    [property: JsonPropertyName("tribunal")] string Tribunal,
    [property: JsonPropertyName("orgaoJulgador")] DataJudOrgaoJulgador? OrgaoJulgador,
    [property: JsonPropertyName("assuntos")] IEnumerable<DataJudAssunto>? Assuntos,
    [property: JsonPropertyName("movimentos")] IEnumerable<DataJudMovimento> Movimentos
)
{
    public string? Vara => OrgaoJulgador?.Nome;
    public string? Assunto => Assuntos?.FirstOrDefault()?.Nome;
}

public record DataJudOrgaoJulgador(
    [property: JsonPropertyName("nome")] string Nome
);

public record DataJudAssunto(
    [property: JsonPropertyName("nome")] string Nome
);

public record DataJudMovimento(
    [property: JsonPropertyName("nome")] string Nome,
    [property: JsonPropertyName("dataHora")] DateTime DataHora,
    [property: JsonPropertyName("codigo")] string? Codigo
);