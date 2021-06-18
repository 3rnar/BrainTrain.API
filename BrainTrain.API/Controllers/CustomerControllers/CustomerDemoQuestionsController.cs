using BrainTrain.API.Helpers.Learnosity;
using BrainTrain.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BrainTrain.API.Controllers.CustomerControllers
{
    [Authorize(Roles = "Обычный пользователь")]
    [Route("api/Customer/DemoQuestions")]
    public class CustomerDemoQuestionsController : BaseApiController
    {
        public CustomerDemoQuestionsController(BrainTrainContext _db) : base(_db)
        {
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetLRNQuestions()
        {
            var lRNQuestions = await db.LRNQuestions.Where(l => l.IsPrecalculus == false).ToListAsync();

            string uuid = "";

            var qJson = LRNQuestionsHelper.Simple(lRNQuestions, out uuid);
            
            return Ok(qJson);
        }

        [HttpGet]
        [Route("PrecalculusQuestions")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPrecalculusQuestions()
        {
            var questions = await db.LRNQuestions.Where(l => l.IsPrecalculus == null ||  l.IsPrecalculus == true ).ToListAsync();

            var uuid = "";
            var json = LRNQuestionsHelper.Simple(questions, out uuid);

            return Ok(json);
        }
    }
}
