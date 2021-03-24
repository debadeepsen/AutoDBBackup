using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDBBackup.Utils
{
    public class Db
    {
        public string Name { get; set; }
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class DbList
    {
        public List<Db> List { get; } = new List<Db>();
    }
}
