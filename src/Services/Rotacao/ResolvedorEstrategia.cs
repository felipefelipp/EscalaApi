using EscalaApi.Data.Entities;
using EscalaApi.Services.Rotacao.Estrategias;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Resolve a implementação de estratégia pelo código cadastrado em <c>estrategia_algoritmo</c>.
/// </summary>
public sealed class ResolvedorEstrategia
{
    private readonly ContextualPorDiaSemana _contextual = new();
    private readonly Global _global = new();

    public IEstrategiaContagem Resolver(string codigo) => codigo switch
    {
        "contextual_dia_semana" => _contextual,
        "global" => _global,
        _ => throw new ArgumentException($"Estratégia de algoritmo desconhecida: '{codigo}'.", nameof(codigo))
    };

    public IEstrategiaContagem ResolverPorId(int idEstrategiaAlgoritmo) => idEstrategiaAlgoritmo switch
    {
        1 => _contextual,
        2 => _global,
        _ => throw new ArgumentException($"Estratégia de algoritmo desconhecida: id {idEstrategiaAlgoritmo}.", nameof(idEstrategiaAlgoritmo))
    };

    public ContextoRotacao ObterContextoDesempate(IEstrategiaContagem estrategia, int tipoId, DateTime data) =>
        estrategia switch
        {
            ContextualPorDiaSemana contextual => contextual.ObterContexto(tipoId, data),
            Global global => global.ObterContexto(tipoId),
            _ => ContextoRotacao.PorTipo(tipoId)
        };
}
