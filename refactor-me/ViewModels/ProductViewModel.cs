using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.ViewModels
{
    public class ProductViewModel
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DeliveryPrice { get; set; }
    }

    public class ProductItemsViewModel
    {
        [JsonProperty]
        public IEnumerable<ProductViewModel> Items {get; set;}
    }
}