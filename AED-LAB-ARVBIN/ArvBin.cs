﻿using System;
using System.IO;
using System.Collections;

namespace AED
{
    #region Classes CNo e CArvBin - Árvore Binária
    class CNo
    {
        public int item;
        public CNo Esq, Dir;

        public CNo(int valorItem)
            : this(valorItem, null, null)
        {
        }

        public CNo(int valorItem, CNo NoEsq, CNo NoDir)
        {
            item = valorItem;
            Esq = NoEsq;
            Dir = NoDir;
        }

    }

    class CArvBin
    {
        string ArquivoLeitura;
        StreamReader arquivoLeitura;
        StreamWriter arquivoEscrita;
        int Quant = 0; // controla quantidade de elementos na árvore
        int Maior = -1; // controla qual o menor elemento contido na árvore
        int Menor = 999; // controla qual o maior elemento contido na árvore

        private CNo Raiz;

        public CArvBin()
        {
            Raiz = null;
        }

        public void Inserir(int item)
        {
            Insere(item, ref Raiz);
        }

        private void Insere(int item, ref CNo no)
        {
            // insere um item na arvore a partir da raiz
            if (no == null) // se estiver vazia
            {
                no = new CNo(item);
                no.Esq = no.Dir = null; // inic. filhos
            }
            else // se a arvore nao esta vazia
            {
                // se for menor
                if (item < no.item)
                    Insere(item, ref no.Esq); // tentar a esq.
                else
                    // se for maior
                    if (item > no.item)
                        Insere(item, ref no.Dir); // tentar a esq.
                    else
                    {
                        Console.WriteLine("Elemento ja existente!!!\n");
                    }
            }
        }

        public int QuantidadeElementos(ref CNo Raiz)
        {
            if (Raiz == null)
                return 0;
            else
                Quant = 1 + QuantidadeElementos(ref Raiz.Esq) + QuantidadeElementos(ref Raiz.Dir);
            return Quant;
        }

        public int MaiorElemento(ref CNo Raiz)
        {
            if (Raiz != null)
            {
                if (Maior < Raiz.item)
                    Maior = Raiz.item;
                this.MaiorElemento(ref Raiz.Esq);
                this.MaiorElemento(ref Raiz.Dir);
                return Maior;
            }
            else
            {
                return 0;
            }

        }

        public int MenorElemento(ref CNo Raiz)
        {
            if (Raiz != null)
            {
                if (Menor > Raiz.item)
                    Menor = Raiz.item;
                this.MenorElemento(ref Raiz.Esq);
                this.MenorElemento(ref Raiz.Dir);
                return Menor;
            }
            else
            {
                return 0;
            }
                
        }

        public void Imprimir(int Ordem)
        {
            // Ordem = 1 => EmOrdem
            // Ordem = 2 => PreOrdem
            // Ordem = 3 => PosOrdem     
            switch (Ordem)
            {
                case 1: Console.WriteLine("\n\nImpressao em ordem\n");
                    EmOrdem(Raiz);
                    break;
                case 2: Console.WriteLine("\n\nImpressao pre ordem\n");
                    PreOrdem(Raiz);
                    break;
                case 3: Console.WriteLine("\n\nImpressao pos ordem\n");
                    PosOrdem(Raiz);
                    break;
                default: Console.WriteLine("Opcao invalida!!!\n\n");
                    break;
            }
        }

        private void EmOrdem(CNo no)
        {
            // percorre em ordem crescente
            if (no != null)
            {
                EmOrdem(no.Esq);
                Console.WriteLine("Chave = {0}\n", no.item);
                EmOrdem(no.Dir);
            }
        }

        private void PreOrdem(CNo no)
        {
            // percorre previlegiando a raiz sobre 
            // seus descendentes
            if (no != null)
            {
                Console.WriteLine("Chave = {0}\n", no.item);
                PreOrdem(no.Esq);
                PreOrdem(no.Dir);
            }
        }

        private void PosOrdem(CNo no)
        {
            // percorre previlegiando os descendentes 
            // sobre a raiz 
            if (no != null)
            {
                PosOrdem(no.Esq);
                PosOrdem(no.Dir);
                Console.WriteLine("Chave = {0}\n", no.item);
            }
        }

        public Boolean Achou(int itemProc)
        {
            // marcar o inicio (entra na arvore pela raiz)
            CNo no = Raiz;
            bool achou = false;

            // enquanto houver dados e nao achar o elemento
            while (no != null && no.item != itemProc)
            {
                // se for menor, avancar a esquerda,
                // se for maior, avancar a direita
                if (itemProc < no.item)
                    no = no.Esq;
                else
                    no = no.Dir;
            }
            // se nao achou, retorna FALSE, se achou, TRUE
            achou = no != null ? true : false;
            return achou;
        }

        public void Retira(int item)
        {
            CNo NoAnt = Raiz, NoRetira = Raiz;
            Remove(NoAnt, NoRetira, NoAnt, NoRetira, item);
        }

        private void Remove(CNo Aux, CNo AuxRaiz, CNo NoAnt, CNo NoRetira, int item)
        {
            if (AuxRaiz != null) //Testa se a árvore não esta nula...
            {

                if (item < AuxRaiz.item) /*Se o item é menor significa que o elemento procurado esta a esquerda
                                          * da atual AuxRaiz */
                {
                    Aux = AuxRaiz;
                    AuxRaiz = AuxRaiz.Esq;
                    Remove(Aux, AuxRaiz, NoAnt, NoRetira, item);
                }
                else
                    if (item > AuxRaiz.item)/*Se o item é maior significa que o elemento procurado esta a direita
                                             * da atual AuxRaiz */
                    {
                        Aux = AuxRaiz;
                        AuxRaiz = AuxRaiz.Dir;
                        Remove(Aux, AuxRaiz, NoAnt, NoRetira, item);
                    }
                    else //Encontrou o elemento procurado...
                    {
                        RemoveItem(Aux, AuxRaiz);
                    }

            }
            else//Não existe o elemento que está tentando remover...
                Console.WriteLine("Não existe este elemento na arvore!");
        }

        /// <summary>Método que retira um elemento da arvore binaria
        /// Método que elimina um elemento da arvore considerando as referencias que lhe são
        /// passados por parametro.
        /// 
        /// Ps.: Usa o algoritmo de remocao da sub-arvore a direita, o elemento mais a esquerda.
        /// </summary>
        /// <param name="AuxAnt">
        /// Variavel que contem a referencia do no anterior ao no que sera removido.
        /// </param>
        /// <param name="raiz">
        /// Variavel que contem a referencia do no que sera removido.
        /// </param>
        private void RemoveItem(CNo AuxAnt, CNo raiz)
        {

            if ((raiz.Dir == null) && (raiz.Esq == null))//Testa se o no que sera removido é no folha.
            {
                if (raiz.item > AuxAnt.item)
                    AuxAnt.Dir = null;
                else
                    AuxAnt.Esq = null;
            }
            else
            {
                if (raiz.Dir == null)//Testa se o no so tem um filho pela esquerda
                {
                    if (raiz.item > AuxAnt.item)
                        AuxAnt.Dir = raiz.Esq;
                    else
                        AuxAnt.Esq = raiz.Esq;
                }
                else
                {
                    if (raiz.Esq == null)//testa se o no so tem um filho pela direita
                    {
                        if (raiz.item > AuxAnt.item)
                            AuxAnt.Dir = raiz.Dir;
                        else
                            AuxAnt.Esq = raiz.Dir;
                    }
                    else
                    {
                        CNo Aux2 = raiz;
                        AuxAnt = raiz;
                        raiz = raiz.Dir;//noRaiz da subarvore
                        if (raiz.Esq == null)//testa se o no da sub-arvore tem filhos a esquerda
                        {
                            AuxAnt.Dir = raiz.Dir;
                        }
                        else
                        {
                            while (raiz.Esq != null)//busca o elemento mais a esquerda
                            {
                                Aux2 = raiz;
                                raiz = raiz.Esq;
                            }

                            AuxAnt.item = raiz.item;
                            if (raiz.Dir != null)//Testa se a direita do elemento mais a esquerda tem elementos
                                Aux2.Esq = raiz.Dir;
                            else
                            {
                                if (raiz.item > Aux2.item)
                                    Aux2.Dir = null;
                                else
                                    Aux2.Esq = null;
                            }
                        }
                    }
                }
            }
        }

        //Recebe o caminho do arquivo passado pelo usuário e lê o arquivo solicitado pelo usuário
        public void RecebeDiretorioArquivo(string ArqUsuario)
        {
            this.ArquivoLeitura = ArqUsuario;
            arquivoLeitura = new StreamReader(ArquivoLeitura);
        }

        //Armazenar os números do arquivo informado pelo usuário criando uma árvore binária
        public void ArmazenarNumeroArquivo()
        {
            string linha = "";

            //Preenchendo o Árvore Binária com os números do arquivo             
            while (linha != null)
            {
                linha = arquivoLeitura.ReadLine();
                int numero = int.Parse(linha);
                Inserir(numero);
            }

            arquivoLeitura.Close();
        }

        //Imprime o Arquivo de Resultados
        public void ImprimirArquivo()
        {
            arquivoEscrita = new StreamWriter("Resultado.txt", true);

            arquivoEscrita.WriteLine("Nó raiz: " + Raiz);
            arquivoEscrita.WriteLine("Quantidade de nós da árvore: " + QuantidadeElementos(ref Raiz));
            arquivoEscrita.WriteLine("O valor do maior nó da árvore: " + MaiorElemento(ref Raiz));
            arquivoEscrita.WriteLine("O valor do menor nó da árvore: " + MenorElemento(ref Raiz));
        }
    }
    #endregion
}