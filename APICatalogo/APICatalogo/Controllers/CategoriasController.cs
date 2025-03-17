using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProduto()
        {
            return _context.Categorias.Include(p=>p.Produtos).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
            //throw new DataMisalignedException();
            return _context.Categorias.AsNoTracking().ToList();
            // é recomendável sempre fazer uma filtragem ao invés de pegar todos os items
            // return _context.Categorias.Include(p=>p.Produtos).Where(c=>c.CategoriaId<=5).ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            if(categoria == null)
            {
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

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId)
            {
                return BadRequest("Id não corresponde à categoria");
            }
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            if(categoria == null)
            {
                return NotFound("Categoria não encontrada");
            }
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            return Ok(categoria);
        }



    }
}
