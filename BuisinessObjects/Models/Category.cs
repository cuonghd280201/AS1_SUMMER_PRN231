using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuisinessObjects.Models
{
    public partial class Category
    {
        public Category()
        {
            FlowerBouquets = new HashSet<FlowerBouquet>();
        }

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is required!!")]

        public string CategoryName { get; set; } = null!;

        [Required(ErrorMessage = "Category Name is required!!")]
        public string? CategoryDescription { get; set; }

        public virtual ICollection<FlowerBouquet> FlowerBouquets { get; set; }
    }
}
