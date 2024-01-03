namespace PersonalPortfolio.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PortfolioItemId { get; set; }
        public PortfolioItem PortfolioItem { get; set; }
    }
}
