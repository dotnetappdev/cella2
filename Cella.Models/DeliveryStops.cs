using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cella.Models
{
   public  class DeliveryStops
    {

        public int Id { get; set; }



        public Guid? StoreId { get; set; }

        public Guid? UserId { get; set; }


        public decimal Lat { get; set; }

        public decimal Long { get; set; }


        public virtual Drivers Driver { get; set; }

        public virtual int? DeliveryAddress { get; set; }

    }
}
