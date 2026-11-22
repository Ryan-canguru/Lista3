using System;
using System.Collections.Generic;
using System.IO;
using CadastroBanda;

class Program
{
    
    static void salvarDados(List<Banda> listaBandas, string nomeArquivo)
    {
        using (StreamWriter writer = new StreamWriter(nomeArquivo))
        {
            foreach (Banda banda in listaBandas)
            {
                writer.WriteLine($"{banda.nome},{banda.genero},{banda.integrantes},{banda.ranking}");
            }
        }

        Console.WriteLine("Dados salvos com sucesso!");
    }

    
    static void carregarDados(List<Banda> listaBandas, string nomeArquivo)
    {
        if (File.Exists(nomeArquivo))
        {
            string[] linhas = File.ReadAllLines(nomeArquivo);

            listaBandas.Clear();

            foreach (string linha in linhas)
            {
                string[] campos = linha.Split(',');

                if (campos.Length == 4 &&
                    int.TryParse(campos[2], out int integrantes) &&
                    int.TryParse(campos[3], out int ranking))
                {
                    Banda banda = new Banda();

                    banda.nome = campos[0];
                    banda.genero = campos[1];
                    banda.integrantes = integrantes;
                    banda.ranking = ranking;

                    listaBandas.Add(banda);
                }
            }

            Console.WriteLine("Dados carregados com sucesso!");
        }
        else
        {
            Console.WriteLine("Arquivo não encontrado!");
        }
    }

    
    static int menu()
    {
        Console.WriteLine("\n=== SISTEMA DE CADASTRO DE BANDAS ===");
        Console.WriteLine("1 - Adicionar Banda");
        Console.WriteLine("2 - Mostrar Bandas");
        Console.WriteLine("3 - Buscar Banda por Nome");
        Console.WriteLine("4 - Buscar Banda por Ranking");
        Console.WriteLine("5 - Buscar Banda por Gênero");
        Console.WriteLine("6 - Atualizar Banda");
        Console.WriteLine("7 - Remover Banda");
        Console.WriteLine("8 - Salvar Dados");
        Console.WriteLine("9 - Carregar Dados");
        Console.WriteLine("0 - Sair");
        Console.Write("Escolha uma opção: ");

        if (int.TryParse(Console.ReadLine(), out int opcao))
        {
            return opcao;
        }

        return -1;
    }

    // LER NÚMEROS
    static int lerNumero(string mensagem)
    {
        int numero;

        while (true)
        {
            Console.Write(mensagem);

            if (int.TryParse(Console.ReadLine(), out numero) && numero > 0)
            {
                return numero;
            }

            Console.WriteLine("Digite um número inteiro maior que zero!");
        }
    }

    
    static string lerTexto(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string texto = Console.ReadLine()!;

            if (!string.IsNullOrWhiteSpace(texto) && !texto.Contains(","))
            {
                return texto.Trim();
            }

            Console.WriteLine("Digite um texto válido, sem vírgulas!");
        }
    }

    
    static void addBanda(List<Banda> listaBandas)
    {
        Banda novaBanda = new Banda();

        Console.WriteLine("\n=== CADASTRAR BANDA ===");

        novaBanda.nome = lerTexto("Nome da banda: ");
        novaBanda.genero = lerTexto("Gênero da banda: ");
        novaBanda.integrantes = lerNumero("Número de integrantes: ");
        novaBanda.ranking = lerNumero("Ranking da banda: ");

        listaBandas.Add(novaBanda);

        Console.WriteLine("Banda cadastrada com sucesso!");
    }

    
    static void mostrarDadosBanda(Banda banda)
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine($"Nome: {banda.nome}");
        Console.WriteLine($"Gênero: {banda.genero}");
        Console.WriteLine($"Integrantes: {banda.integrantes}");
        Console.WriteLine($"Ranking: {banda.ranking}");
        Console.WriteLine("-------------------------");
    }

    
    static void mostrarBandas(List<Banda> listaBandas)
    {
        Console.WriteLine("\n=== BANDAS CADASTRADAS ===");

        if (listaBandas.Count == 0)
        {
            Console.WriteLine("Nenhuma banda cadastrada!");
            return;
        }

        for (int i = 0; i < listaBandas.Count; i++)
        {
            Console.WriteLine($"\nBanda {i + 1}");
            mostrarDadosBanda(listaBandas[i]);
        }
    }

    
    static bool buscarBanda(List<Banda> listaBandas, string nomeBanda)
    {
        bool encontrado = false;

        foreach (Banda banda in listaBandas)
        {
            if (banda.nome.ToUpper().Contains(nomeBanda.ToUpper()))
            {
                mostrarDadosBanda(banda);
                encontrado = true;
            }
        }

        return encontrado;
    }

    
    static void buscarRanking(List<Banda> listaBandas)
    {
        int ranking = lerNumero("Digite a posição do ranking: ");

        bool encontrado = false;

        foreach (Banda banda in listaBandas)
        {
            if (banda.ranking == ranking)
            {
                mostrarDadosBanda(banda);
                encontrado = true;
            }
        }

        if (!encontrado)
        {
            Console.WriteLine("Nenhuma banda encontrada nesse ranking!");
        }
    }

    
    static void buscarGenero(List<Banda> listaBandas)
    {
        string genero = lerTexto("Digite o gênero musical: ");

        bool encontrado = false;

        foreach (Banda banda in listaBandas)
        {
            if (banda.genero.ToUpper().Equals(genero.ToUpper()))
            {
                mostrarDadosBanda(banda);
                encontrado = true;
            }
        }

        if (!encontrado)
        {
            Console.WriteLine("Nenhuma banda encontrada desse gênero!");
        }
    }

    
    static int buscarIndiceBanda(List<Banda> listaBandas, string nomeBanda)
    {
        for (int i = 0; i < listaBandas.Count; i++)
        {
            if (listaBandas[i].nome.ToUpper().Equals(nomeBanda.ToUpper()))
            {
                return i;
            }
        }

        return -1;
    }

    
    static bool atualizarBanda(List<Banda> listaBandas, string nomeBanda)
    {
        int indice = buscarIndiceBanda(listaBandas, nomeBanda);

        if (indice != -1)
        {
            Console.WriteLine("\nDados atuais da banda:");
            mostrarDadosBanda(listaBandas[indice]);

            Console.WriteLine("\nDigite os novos dados:");

            listaBandas[indice].nome = lerTexto("Novo nome: ");
            listaBandas[indice].genero = lerTexto("Novo gênero: ");
            listaBandas[indice].integrantes = lerNumero("Novos integrantes: ");
            listaBandas[indice].ranking = lerNumero("Novo ranking: ");

            return true;
        }

        return false;
    }

    
    static bool removerBanda(List<Banda> listaBandas, string nomeBanda)
    {
        int indice = buscarIndiceBanda(listaBandas, nomeBanda);

        if (indice != -1)
        {
            Console.WriteLine("\nBanda encontrada:");
            mostrarDadosBanda(listaBandas[indice]);

            Console.Write("Confirma a remoção? (1-Sim / 2-Não): ");

            if (int.TryParse(Console.ReadLine(), out int confirmacao)
                && confirmacao == 1)
            {
                listaBandas.RemoveAt(indice);
                return true;
            }

            Console.WriteLine("Remoção cancelada!");
            return false;
        }

        Console.WriteLine("Banda não encontrada!");
        return false;
    }

    
    static void Main()
    {
        List<Banda> Listabandas = new List<Banda>();

        string nomeArquivo = "bandas.txt";
        int opcao;

        carregarDados(Listabandas, nomeArquivo);

        do
        {
            opcao = menu();

            switch (opcao)
            {
                case 1:
                    addBanda(Listabandas);
                    break;

                case 2:
                    mostrarBandas(Listabandas);
                    break;

                case 3:
                    Console.WriteLine("\n=== BUSCAR POR NOME ===");

                    string nomeBusca = lerTexto("Digite o nome da banda: ");

                    bool encontrado = buscarBanda(Listabandas, nomeBusca);

                    if (!encontrado)
                    {
                        Console.WriteLine("Banda não encontrada!");
                    }
                    break;

                case 4:
                    Console.WriteLine("\n=== BUSCAR POR RANKING ===");
                    buscarRanking(Listabandas);
                    break;

                case 5:
                    Console.WriteLine("\n=== BUSCAR POR GÊNERO ===");
                    buscarGenero(Listabandas);
                    break;

                case 6:
                    Console.WriteLine("\n=== ATUALIZAR BANDA ===");

                    string nomeAtualizar = lerTexto("Digite o nome da banda: ");

                    bool atualizado = atualizarBanda(Listabandas, nomeAtualizar);

                    if (atualizado)
                    {
                        Console.WriteLine("Banda atualizada com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Banda não encontrada!");
                    }
                    break;

                case 7:
                    Console.WriteLine("\n=== REMOVER BANDA ===");

                    string nomeRemover = lerTexto("Digite o nome da banda: ");

                    bool removido = removerBanda(Listabandas, nomeRemover);

                    if (removido)
                    {
                        Console.WriteLine("Banda removida com sucesso!");
                    }
                    break;

                case 8:
                    salvarDados(Listabandas, nomeArquivo);
                    break;

                case 9:
                    carregarDados(Listabandas, nomeArquivo);
                    break;

                case 0:
                    salvarDados(Listabandas, nomeArquivo);
                    Console.WriteLine("Saindo do programa...");
                    break;

                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }

        } while (opcao != 0);
    }
}