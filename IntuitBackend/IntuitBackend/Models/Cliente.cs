using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntuitBackend.Models
{
    
    public partial class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "El nombre no debe superar los {1} carácteres.")]
        public string Nombres { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "El apellido no debe superar los {1} carácteres.")]
        public string Apellidos { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [FechaNacimientoValida]
        public DateTime? FechaNacimiento { get; set; }
        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Número de cuit inválido.")]
        [RegularExpression(@"^\d{2}-\d{8}-\d{1}$", ErrorMessage = "CUIT inválido")]
        public string Cuit { get; set; }
        [StringLength(300, ErrorMessage = "El domicilio no debe superar los {1} carácteres.")]
        public string Domicilio { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "El número de teléfono no debe superar los {1} carácteres.")]
        public string TelefonoCelular { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Formato de email incorrecto.")]
        [StringLength(300, ErrorMessage = "El email no debe superar los {1} carácteres.")]
        public string Email { get; set; }
    }

    public class FechaNacimientoValidaAttribute : RangeAttribute
    {
        public FechaNacimientoValidaAttribute() : base(typeof(DateTime), DateTime.Now.AddYears(-110).ToString("yyyy-MM-dd"), DateTime.Now.AddYears(-18).ToString("yyyy-MM-dd"))
        {
            ErrorMessage = "La fecha debe estar entre {1} y {2}";
        }
    }
}
