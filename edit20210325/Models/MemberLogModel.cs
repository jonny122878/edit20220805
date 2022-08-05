using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace edit20210325.Models
{
    public class MemberLogModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("會員ID")]
        public Guid Id { get; set; }

        public string MAC { get; set; }

        [DisplayName("登入日期")]
        public DateTime LoginDate { get; set; }
    }
}
