using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.ViewModels
{
    public partial class ProductOptionViewModel
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProductOptionItemsViewModel
    {
        [JsonProperty]
        public IEnumerable<ProductOptionViewModel> Items { get; set; }
    }
}