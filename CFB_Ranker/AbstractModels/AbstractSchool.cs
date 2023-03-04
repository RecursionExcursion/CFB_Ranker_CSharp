namespace CFB_Ranker.AbstractModels
{
    [Serializable]
    public abstract class AbstractSchool
    {
        public string Id { get; set; }
        public string School { get; set; }
        public string Mascot { get; set; }
        public string Color { get; set; }
        public string Alt_Color { get; set; }
        public string[] Logos { get; set; }

        protected AbstractSchool(string id, string school, string mascot, string color, string alt_Color, string[] logos)
        {
            Id = id;
            School = school;
            Mascot = mascot;
            Color = color;
            Alt_Color = alt_Color;
            Logos = logos;
        }
    }
}
