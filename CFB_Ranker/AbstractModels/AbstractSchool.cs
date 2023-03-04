namespace CFB_Ranker.AbstractModels
{
    [Serializable]
    public abstract class AbstractSchool
    {
        public string Id { get; set; }
        public string School { get; set; }
        public string Mascot { get; set; }

        protected AbstractSchool(string id, string school, string mascot)
        {
            Id = id;
            School = school;
            Mascot = mascot;
        }
    }
}
