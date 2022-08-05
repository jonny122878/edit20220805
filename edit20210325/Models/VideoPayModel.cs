using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.Models
{
    public class VideoPayModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("付費影片ID")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "請上傳圖片")]
        [DisplayName("上傳圖片")]
        public string videoImageUrl { get; set; }

        [Required(ErrorMessage = "請上傳影片")]
        [DisplayName("上傳影片")]
        public string videoUrl { get; set; }

        [Required(ErrorMessage = "請輸入影片標題")]
        [DisplayName("影片標題")]
        public string videoTitle { get; set; }

        [DisplayName("影片介紹")]
        public string videoIntro { get; set; }

    }
}
