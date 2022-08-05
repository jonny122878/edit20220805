using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.Models
{
    [Table("SalesOrder")]
    public class SalesOrderModel
    {
        [Key]
        [Column(Order = 1)]
        [Required(ErrorMessage = "請輸入會員信箱")]
        [DisplayName("會員信箱")]
        public string SalesOrderMemberGmail { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required(ErrorMessage = "請輸入訂單編號")]
        [DisplayName("訂單編號")]
        public string SalesOrderOrderID { get; set; }

        [Key]
        [Column(Order = 3)]
        [Required(ErrorMessage = "請輸入產品流水號")]
        [DisplayName("產品流水號")]
        public string SalesOrderItem { get; set; }


        [DisplayName("購買產品型式")]
        public string SalesOrderCategory { get; set; }


        [DisplayName("商品名稱")]
        public string ProductName { get; set; }

        [DisplayName("商品介紹")]
        public string ProductInfo { get; set; }





        [RegularExpression("^[0-9]*$", ErrorMessage = "輸入計次錯誤")]
        [DisplayName("累計使用次數")]
        public int ProductCounts { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "輸入天數錯誤")]
        [DisplayName("天數")]
        public int ProductDays { get; set; }

        [DisplayName("客戶備註")]
        public string ProductRemarks1 { get; set; }
        [DisplayName("商家備註")]
        public string ProductRemarks2 { get; set; }


        [DataType(DataType.Date, ErrorMessage = "日期錯誤")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("建立日期")]
        public DateTime ProductCreateDate { get; set; }


        [DataType(DataType.DateTime, ErrorMessage = "日期時間錯誤")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [DisplayName("首次啟用日")]
        public DateTime ProductChangeDate { get; set; }


        //用不到後續要delete




        [DisplayName("計費模式")]
        public string ProductCharge { get; set; }


        #region old code

        [RegularExpression("^[0-9]*$", ErrorMessage = "輸入收費單位天數欄位錯誤")]
        [DisplayName("單位天數")]
        public int ProductUnitDays { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "輸入收費單位價格欄位錯誤")]
        [DisplayName("單位價格")]
        public int ProductUnitPrice { get; set; }


        [DisplayName("商品ID")]
        public Guid Id { get; set; }


        [DisplayName("會員帳號")]
        public string MemberId { get; set; }
        #endregion
    }
}
