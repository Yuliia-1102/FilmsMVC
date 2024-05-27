using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmsDomain.Model
{
    public class Query
    {
        public string QueryName { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
        public string CustomerName { get; set; }

        public List<string> FilmNames { get; set; } = new List<string>();

        [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
        public string FilmName { get; set; }

        public List<string> ActorNames { get; set; } = new List<string>();

        [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
        public string DirectorName { get; set; }
        public float TotalPrice { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
        public string GenreName { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
        [Range(1900, 2024, ErrorMessage = "Рік повинен бути між 1900 та 2024.")]
        public int? ReleaseYear { get; set; }
        public List<string> TrailerLinks { get; set; } = new List<string>();

        [Required(ErrorMessage = "Поле не повинно бути порожнім.")]
        [Range(0, float.MaxValue, ErrorMessage = "Будь ласка, введіть коректну ціну.")]
        public float? Price { get; set; }

        public List<string> CustomerNames { get; set; } = new List<string>();

        public List<string> CustomerEmails { get; set; } = new List<string>();

        public string ErrorName { get; set; }

        public int ErrorFlag { get; set; }
    }
}
