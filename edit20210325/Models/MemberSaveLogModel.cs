using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.Models
{
    public class MemberSaveLogModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("編號")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "請輸入會員帳號")]
        [DisplayName("會員帳號")]
        public string MemberSaveId { get; set; }

        [Required(ErrorMessage = "請輸入金幣")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "輸入金幣錯誤")]
        [DisplayName("加值金額")]
        public int MemberSaveCash { get; set; }

        [DisplayName("備註")]
        public string MemberSaveRemarks { get; set; }

        [Required(ErrorMessage = "日期錯誤")]
        [DataType(DataType.Date, ErrorMessage = "日期錯誤")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        [DisplayName("建立日期")]
        public DateTime MemberSaveCreateDate { get; set; }
    }
}
