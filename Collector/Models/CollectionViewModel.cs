using System.Collections.Generic;

namespace Collector.Models
{
    public class CollectionViewModel
    {
        public CollectionViewModel()
        {
            ProductViews = new List<CollectedProductViewModel>();
        }
        public string CollectionUrl { get; set; }
        public IEnumerable<CollectedProductViewModel> ProductViews { get; set; }
    }
}