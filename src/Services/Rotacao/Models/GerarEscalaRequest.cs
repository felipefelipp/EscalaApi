namespace EscalaApi.Services.Rotacao.Models;

public sealed class GerarEscalaRequest
{
    public int ConfiguracaoEscalaId { get; set; }
    public bool ImpedirMultiplosTiposMesmoDia { get; set; } = true;
    public bool DesempateAleatorio { get; set; } = true;
    public bool Persistir { get; set; } = false;
}
