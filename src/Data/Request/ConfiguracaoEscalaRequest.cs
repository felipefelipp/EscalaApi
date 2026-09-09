namespace EscalaApi.Data.Request;

/// <summary>Payload para criar ou atualizar uma configuração de escala.</summary>
public class ConfiguracaoEscalaRequest
{
    /// <summary>Nome amigável da configuração (ex.: "Q1 2026").</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Início do período em que as datas serão expandídas.</summary>
    public DateTime DataInicio { get; set; }

    /// <summary>Fim do período. O intervalo é limitado por GET/PUT /parametros/range-maximo.</summary>
    public DateTime DataFim { get; set; }

    /// <summary>
    /// Dias (ou slots) em que a escala se repete no período.
    /// Use DayOfWeek: 0=Domingo, 1=Segunda, 2=Terça, 3=Quarta, 4=Quinta, 5=Sexta, 6=Sábado.
    /// Ex.: [3, 0] = quartas e domingos. Obrigatório pelo menos um valor.
    /// </summary>
    public List<int> ValoresRecorrentes { get; set; } = [];

    /// <summary>
    /// IDs dos tipos de integrante que participam desta escala.
    /// Consulte GET /tipos-integrante. Obrigatório pelo menos um.
    /// </summary>
    public List<int> TiposIntegrante { get; set; } = [];
}
