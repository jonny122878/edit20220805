using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.Models
{
    public class VideoFreeModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("免費影片ID")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "請上傳影片")]
        [Url(ErrorMessage = "請輸入網址")]
        //[RegularExpression("https:\\/\\/(www|m).youtube.com\\/watch\\?v=[\\w-]*&?[\\w=%-]*", ErrorMessage = "只接受來自youtube的影片 (或是你嘗試輸入youtube播放清單的影片)")]
        //[RegularExpression(@"(?:https?:\/\/)?(?:www\.)?youtu(?:\.be\/|be.com\/\S*(?:watch|embed)(?:(?:(?=\/[^&\s\?]+(?!\S))\/)|(?:\S*v=|v\/)))([^&\s\?]+)", ErrorMessage = "只接受來自youtube的影片 (或是你嘗試輸入youtube播放清單的影片)")]
        //[RegularExpression(@"http(?:s?):\/\/(?:www\.)?youtu(?:be\.com\/watch\?v=|\.be\/)([\w\-\_]*)(&(amp;)?‌​[\w\?‌​=]*)?", ErrorMessage = "只接受來自youtube的影片 (或是你嘗試輸入youtube播放清單的影片)")]
        //[RegularExpression("https:\\/\\/youtu.be\\/^[a-zA-Z0-9]*$", ErrorMessage = "只接受來自youtube的影片 (或是你嘗試輸入youtube播放清單的影片)")]
        //(?:https?:\/\/)?(?:(www|m)\.)?youtu(?:\.be\/|be.com\/\S*(?:watch|embed)(?:(?:(?=\/[^&\s\?]+(?!\S))\/)|(?:\S*v=|v\/)))([^&\s\?]+)((\&[A-Z0-9+&@#\/%=~_|$]+)?)
        //[RegularExpression(@"(?:https?:\/\/)?(?:(www|m)\.)?youtu(?:\.be\/|be.com\/\S*(?:watch|embed)(?:(?:(?=\/[^&\s\?]+(?!\S))\/)|(?:\S*v=|v\/)))([^&\s\?]+)?((\&[A-Z0-9+&@#\/%=~_|$]+)?)", ErrorMessage = "只接受來自youtube的影片 (或是你嘗試輸入youtube播放清單的影片")]
        [DisplayName("影片網址")]
        public string videoUrl { get; set; }
        public string videoId { get; set; }

        [Required(ErrorMessage = "請輸入影片標題")]
        [DisplayName("影片標題")]
        public string videoTitle { get; set; }

        [DisplayName("影片介紹")]
        public string videoIntro { get; set; }

        [Required(ErrorMessage = "請輸入集數")]
        [DisplayName("集數")]
        public string videoCount { get; set; }

        [DisplayName("備註1")]
        public string videoRemark1 { get; set; }
        [DisplayName("備註2")]
        public string videoRemark2 { get; set; }
    }
}
