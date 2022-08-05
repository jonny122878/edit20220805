using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.Models
{
    public class MemberModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("會員ID")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "請輸入正確的Email")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "請輸入正確的Email")]
        [DisplayName("會員帳號")]
        public string MemberGmail { get; set; }

        [DataType(DataType.Password,ErrorMessage = "請輸入密碼")]
        [DisplayName("會員密碼")]
        public string MemberPassword { get; set; }

        [Required(ErrorMessage = "請輸入姓名")]
        [DisplayName("姓名")]
        public string MemberName { get; set; }

        [Required(ErrorMessage = "請輸入正確的身分證字號/統一編號")]
        [DisplayName("身分證字號/統一編號")]
        public string MemberIden { get; set; }

        [Required(ErrorMessage = "請輸入手機號碼")]
        [RegularExpression("^09[0-9]{8}$", ErrorMessage = "請輸入手機號碼")]
        [DisplayName("手機號碼")]
        public string MemberPhone { get; set; }

        [DisplayName("任職公司")]
        public string MemberCompany { get; set; }

        [DisplayName("備註1")]
        public string MemberRemarks1 { get; set; }

        [DisplayName("備註2")]
        public string MemberRemarks2 { get; set; }

        [DisplayName("保留欄位1")]
        public string MemberSpace1 { get; set; }

        [DisplayName("保留欄位2")]
        public string MemberSpace2 { get; set; }

        [Required(ErrorMessage = "日期錯誤")]
        [DataType(DataType.Date, ErrorMessage = "日期錯誤")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("建立日期")]
        public DateTime MemberCreateDate { get; set; }

        [Required(ErrorMessage = "日期錯誤")]
        [DataType(DataType.Date, ErrorMessage = "日期錯誤")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("最後變更日期")]
        public DateTime MemberChangeDate { get; set; }

    }
}
