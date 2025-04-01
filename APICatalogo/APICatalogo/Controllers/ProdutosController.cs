using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _repository;

    public ProdutosController(IProdutoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _repository.GetProdutos().ToList();
        if (produtos is null)
        {
            return NotFound("Produtos não encontrados...");
        }
        return Ok(produtos);
    }

    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
    //{
    //    var produtos = _context.Produtos.AsNoTracking().ToListAsync();
    //    if (produtos is null)
    //    {
    //        return NotFound("Produtos não encontrados...");
    //    }
    //    return await produtos;
    //}

    [HttpGet("primeiro")]
    //[HttpGet("/primeiro")]
    //[HttpGet("teste")]
    //[HttpGet("{valor:alpha:length(5)}")]
    //public ActionResult<Produto> Get2(string valor) {
    // var teste = valor;
    public ActionResult<Produto> GetPrimeiro()
    {
        var produto = _repository.GetProdutoPrimeiro();
        if (produto is null)
        {
            return NotFound("Produtos não encontrados...");
        }
        return Ok(produto);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _repository.GetProduto(id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado");
        }
        return Ok(produto);
    }
    //[HttpGet("{id:int:min(1) }", Name = "ObterProduto")]
    //[HttpGet("{id:int}", Name = "ObterProduto")]
    //public async Task<ActionResult<Produto>> GetAsync(int id)
    //{
    //    var produto = _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
    //    if (produto is null)
    //    {
    //        return NotFound("Produto não encontrado");
    //    }
    //    return await produto;
    //}


    //[HttpGet("{id:int}", Name = "ObterProduto")]
    //public async Task<ActionResult<Produto>> GetAsync(int id, [BindRequired]string nome)
    //{
    //    var nomeProduto = nome;
    //    var produto = _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
    //    if (produto is null)
    //    {
    //        return NotFound("Produto não encontrado");
    //    }
    //    return await produto;
    //}
    //[HttpGet("{id:int}", Name = "ObterProduto")]
    //public async Task<ActionResult<Produto>> GetAsync(int id, [FromQuery]string nome)
    //{
    //    var nomeProduto = nome;
    //    var produto = _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
    //    if (produto is null)
    //    {
    //        return NotFound("Produto não encontrado");
    //    }
    //    return await produto;
    //}

    //[HttpGet("{id:int}/{param2=Caderno}", Name = "ObterProduto2")]
    //public ActionResult<Produto> Get(int id, string param2)
    //{
    //    var parametro = param2;
    //    var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
    //    if (produto is null)
    //    {
    //        return NotFound("Produto não encontrado");
    //    }
    //    return produto;
    //}

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if(produto is null)
        {
            return BadRequest("Campo Produto vazio...");
        }

        var novoProduto = _repository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if(id != produto.ProdutoId)
        {
            return BadRequest("Id passado é diferente do Id do produto que está tentando alterar");
        }

        bool atualizado = _repository.Update(produto);

        if (atualizado)
        {
            return Ok(produto);
        }
        else
        {
            return StatusCode(500, $"Falha ao atualizar o produto de id={id}");
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        bool deletado = _repository.Delete(id);

        if (deletado)
        {
            return Ok($"Produto de id={id} foi excluído");
        }
        else
        {
            return StatusCode(500, $"Falha ao excluir o produto de id={id}");
        }
    }

}
