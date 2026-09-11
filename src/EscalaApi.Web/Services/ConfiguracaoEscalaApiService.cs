using System.Net.Http.Json;
using EscalaApi.Web.Models;

namespace EscalaApi.Web.Services;

public class ConfiguracaoEscalaApiService
{
    private readonly HttpClient _http;

    public ConfiguracaoEscalaApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ConfiguracaoEscalaModel>> ListarAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<List<ConfiguracaoEscalaModel>>("/configuracoes-escala");
            return response ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao listar configurações de escala: {ex.Message}");
            return [];
        }
    }

    public async Task<ConfiguracaoEscalaModel?> ObterPorIdAsync(int id)
    {
        try
        {
            return await _http.GetFromJsonAsync<ConfiguracaoEscalaModel>($"/configuracoes-escala/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter configuração {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> InserirAsync(ConfiguracaoEscalaRequest request)
    {
        var response = await _http.PostAsJsonAsync("/configuracoes-escala", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AtualizarAsync(int id, ConfiguracaoEscalaRequest request)
    {
        var response = await _http.PutAsJsonAsync($"/configuracoes-escala/{id}", request);
        return response.IsSuccessStatusCode;
    }
}
