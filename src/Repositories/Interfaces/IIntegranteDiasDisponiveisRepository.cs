using EscalaApi.Data.DTOs;

namespace EscalaApi.Repositories.Interfaces;

public interface IIntegranteDiasDisponiveisRepository
{
    Task<bool> InserirDiasDisponiveis(List<IntegranteDto> diasDisponiveisDto);
    Task<bool> AtualizarDiasDisponiveis(List<IntegranteDto> diasDisponiveisDto);
    Task<bool> RemoverDiasDisponiveis(int idIntegrante);

}