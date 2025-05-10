using EscalaApi.Utils.Enums;

namespace EscalaApi.Data.Entities;

public record EscalaIntegrante(
    int idIntegrante,
    DateTime Data,
    TipoEscala TipoEscala);