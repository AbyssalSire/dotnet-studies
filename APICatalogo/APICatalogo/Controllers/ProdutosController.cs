using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [HttpGet("produtos/{id:int}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
    {
        var produtos = _uow.ProdutoRepository.GetProdutosPorCategoria(id);
        if (produtos is null)
            return NotFound();
        // var destino = _mapper.Map<Destino>(origem);
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = _uow.ProdutoRepository.GetProdutos(produtosParameters);

        return ObterProdutos(produtos);
    }

    [HttpGet("filter/preco/pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco produtosFilterParams)
    {
        var produtos = _uow.ProdutoRepository.GetProdutosFiltroPreco(produtosFilterParams);

        return ObterProdutos(produtos);

    }

    private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
    {
        var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        var produtos = _uow.ProdutoRepository.GetAll();
        if (produtos is null)
        {
            return NotFound("Produtos não encontrados...");
        }
        // var destino = _mapper.Map<Destino>(origem);
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);
    }

    [HttpGet("primeiro")]
    //[HttpGet("/primeiro")]
    //[HttpGet("teste")]
    //[HttpGet("{valor:alpha:length(5)}")]
    //public ActionResult<Produto> Get2(string valor) {
    // var teste = valor;
    public ActionResult<ProdutoDTO> GetPrimeiro()
    {
        var produto = _uow.ProdutoRepository.GetProdutoPrimeiro();
        if (produto is null)
        {
            return NotFound("Produtos não encontrados...");
        }
        var produtoDto = _mapper.Map<ProdutoDTO>(produto);
        return Ok(produtoDto);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id)
    {
        var produto = _uow.ProdutoRepository.Get(p=>p.ProdutoId==id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado");
        }
        var produtoDto = _mapper.Map<ProdutoDTO>(produto);
        return Ok(produtoDto);
    }
  
    [HttpPost]
    public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
    {
        if(produtoDto is null)
        {
            return BadRequest("Campo Produto vazio...");
        }

        var produto = _mapper.Map<Produto>(produtoDto);
        var novoProduto = _uow.ProdutoRepository.Create(produto);
        _uow.Commit ();

        var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);
        return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
    }

    [HttpPatch("{id}/UpdatePartial")]
    public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if(patchProdutoDTO is null || id <= 0)
        {
            return BadRequest();
        }
        var produto = _uow.ProdutoRepository.Get(c => c.ProdutoId == id); 

        if(produto is null)
        {
            return NotFound();
        }

        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);
        if(produtoUpdateRequest is null)
        {
            return BadRequest("Ocorreu um erro ao mapear o produto");
        }

        patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);

        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var validationResults = ValidateChangedFields(patchProdutoDTO, produtoUpdateRequest);

        if (validationResults.Any())
        {
            foreach (var validationResult in validationResults)
            {
                ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
            return BadRequest(ModelState);
        }

        _mapper.Map(produtoUpdateRequest, produto);

        _uow.ProdutoRepository.Update(produto);
        _uow.Commit();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }

    private List<ValidationResult> ValidateChangedFields(JsonPatchDocument<ProdutoDTOUpdateRequest> patchDoc,
                                                          ProdutoDTOUpdateRequest produtoUpdateRequest)
    {
        var validationResults = new List<ValidationResult>();

        foreach (var operation in patchDoc.Operations)
        {
            if (operation.path.Equals("/estoque", StringComparison.OrdinalIgnoreCase))
            {
                var context = new ValidationContext(produtoUpdateRequest) { MemberName = nameof(produtoUpdateRequest.Estoque) };
                Validator.TryValidateProperty(produtoUpdateRequest.Estoque, context, validationResults);
            }
            else if (operation.path.Equals("/datacadastro", StringComparison.OrdinalIgnoreCase))
            {
                var context = new ValidationContext(produtoUpdateRequest) { MemberName = nameof(produtoUpdateRequest.DataCadastro) };
                Validator.TryValidateProperty(produtoUpdateRequest.DataCadastro, context, validationResults);

                // Adiciona validação personalizada
                if (produtoUpdateRequest.DataCadastro.Date <= DateTime.Now.Date)
                {
                    validationResults.Add(new ValidationResult("A data deve ser maior ou igual a data atual",
                                          new[] { nameof(produtoUpdateRequest.DataCadastro) }));
                }
            }
        }
        return validationResults;
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
    {
        if(id != produtoDto.ProdutoId)
        {
            return BadRequest("Id passado é diferente do Id do produto que está tentando alterar");
        }

        var produto = _mapper.Map<Produto>(produtoDto);
        var produtoAtualizado = _uow.ProdutoRepository.Update(produto);
        _uow.Commit();

        var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);

         return Ok(produtoAtualizadoDto);        
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produto = _uow.ProdutoRepository.Get(p => p.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("Produtos não encontrados...");
        }

        var produtoDeletado = _uow.ProdutoRepository.Delete(produto);
        _uow.Commit();
        var produtoDeletadoDto = _mapper.Map<ProdutoDTO>(produtoDeletado);
        return Ok(produtoDeletadoDto);
    }
}
