using EscalaApi.Data.DTOs;

namespace EscalaApi.Repositories.Interfaces;

public interface IIntegranteDiasDisponiveisRepository
{
    Task<bool> InserirDiasDisponiveis(List<IntegranteDiasDisponiveisDto> diasDisponiveisDto);
}