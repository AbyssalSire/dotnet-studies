using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    IEnumerable<Categoria> GetCategoriasWithProducts();
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams);
    PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParams);


}
