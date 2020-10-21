using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceAssembler.Model
{
    #region NoTimeTravel
    public class Discounts
    {
        public Discounts()
        {
            Values = new List<Discount>();
        }

        public List<Discount> Values { get; set; }

        public class Discount
        {
            public Discount(int productId, float percent)
            {
                ProductId = productId;
                Percent = percent;
            }

            public int ProductId { get; private set; }
            public float Percent { get; private set; }
        }

        public override string ToString()
        {
            if (Values == null || Values.Count == 0)
            {
                return base.ToString();
            }
            var ret = "";
            foreach (var val in Values)
            {
                ret += $"{{productId:{val.ProductId}, discount:{val.Percent}%}}";
            }

            return ret;
        }
    }
    #endregion
}
