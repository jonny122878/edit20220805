using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.ViewModels
{
    public class MarkFileLoadInfoModel
    {
        [DisplayName("檔案ID")]
        public Guid Id { get; set; }

        [DisplayName("會員帳號")]
        public string MemberGmail { get; set; }

        [DisplayName("會員帳號")]
        public string MemberName { get; set; }

        [DisplayName("檔案標題")]
        public string FileTitle { get; set; }

        [DisplayName("上傳程式檔案")]
        public string FileUrl { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        [DisplayName("更新時間")]
        public DateTime FileUpDateTime { get; set; }
    }
}
