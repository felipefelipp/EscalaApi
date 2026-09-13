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

    public async Task<(bool Sucesso, string? MensagemErro)> ExcluirAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"/configuracoes-escala/{id}");
            if (response.IsSuccessStatusCode)
                return (true, null);

            var conteudo = await response.Content.ReadAsStringAsync();
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(conteudo);
                if (doc.RootElement.TryGetProperty("erros", out var errosProp) &&
                    errosProp.ValueKind == System.Text.Json.JsonValueKind.Array &&
                    errosProp.GetArrayLength() > 0)
                {
                    var primeiro = errosProp[0];
                    if (primeiro.ValueKind == System.Text.Json.JsonValueKind.Object &&
                        primeiro.TryGetProperty("message", out var msgProp))
                    {
                        return (false, msgProp.GetString());
                    }
                    if (primeiro.ValueKind == System.Text.Json.JsonValueKind.String)
                    {
                        return (false, primeiro.GetString());
                    }
                }
            }
            catch
            {
                // Fallback se formato não for JSON esperado
            }

            return (false, "Não foi possível excluir a configuração de escala.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao excluir configuração {id}: {ex.Message}");
            return (false, ex.Message);
        }
    }
}
