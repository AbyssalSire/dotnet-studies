namespace APICatalogo.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("Produtos")]
public class Produto
{
    [Key]
    public int ProdutoId { get; set; }
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }
    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }
    [Required]
    [Column(TypeName ="decimal(10,2)")]
    public decimal Preco { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImagemURL { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    // Propriedade que vai mapear para uma chave estrangeira no banco de dados
    [ForeignKey("CategoriaId")]
    public int CategoriaId { get; set; }
    // Propriedade de Navegação que indica que um Produto está relacionado a uma Categoria
    [JsonIgnore]
    public Categoria? Categoria { get; set; }
}