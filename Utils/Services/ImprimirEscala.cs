// using EscalaApi.Utils.Extensions;
//
// namespace EscalaApi.Utils.Services;
//
// public static class ImprimirEscala
// {
//     public static void ExportarEscala(List<Data.Entities.Escala> escalaMes, DateTime dataInicio, DateTime dataFim)
//     {
//         // Caminho do arquivo
//         string directoryPath = "/home/felipe/Desktop/Escala/EscalaLouvor/Logs";
//
//         // Verifica se a pasta 'Logs' existe; se não, cria
//         if (!Directory.Exists(directoryPath))
//         {
//             Directory.CreateDirectory(directoryPath);
//         }
//
//         // Caminho do arquivo
//         string filePath = Path.Combine(directoryPath, "escala.txt");
//
//         // Usando StreamWriter para salvar a escala no arquivo
//         using (StreamWriter writer = new StreamWriter(filePath))
//         {
//             writer.WriteLine("*********************************************************************************");
//             writer.WriteLine($"Escala louvor Ebenézer - Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}");
//             writer.WriteLine("*********************************************************************************");
//
//             // Agrupa a escala por data e dia da semana
//             var agrupamento = escalaMes
//                 .GroupBy(e => new { e.Data, e.NomeSemana })
//                 .OrderBy(g => g.Key.Data);
//
//             foreach (var grupo in agrupamento)
//             {
//                 writer.WriteLine($"{grupo.Key.Data:dd/MM/yyyy} - {grupo.Key.NomeSemana.ParaValor()}");
//
//                 foreach (var item in grupo)
//                 {
//                     writer.WriteLine($"{item.Integrante.TipoIntegrante}: {item.Integrante.Nome}");
//                 }
//
//                 writer.WriteLine(); // Linha em branco entre dias
//             }
//         }
//
//         Console.WriteLine($"Escala salva em: {filePath}");
//     }
// }
