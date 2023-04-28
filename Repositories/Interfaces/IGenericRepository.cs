using Prova1BIMIntegracaoSoftwareProjeto1.Entities;

namespace Prova1BIMIntegracaoSoftwareProjeto1.Repositories.Interfaces;

public interface IGenericRepository
{
    public Task Create(FolhaPagamento folha);
    public Task CreateFuncionario(FolhaPagamento folha);
    public Task<List<FolhaPagamento>> Read();
    public Task Update();
    public Task Delete();
}