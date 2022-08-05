using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.ViewModels
{
    public class MarkDepositOrderLogModel
    {
        [DisplayName("會員ID")]
        public Guid Id { get; set; }

        [DisplayName("Email")]
        public string MemberEmail { get; set; }

        [DisplayName("會員名稱")]
        public string MemberName { get; set; }

        [DisplayName("給付金額")]
        public int MemberCash { get; set; }


        [DisplayName("在製品名稱")]
        public string CaseName { get; set; }

        [DisplayName("客戶備註")]
        public string CaseRemarks1 { get; set; }
        [DisplayName("商家備註")]
        public string CaseRemarks2 { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        [DisplayName("建立日期")]
        public DateTime CreateDate { get; set; }
    }
}
