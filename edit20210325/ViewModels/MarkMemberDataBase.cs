using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace edit20210325.ViewModels
{
    public class MarkMemberDataBase
    {
        [DisplayName("會員ID")]
        public Guid MemberId { get; set; }
        [DisplayName("會員帳號")]
        public string MemberGmail { get; set; }
        [DisplayName("姓名")]
        public string MemberName { get; set; }
        [DisplayName("身分證字號/統一編號")]
        public string MemberIden { get; set; }
        [DisplayName("手機")]
        public string MemberPhone { get; set; }
        [DisplayName("任職公司")]
        public string MemberCompany { get; set; }
        [DisplayName("目前金幣數")]
        public int MemberMoney { get; set; }
    }
}
