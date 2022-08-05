using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.ViewModels
{
    public class MarkSalesOrderModel
    {
        [DisplayName("商品ID")]
        public Guid Id { get; set; }

        [DisplayName("會員帳號")]
        public string MemberEmail { get; set; }

        [DisplayName("會員名稱")]
        public string MemberName { get; set; }

        [DisplayName("商品名稱")]
        public string ProductName { get; set; }

        [DisplayName("商品介紹")]
        public string ProductInfo { get; set; }


        [DisplayName("計費模式")]
        public string ProductCharge { get; set; }


        [DisplayName("累計使用次數")]
        public int ProductCounts { get; set; }

        [DisplayName("單位天數")]
        public int ProductUnitDays { get; set; }

        [DisplayName("單位價格")]
        public int ProductUnitPrice { get; set; }

        [DisplayName("客戶備註")]
        public string ProductRemarks1 { get; set; }

        [DisplayName("商家備註")]
        public string ProductRemarks2 { get; set; }


        [DisplayName("天數")]
        public int ProductDays { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("建立日期")]
        public DateTime ProductCreateDate { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        [DisplayName("首次啟用日")]
        public DateTime ProductChangeDate { get; set; }
    }
}
