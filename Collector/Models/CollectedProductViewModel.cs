using System.Collections.Generic;

namespace Collector.Models
{
    public class CollectedProductViewModel
    {
        public CollectedProductViewModel()
        {
            ProductImages = new List<CollectedProductImageViewModel>();
        }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductDiscountPrice { get; set; }
        public string ProductCurrency { get; set; }
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }
        public IEnumerable<CollectedProductImageViewModel> ProductImages { get; set; }
    }
}