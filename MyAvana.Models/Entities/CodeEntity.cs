using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvanaApi.Models.Entities
{
    public class CodeEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string AccountId { get; set; }
        public Operation OpCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
    public enum Operation
    {
        CodeVerify,
        ForgetPassword
    }
}
