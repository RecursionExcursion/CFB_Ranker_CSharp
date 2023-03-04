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
    public class GameInfoDTO : AbstractGameInfo
    {
        public GameInfoDTO(string id, string season, string week, string season_Type, string start_Date, string home_Id, string away_Id)
            : base(id, season, week, season_Type, start_Date, home_Id, away_Id) { }
    }
}
