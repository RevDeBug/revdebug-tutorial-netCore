using Newtonsoft.Json;

namespace InvoiceAssembler
{
    public partial class OrderDetails
    {
        public short OrderId { get; set; }
        public short ProductId { get; set; }
        public float UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Orders Order { get; set; }
        public virtual Products Product { get; set; }
    }
}
