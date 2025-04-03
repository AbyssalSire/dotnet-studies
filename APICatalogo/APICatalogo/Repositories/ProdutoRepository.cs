using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{

    public ProdutoRepository(AppDbContext context) : base (context)
    {
    }


    public Produto GetProdutoPrimeiro()
    {
        var produto = _context.Produtos.FirstOrDefault();
        if (produto is null)
            throw new InvalidOperationException("Produto é null");

        return produto;
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}
