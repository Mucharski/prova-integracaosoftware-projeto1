using Microsoft.AspNetCore.Mvc;
using Prova1BIMIntegracaoSoftwareProjeto1.Repositories.Interfaces;

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
    public async Task<IActionResult> Create()
    {
        await _repository.Create();
        return Ok();
    }
    
    [HttpGet]
    [Route("Read")]
    public async Task<IActionResult> Read()
    {
        return Ok();
    }
    
    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> Update()
    {
        return Ok();
    }
    
    [HttpDelete]
    [Route("Delete")]
    public async Task<IActionResult> Delete()
    {
        return Ok();
    }
}