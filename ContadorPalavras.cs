using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContadorPalavras
{
    public class ContadorParallel
    {
        public class TarefaContadorPalavra
        {
            public string[] Palavras { get; }
            public int Inicio { get; }
            public int Fim { get; }

            public TarefaContadorPalavra(string[] palavras, int inicio, int fim)
            {
                Palavras = palavras;
                Inicio = inicio;
                Fim = fim;
            }

            public int ContadorPalavras()
            {
                return Fim - Inicio; // Contando as palavras
            }
        }
    }
}
