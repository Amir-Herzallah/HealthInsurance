using HealthInsurance.Models;

namespace HealthInsurance.Controllers
{
    public interface IHomePageService
    {
        List<HomePage> GetHomePageContent();
    }

    public class HomePageService : IHomePageService
    {
        private readonly ModelContext _dbContext;

        public HomePageService(ModelContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<HomePage> GetHomePageContent()
        {
            return _dbContext.HomePage.ToList();
        }
    }
}
