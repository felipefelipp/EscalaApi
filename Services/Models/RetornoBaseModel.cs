using Flunt.Notifications;

namespace EscalaApi.Services.Models;

public class RetornoBaseModel<T>
{
    public T? Dados { get; set; }
}

public class RetornoSucessoPaginacaoModel<TMetadados, TDados>
{
    public TMetadados Metadados { get; set; }
    public TDados Dados { get; set; }
}

public class RetornoErroModel
{
    public List<Notification> Erros { get; set; } = new List<Notification>();
}

public class RetornoErroModel<T>
{
    public List<Notification> Erros { get; set; } = new List<Notification>();
}
