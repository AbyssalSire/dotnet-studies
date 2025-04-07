using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    //private readonly IRepository<Categoria> _repository;
    private readonly IUnitOfWork _uow;
    //private readonly ICategoriaRepository _categoriaRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public CategoriasController(IUnitOfWork uow, IConfiguration configuration, ILogger<CategoriasController> logger)
    {
        //_repository = repository;
        _uow = uow;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet("LerAquivoConfiguracao")]
    public string GetValores()
    {
        var valor1 = _configuration["chave1"];
        var valor2 = _configuration["chave2"];
        var secao1 = _configuration["secao1:chave2"];

        return $"Chave 1 = {valor1}\nChave 2 = {valor2}\nSeção 1 => Chave 2 = {secao1}";

    }

    
     [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProduto()
    {
        _logger.LogInformation("--------- GET categoria/produtos ----------");
        var categoriasComProdutos = _uow.CategoriaRepository.GetCategoriasWithProducts();
        return Ok(categoriasComProdutos);
    }
    

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        _logger.LogInformation("--------- GET categoria ----------");

        //throw new DataMisalignedException();
        var categorias = _uow.CategoriaRepository.GetAll();
        return Ok(categorias);

    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {

        //throw new Exception("Exceção ao retornar o a categoria pelo ID");
        var categoria = _uow.CategoriaRepository.Get(c=>c.CategoriaId == id);
        _logger.LogInformation($"--------- GET categoria/id {id} ----------");
        if(categoria == null)
        {
        _logger.LogInformation($"--------- GET categoria/id {id} NOT FOUND ----------");
            return NotFound("Categoria não encontrada...");
        }
        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if(categoria is null)
        {
            return BadRequest("Categoria não encontrada");
        }

        var categoriaCriada = _uow.CategoriaRepository.Create(categoria);
        _uow.Commit();

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        if(id != categoria.CategoriaId)
        {
            return BadRequest("Id não corresponde à categoria");
        }

        _uow.CategoriaRepository.Update(categoria);
        _uow.Commit();

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Categoria> Delete(int id)
    {
        var categoria = _uow.CategoriaRepository.Get(c => c.CategoriaId == id);
        if(categoria == null)
        {
            return NotFound("Categoria não encontrada");
        }

        var categoriaExcluida = _uow.CategoriaRepository.Delete(categoria);
        _uow.Commit();

        return Ok(categoria);
    }



}
