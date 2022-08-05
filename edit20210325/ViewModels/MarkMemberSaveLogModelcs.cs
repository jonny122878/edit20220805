using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.ViewModels
{
    public class MarkMemberSaveLogModelcs
    {
        [DisplayName("會員加值ID")]
        public Guid Id { get; set; }

        [DisplayName("Email")]
        public string MemberEmail { get; set; }

        [DisplayName("會員名稱")]
        public string MemberName { get; set; }

        [DisplayName("加值金額")]
        public int MemberCash { get; set; }

        [DisplayName("備註")]
        public string Remark { get; set; }

        [Required(ErrorMessage = "日期錯誤")]
        [DataType(DataType.Date, ErrorMessage = "日期錯誤")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("建立日期")]
        public DateTime CreateDate { get; set; }

    }
}
