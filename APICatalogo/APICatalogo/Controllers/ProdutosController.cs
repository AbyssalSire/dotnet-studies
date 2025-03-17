using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<Produto>> Get()
        //{
        //    var produtos = _context.Produtos.AsNoTracking() .ToList();
        //    if(produtos is null)
        //    {
        //        return NotFound("Produtos não encontrados...");
        //    }
        //    return produtos;
        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            var produtos = _context.Produtos.AsNoTracking().ToListAsync();
            if (produtos is null)
            {
                return NotFound("Produtos não encontrados...");
            }
            return await produtos;
        }

        [HttpGet("primeiro")]
        //[HttpGet("/primeiro")]
        //[HttpGet("teste")]
        //[HttpGet("{valor:alpha:length(5)}")]
        //public ActionResult<Produto> Get2(string valor) {
        // var teste = valor;
        public ActionResult<Produto> GetPrimeiro()
        {
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault();
            if (produto is null)
            {
                return NotFound("Produtos não encontrados...");
            }
            return produto;
        }

        //[HttpGet("{id:int:min(1) }", Name = "ObterProduto")]
        //public ActionResult<Produto> Get(int id)
        //{
        //    var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
        //    if(produto is null)
        //    {
        //        return NotFound("Produto não encontrado");
        //    }
        //    return produto;
        //}        [HttpGet("{id:int:min(1) }", Name = "ObterProduto")]

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetAsync(int id)
        {
            var produto = _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }
            return await produto;
        }
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
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest("Id passado é diferente do Id do produto que está tentando alterar");
            }
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            // alternativamente pode-se usar o método find caso a chave de pesquisa seja a primária
            // var produto = _context.Produtos.Find(id);

            if (produto is null)
            {
                return NotFound("Produto não localizado...");
            }
            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }

    }
}
