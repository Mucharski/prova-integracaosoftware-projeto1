using Dapper;
using Microsoft.Data.Sqlite;
using Prova1BIMIntegracaoSoftwareProjeto1.Entities;
using Prova1BIMIntegracaoSoftwareProjeto1.Repositories.Interfaces;

namespace Prova1BIMIntegracaoSoftwareProjeto1.Repositories;

public class GenericRepository : IGenericRepository
{
    private readonly SqliteConnection _connection = new("Data Source=./databaseProva.db");

    public async Task Create(FolhaPagamento folha)
    {
        await _connection.OpenAsync();
        
        await _connection.QueryAsync($"INSERT INTO folha (mes, ano, horas, valor, fk_funcionario)" +
                                     "VALUES (@mes, @ano, @horas, @valor, @fkFuncionario)",
            new
            {
                mes = folha.Mes, ano = folha.Ano, horas = folha.Horas, valor = folha.Valor,
                fkFuncionario = folha.Funcionario.Cpf
            });
    }

    public async Task CreateFuncionario(FolhaPagamento folha)
    {
        await _connection.OpenAsync();

        var funcionario = await _connection.QuerySingleOrDefaultAsync("SELECT * FROM funcionario WHERE cpf = @cpf", new
        {
            cpf = folha.Funcionario.Cpf
        });

        if (funcionario == null)
        {
            await _connection.QueryAsync($"INSERT INTO funcionario (cpf, nome)" +
                                         "VALUES (@cpf, @nome)",
                new
                {
                    cpf = folha.Funcionario.Cpf,
                    nome = folha.Funcionario.Nome
                });
        }
    }

    public async Task<List<FolhaPagamento>> Read()
    {
        await _connection.OpenAsync();
        
        var folhas = await _connection.QueryAsync<FolhaPagamento>
            ("SELECT mes, ano, horas, valor, fk_funcionario FROM folha WHERE foiProcessada = 0");

        foreach (var folha in folhas)
        {
            var funcionario = await _connection.QuerySingleAsync<Funcionario>
            ("SELECT * FROM funcionario WHERE cpf = @fkCpf", new
            {
                fkCpf = folha.Fk_Funcionario
            });

            folha.Funcionario = funcionario;
            
            await _connection.ExecuteAsync("UPDATE folha SET foiProcessada = 1");
        }

        return folhas.ToList();
    }

    public async Task Update()
    {
        await _connection.OpenAsync();
    }

    public async Task Delete()
    {
        await _connection.OpenAsync();
    }
}