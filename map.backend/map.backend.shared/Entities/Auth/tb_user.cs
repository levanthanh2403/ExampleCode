using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map.backend.shared.Entities.Auth
{
    public class tb_user : EntityBase
    {
        [MaxLength(50)]
        public string userid { get; set; }
        [MaxLength(255)]
        public string? username { get; set; }
        [MaxLength(255)]
        public string password { get; set; }
        [MaxLength(100)]
        public string? email { get; set; }
        [MaxLength(15)]
        public string? phone { get; set; }
        [NotNull]
        public int limit { get; set; } = 5;
        [MaxLength(1)]
        public string status { get; set; }
        public string? img { get; set; }
    }
}
