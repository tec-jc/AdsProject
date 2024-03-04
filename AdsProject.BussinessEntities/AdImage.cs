using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.BussinessEntities
{
    public class AdImage
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Ad")]
        [Required(ErrorMessage = "El anuncio es requerido")]
        [Display(Name = "Anuncio")]
        public int IdAd { get; set; }

        [Required(ErrorMessage = "La ruta del archivo es requerida")]
        [Display(Name = "Ruta")]
        public string Path { get; set; } = string.Empty;

        [NotMapped]
        public int Top_Aux { get; set; } // propiedad auxiliar
        public Ad Ad { get; set; } = new Ad(); // propiedad de navegación
    }
}
