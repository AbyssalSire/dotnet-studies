using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

[Table("Categorias")]
public class Categoria
{
    // Boa pratica iniciarlizar a coleção quando definimos uma coleção em uma classe
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }
    [Key]
    public int CategoriaId { get; set; }
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImagemURL { get; set; }
    // Propriedade de navegação para indicar que uma Categoria pode possuir muitos Produtos
    //[JsonIgnore]
    public ICollection<Produto>? Produtos { get; set; }
}