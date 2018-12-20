using Nice.DataAccess.Attributes;
using Nice.DataAccess.Models;
using System;

namespace Nice.Code.Demo
{

    [Table(Name = "tbl_user_info")]
    public class UserInfo : TEntity
    {
        [Id(GenerateType = IdGenerateType.Assign)]
        public string UserId { get; set; }
        public string RealName { get; set; }
        public string UserName { get; set; }
        [Column(IsReadOnly = true)]
        public string SPassword { get; set; }
        public string SEmail { get; set; }
        public string STelephone { get; set; }
        public string SRemark { get; set; }
        [Column(IsReadOnly = true)]
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        [Column(IsReadOnly = true)]
        public string CreateUserId { get; set; }
        public string ModifyUserId { get; set; }
        [Valid(1, 0)]
        [Column(Name ="nState")]
        public byte NState { get; set; }
        [Column(IsReadOnly = true)]
        public bool IsReserve { get; set; }
    }
}
