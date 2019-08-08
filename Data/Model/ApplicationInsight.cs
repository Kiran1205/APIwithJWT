using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAPICore.Data.Model
{
    public class ApplicationInsight
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }

        public string IpAddress { get; set; }
        public int UserId { get; set; }
    }
}
