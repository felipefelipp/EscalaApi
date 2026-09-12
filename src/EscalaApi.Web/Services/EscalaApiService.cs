using System.Net.Http.Headers;
using System.Net.Http.Json;
using EscalaApi.Web.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace EscalaApi.Web.Services;

public class EscalaApiService
{
    private readonly HttpClient _http;

    public EscalaApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<EscalaResponseModel>> ObterEscalasAsync(EscalaFiltroModel? filtro = null)
    {
        try
        {
            var skip = filtro?.Skip ?? 0;
            var take = filtro?.Take ?? 100;
            if (take <= 0) take = 100;
            if (skip < 0) skip = 0;

            var query = new List<string>
            {
                $"Skip={skip}",
                $"Take={take}"
            };

            if (filtro?.DataInicio != null)
                query.Add($"DataInicio={filtro.DataInicio.Value:yyyy-MM-dd}");
            if (filtro?.DataFim != null)
                query.Add($"DataFim={filtro.DataFim.Value:yyyy-MM-dd}");
            if (filtro?.TipoIntegrante != null)
                query.Add($"TipoIntegrante={filtro.TipoIntegrante.Value}");
            if (filtro?.IdIntegrante != null)
                query.Add($"IdIntegrante={filtro.IdIntegrante.Value}");

            var url = "/escalas?" + string.Join("&", query);
            var response = await _http.GetFromJsonAsync<EscalaResultWrapper<List<EscalaResponseModel>>>(url);
            return response?.Object ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter escalas: {ex.Message}");
            return [];
        }
    }

    public async Task<ResultadoPreviewModel?> GerarPreviewAsync(GerarEscalaRequest request)
    {
        try
        {
            request.Persistir = false;
            var response = await _http.PostAsJsonAsync("/escalas/gerar", request);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ResultadoPreviewModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao gerar preview de escala: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> ConfirmarEPersistirAsync(GerarEscalaRequest request)
    {
        try
        {
            request.Persistir = true;
            var response = await _http.PostAsJsonAsync("/escalas/gerar", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao persistir escala: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ImportarCsvAsync(IBrowserFile file, bool substituirExistentes = false)
    {
        try
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            content.Add(fileContent, "file", file.Name);

            var url = $"/escalas/import-csv?substituirExistentes={substituirExistentes}";
            var response = await _http.PostAsync(url, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao importar CSV: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EditarEscalaAsync(int id, EditarEscalaModel model)
    {
        try
        {
            var payload = new
            {
                idIntegrante = model.IdIntegrante,
                data = model.Data ?? DateTime.Today,
                tipoEscala = model.TipoEscala
            };

            var response = await _http.PutAsJsonAsync($"/escalas/{id}", payload);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao editar escala {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ExcluirEscalaAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"/escalas/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao excluir escala {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ExcluirEscalasEmLoteAsync(List<int> ids)
    {
        try
        {
            var payload = new ExcluirEscalasRequest { Ids = ids };
            var response = await _http.PostAsJsonAsync("/escalas/excluir-lote", payload);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao excluir escalas em lote: {ex.Message}");
            return false;
        }
    }
}
