using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Cella.Models.ViewModels;

namespace Cella.Models
{
    public class SalesOrder
    {

        public enum SalesOrderType
        {
            SalesOrder = 1,
            Bom = 2,
            PurchaseOrder = 3,
            Quote = 4,
            PickList = 5,
            Invoice = 6,
            Despatched = 7,
            DespatchedPartial = 8

        }

        [Display(Name = "Order Number")]
        public int Id { get; set; }
        public Guid? UserId { get; set; }

        public Guid? TeannatId { get; set; }

        public string? Description { get; set; }

        public string? GiftMessage { get; set; }

        public ICollection<Customer> Customer { get; set; }

        public int? Customer { get; set; }

        public int? PaymentStatus { get; set; }

        public int? BillingAddress { get; set; }

        public int? ShippingAddress { get; set; }


        public Guid? StoreId { get; set; }


        public DateTime? CreatedOn { get; set; }


        public int? OrderType { get; set; }

        public decimal? OrderSubTotal { get; set; }

        public decimal? OrderShipping { get; set; }

        public decimal? OrderTax { get; set; }

        public decimal? OrderTotal { get; set; }

        public int? PaymentMethod { get; set; }
        [StringLength(50)]
        public string? IpAddress { get; set; }
        public bool? isGiftWrapping { get; set; }
        [StringLength(5000)]
        public string? GiftMessage { get; set; }

        public Guid? CouponCode { get; set; }


        public bool? GiftCard { get; set; }
        public decimal? Profit { get; set; }

        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        [StringLength(30)]
        public string? CreatedBy { get; set; }

        public bool? isActive { get; set; }

        public bool? isDeleted { get; set; }

        public List<SalesOrderItem> Items { get; set; }
        public List<SalesOrderLine> Lines { get; set; } = new();
        public string? OtpCode { get; set; }

    }
}
