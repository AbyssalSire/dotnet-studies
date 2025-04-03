using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{

    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }


    public IEnumerable<Categoria> GetCategoriasWithProducts()
    {
        return _context.Categorias.Include(p => p.Produtos).ToList();
    }

   
}
