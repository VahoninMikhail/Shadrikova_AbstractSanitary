using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractSanitaryModel
{
    public class Plumber
    {
        public int Id { get; set; }

        [Required]
        public string PlumberFIO { get; set; }

        [ForeignKey("PlumberId")]
        public virtual List<Ordering> Orderings { get; set; }
    }
}
