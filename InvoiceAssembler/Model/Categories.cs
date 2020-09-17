using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InvoiceAssembler
{
    public partial class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        public short CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Products> Products { get; set; }
    }
}
