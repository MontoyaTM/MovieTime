namespace MovieTime.Components.Models
{

    public class CreditsResponse
    {
        public int id { get; set; }
        public List<Cast> Cast { get; set; } = [];
        public List<Crew> Crew { get; set; } = [];
    }

    public class Cast
    {
        public bool Adult { get; set; }
        public int Gender { get; set; }
        public int Id { get; set; }
        public string KnownForDepartment { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public float Popularity { get; set; }
        public string ProfilePath { get; set; }
        public int CastId { get; set; }
        public string Character { get; set; }
        public string CreditId { get; set; }
        public int Order { get; set; }
    }

    public class Crew
    {
        public bool Adult { get; set; }
        public int Gender { get; set; }
        public int Id { get; set; }
        public string KnownForDepartment { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public float Popularity { get; set; }
        public string ProfilePath { get; set; }
        public string CreditId { get; set; }
        public string Department { get; set; }
        public string Job { get; set; }
    }

}
