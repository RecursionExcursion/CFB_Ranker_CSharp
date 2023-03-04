using CFB_Ranker.AbstractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFB_Ranker.API.DTO
{
    /*
     * DTO object used to transfer raw data from JSON
     */
    public class SchoolDTO : AbstractSchool
    {
        public SchoolDTO(string id, string school, string mascot) : base(id, school, mascot) { }
    }
}
