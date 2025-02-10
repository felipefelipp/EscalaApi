using Dapper;
using EscalaApi.Data;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Scripts;
using EscalaApi.Repositories.Interfaces;

namespace EscalaApi.Repositories;

public class IntegranteDiasDisponiveisRepository : IIntegranteDiasDisponiveisRepository
{
    public async Task<bool> InserirDiasDisponiveis(List<IntegranteDiasDisponiveisDto> diasDisponiveisDto)
    {
        try
        {
            const string insertResult = IntegranteDiasDisponiveisScripts.InserirIntegranteDiasDisponiveis;

            await using var connection = DatabaseContext.GetConnection();

            await connection.ExecuteAsync(insertResult, diasDisponiveisDto);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}