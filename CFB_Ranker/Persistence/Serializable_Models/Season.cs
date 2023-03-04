using CFB_Ranker.Persistence.Serializable_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CFB_Ranker.Persistence.Serialization
{
    [Serializable]
    public class Season : ISer
    {
        public int Year { get; set; }
        public List<SerSchool> Schools { get; set; } = new();
        public List<SerGame> Games { get; set; } = new();
    }
}
