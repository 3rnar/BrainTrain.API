using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainTrain.Core.Models
{
    public class ThemesTrees
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("FirstTheme")]
        public int FirstThemeId { get; set; }

        [ForeignKey("SecondTheme")]
        public int SecondThemeId { get; set; }

        public virtual Theme FirstTheme { get; set; }
        public virtual Theme SecondTheme { get; set; }
    }
}
