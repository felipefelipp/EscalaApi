using System.Net.Http.Json;
using EscalaApi.Web.Models;

namespace EscalaApi.Web.Services;

public class IntegranteApiService
{
    private readonly HttpClient _http;

    public IntegranteApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<IntegranteListResponse?> ObterIntegrantesAsync(int skip = 0, int take = 100, string? nome = null, int? tipo = null)
    {
        try
        {
            if (take > 100) take = 100;
            if (take <= 0) take = 10;
            if (skip < 0) skip = 0;

            var queryParams = new List<string>
            {
                $"skip={skip}",
                $"take={take}"
            };

            if (!string.IsNullOrWhiteSpace(nome))
                queryParams.Add($"nome={Uri.EscapeDataString(nome)}");

            if (tipo.HasValue && tipo > 0)
                queryParams.Add($"tipoIntegrante={tipo.Value}");

            var url = $"/integrantes?{string.Join("&", queryParams)}";
            return await _http.GetFromJsonAsync<IntegranteListResponse>(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar integrantes: {ex.Message}");
            return null;
        }
    }

    public async Task<IntegranteModel?> ObterPorIdAsync(int id)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<EscalaResultWrapper<IntegranteModel>>($"/integrantes/{id}");
            return response?.Object;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar integrante {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> InserirAsync(IntegranteRequest request)
    {
        var response = await _http.PostAsJsonAsync("/integrantes", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AtualizarAsync(int id, IntegranteRequest request)
    {
        var response = await _http.PutAsJsonAsync($"/integrantes/{id}", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var response = await _http.DeleteAsync($"/integrantes/{id}");
        return response.IsSuccessStatusCode;
    }
}
