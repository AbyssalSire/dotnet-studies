namespace APICatalogo.Models;

using APICatalogo.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("Produtos")]
public class Produto :IValidatableObject
{
    [Key]
    public int ProdutoId { get; set; }
    [Required(ErrorMessage ="O nome é obrigatório")]
    [StringLength(80)]
    [PrimeiraLetraMaiuscula]
    public string? Nome { get; set; }
    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }
    [Required]
    [Range(1, 1000000, ErrorMessage ="O preço deve estar entre {1} e {2}")]
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

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(this.Nome))
        {
            var primeiraLetra = this.Nome[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new ValidationResult("A primeira letra do produto deve ser maiúscula", new[] { nameof(this.Nome) });
            }
        }

        if(this.Estoque <= 0)
        {
            yield return new ValidationResult("A estoque deve ser maior que zero!", new[] { nameof(this.Nome) });
        }
    }
}