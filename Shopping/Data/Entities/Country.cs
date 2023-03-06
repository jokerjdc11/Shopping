using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [Display(Name ="País")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caractéres.")]
        [Required(ErrorMessage ="El campo {0} es obligatorio.")]
        public string Name { get; set; }

        //propiedad para tener varios estados
        public ICollection<State> States { get; set; }

        //operador ternario es con un if dentro de la misma sentencia
        [Display(Name = "Departamentos/Estados")]
        public int StatesNumber => States == null ? 0 : States.Count;
    }
}
