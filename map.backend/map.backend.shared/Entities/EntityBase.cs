using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map.backend.shared.Entities
{
    public class EntityBase
    {
        [Key] public long id { get; set; }
        [MaxLength(1)]
        public string? record_stat { get; set; }
        public int? mod_no { get; set; }
        [MaxLength(50)]
        public string? create_by { get; set; }
        public DateTime? create_date { get; set; }
        [MaxLength(50)]
        public string? modify_by { get; set; }
        public DateTime? modify_date { get; set; }
    }
}
