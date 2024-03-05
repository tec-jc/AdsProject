using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.BussinessEntities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Role")]
        [Required(ErrorMessage = "El rol es requerido")]
        [Display(Name = "Rol")]
        public int IdRole { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(30, ErrorMessage = "Máximo 30 caracteres")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [MaxLength(30, ErrorMessage = "Máximo 30 caracteres")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [MaxLength(25, ErrorMessage = "Máximo 25 caracteres")]
        [Display(Name = "Nombre de usuario")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "La contraseña debe tener entre 6 y 32 caracteres", MinimumLength =  6)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es requerido")]
        [Display(Name = "Estado")]
        public byte Status { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime RegistrationDate { get; set; }

        [NotMapped]
        public int Top_Aux { get; set; } // propiedad auxiliar

        [NotMapped]
        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "La contraseña debe tener entre 6 y 32 caracteres", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar la contraseña")]
        public string ConfirmPassword_Aux { get; set; } = string.Empty; // propiedad auxiliar
        
        public Role? Role { get; set; } // propiedad de navegación
    }

    public enum User_Status
    {
        ACTIVO = 1, INACTIVO = 2
    }
}
