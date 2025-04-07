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
    private readonly IUnitOfWork _uow;

    public ProdutosController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet("produtos/{id:int}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
        var produtos = _uow.ProdutoRepository.GetProdutosPorCategoria(id);
        if (produtos is null)
            return NotFound();
        return Ok(produtos);
    }


    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _uow.ProdutoRepository.GetAll();
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
        var produto = _uow.ProdutoRepository.GetProdutoPrimeiro();
        if (produto is null)
        {
            return NotFound("Produtos não encontrados...");
        }
        return Ok(produto);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _uow.ProdutoRepository.Get(p=>p.ProdutoId==id);
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

        var novoProduto = _uow.ProdutoRepository.Create(produto);
        _uow.Commit ();
        return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if(id != produto.ProdutoId)
        {
            return BadRequest("Id passado é diferente do Id do produto que está tentando alterar");
        }

        var produtoAtualizado = _uow.ProdutoRepository.Update(produto);
        _uow.Commit();



         return Ok(produtoAtualizado);        
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _uow.ProdutoRepository.Get(p => p.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("Produtos não encontrados...");
        }

        var produtoDeletado = _uow.ProdutoRepository.Delete(produto);
        _uow.Commit();
        return Ok(produtoDeletado);
    }
}
