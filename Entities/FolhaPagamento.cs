namespace Prova1BIMIntegracaoSoftwareProjeto1.Entities;

public class FolhaPagamento
{
    public int Mes { get; set; }
    public int Ano { get; set; }
    public int Horas { get; set; }
    public int Valor { get; set; }
    public Funcionario Funcionario { get; set; }
    public string? Fk_Funcionario { get; set; }
}