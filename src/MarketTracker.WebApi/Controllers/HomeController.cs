using MarketTracker.WebApi.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MarketTracker.WebApi.Controllers
{
    [ApiController]
    [Route("home")]
    public class HomeController
    {
        private readonly IHomeQuery _homeQuery;

        public HomeController(IHomeQuery homeQuery)
        {
            _homeQuery = homeQuery;
        }

        [HttpGet]
        public string Get()
        {
            return _homeQuery.GetHome();
        }
    }
}
