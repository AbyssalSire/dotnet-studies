using APICatalogo.Models;

namespace APICatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    Produto GetProdutoPrimeiro();
    IEnumerable<Produto> GetProdutosPorCategoria(int id);

}
