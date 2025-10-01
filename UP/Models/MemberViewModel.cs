using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP.Models
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public string UserEmail { get; set; }
        public string Role { get; set; }
    }
}
