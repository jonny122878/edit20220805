using System;
using System.Collections.Generic;
using System.ComponentModel;  
using System.ComponentModel.DataAnnotations;  
using System.Linq;
using System.Threading.Tasks;


namespace edit20210325.ViewModels
{
    public class ConsultNowViewModel
    {
        [DisplayName("請輸入您的E-mail")]
        [Required(ErrorMessage = "請填寫Email")]
        [EmailAddress(ErrorMessage ="Email格式有誤")]
        public string Email { get; set; }

        [DisplayName("請輸入您想諮詢的問題及需求")]
        [Required(ErrorMessage ="請填寫想諮詢的內容")]
        public string Consultation { get; set; }

        public string ConsultationStatus { get; set; }






    }
}
