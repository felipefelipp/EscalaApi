using EscalaApi.Data.Entities;
using EscalaApi.Services.Rotacao.Models;

namespace EscalaApi.Services.Rotacao;

/// <summary>
/// Acumula atribuições geradas no preview e warnings de slots sem candidatos elegíveis.
/// </summary>
public sealed class LoteDeEscalas
{
    private readonly List<Escala> _escalas = [];
    private readonly List<EscalaWarning> _warnings = [];

    public IReadOnlyList<Escala> Escalas => _escalas;
    public IReadOnlyList<EscalaWarning> Warnings => _warnings;

    public bool JaOcupado(DateTime data, int tipoId) =>
        _escalas.Any(e => e.Data.Date == data.Date && e.TipoEscala == tipoId);

    public void Adicionar(Escala escala) => _escalas.Add(escala);

    public void AdicionarWarning(DateTime data, int tipoId, string? tipoNome = null)
    {
        var dia = ObterNomeDia(data.DayOfWeek);
        var tipo = tipoNome ?? tipoId.ToString();
        _warnings.Add(new EscalaWarning
        {
            Data = data.Date,
            TipoIntegranteId = tipoId,
            Mensagem = $"Nenhum integrante elegível para tipo '{tipo}' na {dia}"
        });
    }

    public IEnumerable<Escala> TodasAsEscalas(IEnumerable<Escala> historico) =>
        historico.Concat(_escalas);

    private static string ObterNomeDia(DayOfWeek dia) => dia switch
    {
        DayOfWeek.Sunday => "Domingo",
        DayOfWeek.Monday => "Segunda-feira",
        DayOfWeek.Tuesday => "Terça-feira",
        DayOfWeek.Wednesday => "Quarta-feira",
        DayOfWeek.Thursday => "Quinta-feira",
        DayOfWeek.Friday => "Sexta-feira",
        DayOfWeek.Saturday => "Sábado",
        _ => dia.ToString()
    };
}
