﻿using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    Produto GetProdutoPrimeiro();
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
    //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);

    PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams);
}
