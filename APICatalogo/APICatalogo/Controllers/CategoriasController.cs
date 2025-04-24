using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProduto()
    {
        _logger.LogInformation("--------- GET categoria/produtos ----------");
        var categoriasComProdutos = _uow.CategoriaRepository.GetCategoriasWithProducts();
        return Ok(categoriasComProdutos.ToCategoriaDTOList());
    }


    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        _logger.LogInformation("--------- GET categoria ----------");

        //throw new DataMisalignedException();
        var categorias = _uow.CategoriaRepository.GetAll();

        var categoriasDto = categorias.ToCategoriaDTOList();

        return Ok(categoriasDto);

    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = _uow.CategoriaRepository.GetCategorias(categoriasParameters);

        return ObterCategorias(categorias);

    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
    {
        var categoriasFiltradas = _uow.CategoriaRepository.GetCategoriasFiltroNome(categoriasFiltro);

        return ObterCategorias(categoriasFiltradas);
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.HasNext,
            categorias.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriasDTO = categorias.ToCategoriaDTOList();

        return Ok(categoriasDTO);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {

        //throw new Exception("Exceção ao retornar o a categoria pelo ID");
        var categoria = _uow.CategoriaRepository.Get(c => c.CategoriaId == id);
        _logger.LogInformation($"--------- GET categoria/id {id} ----------");
        if (categoria == null)
        {
            _logger.LogInformation($"--------- GET categoria/id {id} NOT FOUND ----------");
            return NotFound("Categoria não encontrada...");
        }

        var categoriaDto = categoria.ToCategoriaDTO();

        return Ok(categoriaDto);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
        {
            return BadRequest("Categoria não encontrada");
        }

        var categoria = categoriaDto.ToCategoria();

        var categoriaCriada = _uow.CategoriaRepository.Create(categoria);
        _uow.Commit();

        var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
        {
            return BadRequest("Id não corresponde à categoria");
        }

        var categoria = categoriaDto.ToCategoria();

        var categoriaAtualizada = _uow.CategoriaRepository.Update(categoria);
        _uow.Commit();

        var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();



        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _uow.CategoriaRepository.Get(c => c.CategoriaId == id);
        if (categoria == null)
        {
            return NotFound("Categoria não encontrada");
        }

        var categoriaExcluida = _uow.CategoriaRepository.Delete(categoria);
        _uow.Commit();

        var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();

        return Ok(categoriaExcluidaDto);
    }



}
