using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utulek1.Domain.Entities
{
    [Table("AppLog")]
    public class SystemLog
    {
        [Key]
        public int Id { get; set; }

        public string? Message { get; set; }

        public string? MessageTemplate { get; set; }

        [Column("LogLevel")]
        [StringLength(128)]
        public string? Level { get; set; }

        public DateTime TimeStamp { get; set; }

        public string? Exception { get; set; }

        public string? Properties { get; set; } 
    }
}
