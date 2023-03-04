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
        public SchoolDTO(string id, string school, string mascot, string color, string alt_Color, string[] logos) 
            : base(id, school, mascot, color, alt_Color, logos) { }
    }
}
