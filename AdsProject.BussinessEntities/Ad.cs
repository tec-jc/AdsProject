using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.BussinessEntities
{
    public class Ad
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Category")]
        [Required(ErrorMessage = "La categoría es requerida")]
        [Display(Name  = "Categoría")]
        public int IdCategory { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [MaxLength(200, ErrorMessage = "Máximo 200 caracteres")]
        [Display(Name = "Título")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(800, ErrorMessage = "Máximo 800 caracteres")]
        [Display(Name = "Descripción")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es requerido")]
        [Display(Name = "Precio")]
        public decimal Price { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [MaxLength(20, ErrorMessage = "Máximo 20 caracteres")]
        [Display(Name = "Estado")]
        public string State { get; set; } = string.Empty;

        [NotMapped]
        public int Top_Aux { get; set; } // propiedad de navegación
        public Category? Category { get; set; } // propiedad de navegación
        public List<AdImage>? AdImages { get; set; } // propiedad de navegación

    }
}
