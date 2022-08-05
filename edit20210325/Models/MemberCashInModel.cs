using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.Models
{
    public class MemberCashInModel
    {
        [Key]
        [Column(Order = 1)]
        [Required(ErrorMessage = "請輸入會員信箱")]
        [DisplayName("會員信箱")]
        public string MemberCashInMemberGmail { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required(ErrorMessage = "請輸入訂單編號")]
        [DisplayName("訂單編號")]
        public string MemberCashInOrderID { get; set; }

        [DisplayName("商品版本")]
        public string MemberCashInVersion { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "已付訂金欄位錯誤")]
        [DisplayName("已付訂金")]
        public int MemberCashInCash { get; set; }


        [DisplayName("訂製商品")]
        public string MemberCashInName { get; set; }

        [DisplayName("進度說明")]
        public string MemberCashInRemarks { get; set; }

        [DisplayName("給付原因")]
        public string MemberCashInSpace1 { get; set; }

        [DisplayName("客戶備註")]
        public string MemberCashInSpace2 { get; set; }

        [DisplayName("商家備註")]
        public string MemberCashInSpace3 { get; set; }

        [DisplayName("訂單狀態")]
        public string OrderState { get; set; }


        [DataType(DataType.Date, ErrorMessage = "日期錯誤")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("建立日期")]
        public DateTime MemberCashInCrtDate { get; set; }


        [DataType(DataType.Date, ErrorMessage = "日期錯誤")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("最後變更日期")]
        public DateTime MemberCashInChgDate { get; set; }

        //用不到後續要delete

        [DisplayName("編號")]
        public Guid Id { get; set; }


        [DisplayName("會員帳號")]
        public string MemberCashInId { get; set; }
    }
}
