using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.Models
{
    public class ManagerModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("管理者ID")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "請輸入正確的Email")]
        [DisplayName("管理者帳號")]
        public string　ManagerUser { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password, ErrorMessage = "請輸入密碼")]
        [DisplayName("管理者密碼")]
        public string ManagerPassword { get; set; }

        [Required(ErrorMessage = "請輸入名稱")]
        [DisplayName("管理者名稱")]
        public string ManagerName { get; set; }

    }
}
