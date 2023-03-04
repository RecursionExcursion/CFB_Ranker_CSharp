using CFB_Ranker.AbstractModels;
using CFB_Ranker.Persistence.Serializable_Models;
using Newtonsoft.Json;

namespace CFB_Ranker.Persistence.Serialization
{
    [Serializable]
    public class SerSchool : AbstractSchool, ISer
    {
        [JsonConstructor]
        public SerSchool(string id, string school, string mascot, string color, string alt_Color, string[] logos)
            : base(id, school, mascot, color, alt_Color, logos) { }

        public SerSchool(AbstractSchool school) :
            this(school.Id, school.School, school.Mascot, school.Color, school.Alt_Color, school.Logos) { }
    }
}
