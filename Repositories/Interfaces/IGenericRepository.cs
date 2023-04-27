namespace Prova1BIMIntegracaoSoftwareProjeto1.Repositories.Interfaces;

public interface IGenericRepository
{
    public Task Create();
    public Task Read();
    public Task Update();
    public Task Delete();
}