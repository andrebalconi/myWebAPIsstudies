using APICatalogo.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto : IValidatableObject
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage ="The name is required!")]
        [StringLength(80,ErrorMessage ="The Name must contain between 5 and 80 caracters.", MinimumLength = 5)]
        //[PrimeiraLetraMaiuscula] //First option
        public string Nome { get; set; }
        [Required]
        [StringLength(300, ErrorMessage = "The Name must contain between 10 and 300 caracters.", MinimumLength = 10)]
        public string Descricao { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,2)")]
        [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }
        [Required]
        [MaxLength(500)]
        public string ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.Nome))
            {
                var primeiraLetra = this.Nome[0].ToString();
                if (primeiraLetra != primeiraLetra.ToUpper())
                {
                    yield return new
                        ValidationResult("A primeira letra do produto deve ser maiúscula",
                        new[]
                        { nameof(this.Nome)});
                }
            }
        }
    }
}
