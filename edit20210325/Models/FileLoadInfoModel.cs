using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.Models
{
    public class FileLoadInfoModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("檔案ID")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "會員帳號")]
        [DisplayName("會員帳號")]
        public string MmberId { get; set; }

        [Required(ErrorMessage = "檔案標題")]
        [DisplayName("檔案標題")]
        public string FileTitle { get; set; }

        [DisplayName("檔案介紹")]
        public string FileInfo { get; set; }

        [DisplayName("網卡編號")]
        public string FileMac { get; set; }

        [DisplayName("上傳程式檔案")]
        public string FileUrl { get; set; }

        [Required(ErrorMessage = "請輸入時間")]
        [DataType(DataType.DateTime, ErrorMessage = "日期時間錯誤")]
        [DisplayName("更新時間")]
        public DateTime FileUpDateTime { get; set; }
        
    }
}
