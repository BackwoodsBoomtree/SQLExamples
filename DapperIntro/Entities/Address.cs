using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperIntro.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public int PersonID { get; set; }
    }
}
