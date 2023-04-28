using Microsoft.AspNetCore.Mvc;
using Prova1BIMIntegracaoSoftwareProjeto1.Entities;
using Prova1BIMIntegracaoSoftwareProjeto1.Repositories.Interfaces;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace Prova1BIMIntegracaoSoftwareProjeto1.Controllers;

[ApiController]
[Route("[controller]")]
public class GenericController : ControllerBase
{
    private readonly IGenericRepository _repository;

    public GenericController(IGenericRepository repository)
    {
        _repository = repository;
    }
    
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create([FromBody] FolhaPagamento folha)
    {
        await _repository.CreateFuncionario(folha);
        await _repository.Create(folha);
        return Ok();
    }
    
    [HttpGet]
    [Route("Calculate")]
    public async Task<IActionResult> Calculate()
    {
        var folhasAEnviar = await _repository.Read();
        List<FolhaCalculada> folhasCalculadas = new();
        foreach (var folha in folhasAEnviar)
        {
            FolhaCalculada folhaCalculada = new();
            folhaCalculada.Bruto = folha.Horas * folha.Valor;

            double aliquotaIrrf = 0;
            double parcelaADeduzir = 0;

            if (folhaCalculada.Bruto >= 1904 && folhaCalculada.Bruto <= 2827)
            {
                aliquotaIrrf = 7.5;
                parcelaADeduzir = 142.80;
            } else if (folhaCalculada.Bruto > 2827 && folhaCalculada.Bruto <= 3751)
            {
                aliquotaIrrf = 15;
                parcelaADeduzir = 354.80;
            } else if (folhaCalculada.Bruto > 3751 && folhaCalculada.Bruto <= 4665)
            {
                aliquotaIrrf = 22.5;
                parcelaADeduzir = 636.13;
            } else if (folhaCalculada.Bruto > 4665)
            {
                aliquotaIrrf = 27.5;
                parcelaADeduzir = 869.36;
            }

            double inss = 0.08;

            if (folhaCalculada.Bruto >= 1694 && folhaCalculada.Bruto < 2823)
            {
                inss = 0.09;
            } else if (folhaCalculada.Bruto >= 2823 && folhaCalculada.Bruto < 5646)
            {
                inss = 0.11;
            } else if (folhaCalculada.Bruto >= 5645)
            {
                inss = 621.03;
            }

            double fgts = folhaCalculada.Bruto * 0.08;
            double inssADescontar = 0;
            if (inss != 621.03)
            {
                inssADescontar = folhaCalculada.Bruto * inss;
            }
            else
            {
                inssADescontar = folhaCalculada.Bruto - inss;
            }

            folhaCalculada.Mes = folha.Mes;
            folhaCalculada.Ano = folha.Ano;
            folhaCalculada.Horas = folha.Horas;
            folhaCalculada.Valor = folha.Valor;
            folhaCalculada.Inss = inssADescontar;
            folhaCalculada.Irrf = parcelaADeduzir;
            folhaCalculada.Fgts = fgts;
            folhaCalculada.Liquido = folhaCalculada.Bruto - parcelaADeduzir - inssADescontar;
            
            folhasCalculadas.Add(folhaCalculada);
        }

        HttpClient httpClient = new();
        HttpContent content = new StringContent(JsonConvert.SerializeObject(folhasCalculadas), System.Text.Encoding.UTF8, "application/json");
        await httpClient.PostAsync("https://localhost:7273/Generic/SaveFolhas", content);

        return Ok();
    }
}