using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.ViewModels
{
    public class MarkMemberCashInModel
    {
        [DisplayName("會員ID")]
        public Guid Id { get; set; }

        [DisplayName("Email")]
        public string MemberEmail { get; set; }

        [DisplayName("會員名稱")]
        public string MemberName { get; set; }

        [DisplayName("金額")]
        public int MemberCash { get; set; }

        [DisplayName("訂金專案名稱")]
        public string CaseName { get; set; }

        [DisplayName("進度說明")]
        public string CaseRemarks { get; set; }

        [DisplayName("給付原因")]
        public string Space1 { get; set; }

        [DisplayName("客戶備註")]
        public string Space2 { get; set; }
        [DisplayName("商家備註")]
        public string Space3 { get; set; }

        [Required(ErrorMessage = "日期錯誤")]
        [DataType(DataType.Date, ErrorMessage = "日期錯誤")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("建立日期")]
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "日期錯誤")]
        [DataType(DataType.Date, ErrorMessage = "日期錯誤")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("最後變更日")]
        public DateTime ChangeDate { get; set; }

    }
}
