using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        int[] contadorPalavras = { 100000, 500000, 1000000, 5000000 };

        foreach (int contagem in contadorPalavras)
        {
            Console.WriteLine($"\nNúmero total de palavras: {contagem}");
            double tempoTotalExecucao = 0;
            double memoriaTotalUsada = 0;

            for (int i = 1; i <= 10; i++)
            {
                // Gerando palavras de exemplo
                string[] palavras = GerarPalavras(contagem);

                // Forçando a coleta de lixo antes da medição
                GC.Collect();
                GC.WaitForPendingFinalizers();
                long memoriaAntes = GC.GetTotalMemory(true);

                Stopwatch Stopwatch = Stopwatch.StartNew();

                // Processamento das palavras usando TPL
                Task<int>[] tasks = new Task<int>[palavras.Length];
                for (int j = 0; j < palavras.Length; j++)
                {
                    int indice = j; // Captura do índice para uso na Task
                    tasks[j] = Task.Run(() => ProcessarPalavra(palavras[indice]));
                }

                // Aguardar todas as tarefas completarem
                Task.WaitAll(tasks);

                // Somando os resultados
                int totalCaracteresProcessados = tasks.Sum(task => task.Result);

                Stopwatch.Stop();


                long memoriaDepois = GC.GetTotalMemory(false);
                double memoriaUsada = (memoriaDepois - memoriaAntes) / (1024.0 * 1024.0); // MB

                tempoTotalExecucao += Stopwatch.Elapsed.TotalMilliseconds;
                memoriaTotalUsada += memoriaUsada;

                Console.WriteLine($"Iteração {i}: Tempo = {Stopwatch.Elapsed.TotalMilliseconds / 1000.0} s, Memória usada = {memoriaUsada} MB");
            }

            double tempoMedio = tempoTotalExecucao / 10;
            double memoriaMedia = memoriaTotalUsada / 10;

            Console.WriteLine($"Tempo médio de execução: {tempoMedio / 1000.0} s");
            Console.WriteLine($"Memória média usada: {memoriaMedia} MB");
        }
    }

    private static string[] GerarPalavras(int contagem)
    {
        return Enumerable.Range(0, contagem).Select(i => "Palavra" + i).ToArray();
    }

    private static int ProcessarPalavra(string palavra)
    {
        // Removendo pontuação e convertendo para minúsculas
        string palavraLimpa = new string(palavra.Where(char.IsLetter).ToArray()).ToLower();
        return palavraLimpa.Length; // Contando o número de caracteres
    }
}
