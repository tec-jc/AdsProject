using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.BussinessEntities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(50, ErrorMessage = "El largo máximo es de 50 caracteres")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [NotMapped]
        public int Top_Aux { get; set; } // propiedad auxiliar
        public List<Ad> Ads { get; set; } = new List<Ad>(); // propiedad de navegación
    }
}
