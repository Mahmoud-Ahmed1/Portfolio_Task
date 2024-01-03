namespace PersonalPortfolio.Models
{
    public class PortfolioItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Skill> Skills { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
