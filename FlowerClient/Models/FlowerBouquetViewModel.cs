using System.ComponentModel.DataAnnotations;

namespace FlowerManagementWebClient.ViewModels
{
    public class FlowerBouquetViewModel
    {
        [Required]
        [Display(Name = "Flower Bouquet Id")]
        public int FlowerBouquetId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Flower Bouquet Name")]
        public string FlowerBouquetName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Display(Name = "Units In Stock")]
        public int UnitsInStock { get; set; }

        [Required]
        [Display(Name = "Flower Bouquet Status")]
        public byte? FlowerBouquetStatus { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }
    }
}
