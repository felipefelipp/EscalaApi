using EscalaApi.Data.DTOs;

namespace EscalaApi.Repositories.Interfaces;

public interface IIntegranteDiasDisponiveisRepository
{
    Task<bool> InserirDiasDisponiveis(IntegranteDto diasDisponiveisDto);
    Task<bool> AtualizarDiasDisponiveis(IntegranteDto diasDisponiveisDto);

}