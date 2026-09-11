namespace EscalaApi.Web.Models;

public class EscalaResponseModel
{
    public int IdEscala { get; set; }
    public DateTime? Data { get; set; }
    public string Dia { get; set; } = string.Empty;
    public int? IdIntegrante { get; set; }
    public string? NomeIntegrante { get; set; }
    public int CodigoTipoEscala { get; set; }
    public string? DescricaoTipoEscala { get; set; }
}

public class EscalaResultWrapper<T>
{
    public bool Sucess { get; set; }
    public T? Object { get; set; }
    public int StatusCode { get; set; }
    public List<NotificationModel> Notifications { get; set; } = [];
}

public class NotificationModel
{
    public string? Key { get; set; }
    public string? Message { get; set; }
}

public class EscalaFiltroModel
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 100;
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int? TipoIntegrante { get; set; }
    public int? IdIntegrante { get; set; }
}

public class GerarEscalaRequest
{
    public int ConfiguracaoEscalaId { get; set; }
    public bool ImpedirMultiplosTiposMesmoDia { get; set; } = true;
    public bool Persistir { get; set; } = false;
}

public class ResultadoPreviewModel
{
    public List<EscalaItemModel> Escalas { get; set; } = [];
    public List<BalanceamentoItemModel> Balanceamento { get; set; } = [];
    public List<EscalaWarningModel> Warnings { get; set; } = [];
}

public class EscalaItemModel
{
    public DateTime Data { get; set; }
    public string DiaDaSemana { get; set; } = string.Empty;
    public int TipoEscala { get; set; }
    public IntegranteModel Integrante { get; set; } = new();
}

public class BalanceamentoItemModel
{
    public int TipoIntegranteId { get; set; }
    public string? TipoIntegranteNome { get; set; }
    public string? DiaSemana { get; set; }
    public List<ContagemIntegranteModel> Contagens { get; set; } = [];
    public int DesvioMaximo { get; set; }
}

public class ContagemIntegranteModel
{
    public int IntegranteId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int TotalEscalado { get; set; }
}

public class EscalaWarningModel
{
    public DateTime Data { get; set; }
    public int TipoIntegranteId { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}
