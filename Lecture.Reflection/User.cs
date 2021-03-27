using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lecture.Reflection
{
    public class Usere
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [ColumnName("first_name")]
        public string FirstName { get; set; }
    }
}
