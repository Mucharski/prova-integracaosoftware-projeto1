using Microsoft.Data.Sqlite;
using Prova1BIMIntegracaoSoftwareProjeto1.Repositories.Interfaces;

namespace Prova1BIMIntegracaoSoftwareProjeto1.Repositories;

public class GenericRepository : IGenericRepository
{
    private readonly SqliteConnection _connection = new("Data Source=./databaseProva.db");

    public async Task Create()
    {
        await _connection.OpenAsync();
    }
    
    public async Task Read()
    {
        await _connection.OpenAsync();
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