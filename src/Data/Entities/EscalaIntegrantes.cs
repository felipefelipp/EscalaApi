using EscalaApi.Utils.Enums;

namespace EscalaApi.Data.Entities;

public record EscalaIntegrantes(
    DateTime DataInicio,
    DateTime DataFim,
    //List<int> IdIntegrantes, Possivel funcionalidade: Criar a escala com apenas os integrantes selecionados mesmo que haja disponibilidade para nãos selecionados
    List<TipoEscala> TipoEscala,
    List<DayOfWeek> DiasDaSemana);