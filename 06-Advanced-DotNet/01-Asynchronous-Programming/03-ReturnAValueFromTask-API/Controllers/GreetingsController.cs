using Microsoft.AspNetCore.Mvc;

namespace _03_ReturnAValueFromTask_API.Controllers
{
    public class GreetingsController : Controller
    {
        //api/greetings/name
        [Route("api/greetings/{name}")]
        [HttpGet]
        public string GetGreetings(string name)
        {
            return $"Hello {name}, Welcome to Web API";
        }
    }
}
