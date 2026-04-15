using JusMonitor.Domain.Common;

namespace JusMonitor.Domain.Entities;

public class Cliente : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Telefone { get; private set; }
    public string? FcmToken { get; private set; }

    private readonly List<Processo> _processos = [];
    public IReadOnlyCollection<Processo> Processos => _processos.AsReadOnly();

    protected Cliente() { }

    public static Cliente Criar(string nome, string email, string? telefone = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nome);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return new Cliente
        {
            Nome = nome,
            Email = email,
            Telefone = telefone
        };
    }

    public void AtualizarFcmToken(string token)
    {
        FcmToken = token;
        AtualizadoEm = DateTime.UtcNow;
    }
}