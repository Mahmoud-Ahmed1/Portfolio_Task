namespace PersonalPortfolio.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PortfolioItemId { get; set; }
        public PortfolioItem PortfolioItem { get; set; }
    }
}
