using JusMonitor.Domain.Common;

namespace JusMonitor.Domain.Entities;

public class Advogado : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string NumeroOAB { get; private set; } = string.Empty;
    public string UfOAB { get; private set; } = string.Empty;
    public bool OABValidada { get; private set; }
    public DateTime? DataValidacaoOAB { get; private set; }
    public string SenhaHash { get; private set; } = string.Empty;

    private readonly List<Processo> _processos = [];
    public IReadOnlyCollection<Processo> Processos => _processos.AsReadOnly();

    protected Advogado() { }

    public static Advogado Criar(string nome, string email, string numeroOAB, string ufOAB, string senhaHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nome);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(numeroOAB);
        ArgumentException.ThrowIfNullOrWhiteSpace(ufOAB);

        return new Advogado
        {
            Nome = nome,
            Email = email,
            NumeroOAB = numeroOAB.ToUpper().Trim(),
            UfOAB = ufOAB.ToUpper().Trim(),
            SenhaHash = senhaHash
        };
    }

    public void ConfirmarValidacaoOAB()
    {
        OABValidada = true;
        DataValidacaoOAB = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }
}