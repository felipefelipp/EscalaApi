using System.Net.Http.Json;
using EscalaApi.Web.Models;

namespace EscalaApi.Web.Services;

public class TipoIntegranteApiService
{
    private readonly HttpClient _http;

    public TipoIntegranteApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<TipoIntegranteModel>> ListarAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<List<TipoIntegranteModel>>("/tipos-integrante");
            return response ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao listar tipos de integrante: {ex.Message}");
            return [];
        }
    }

    public async Task<bool> InserirAsync(TipoIntegranteRequest request)
    {
        var response = await _http.PostAsJsonAsync("/tipos-integrante", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AtualizarAsync(int id, TipoIntegranteRequest request)
    {
        var response = await _http.PutAsJsonAsync($"/tipos-integrante/{id}", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var response = await _http.DeleteAsync($"/tipos-integrante/{id}");
        return response.IsSuccessStatusCode;
    }
}
