using EscalaApi.Services.Rotacao.Estrategias;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Fornece a implementação da estratégia de rotação (padrão: contextual por dia da semana).
/// </summary>
public sealed class ResolvedorEstrategia
{
    private readonly ContextualPorDiaSemana _contextual = new();
    private readonly Global _global = new();

    public IEstrategiaContagem Resolver(string? codigo = null) => codigo switch
    {
        "global" => _global,
        _ => _contextual
    };

    public IEstrategiaContagem ResolverPorId(int idEstrategiaAlgoritmo) => idEstrategiaAlgoritmo switch
    {
        2 => _global,
        _ => _contextual
    };
}
