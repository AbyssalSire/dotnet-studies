namespace APICatalogo.Pagination;

public class ProdutosFiltroPreco : QueryStringParameters
{
    public decimal? Preco { get; set; }
    public string? PrecoCiterio { get; set; } //"maior", "menor" ou "igual"
}
