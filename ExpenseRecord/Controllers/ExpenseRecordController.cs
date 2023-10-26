using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using ExpenseRecord.Models;
using ExpenseRecord.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpenseRecord.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [Produces("application/json")]
    public class ExpenseRecordController : ControllerBase
    {
        private readonly IExpenseRecordService _ExpenseRecordService; //implement service interface and call it _ExpenseRecordService
        private readonly ILogger<ExpenseRecordController> _logger;


        public ExpenseRecordController(IExpenseRecordService InMemoryExpenseRecordService, ILogger<ExpenseRecordController> logger)
        {
            _ExpenseRecordService = InMemoryExpenseRecordService;
            _logger = logger;

        }


        [HttpGet]
        [ProducesResponseType(typeof(List<ExpenseRecordDto>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Get All",
            Description = "Get all to-do items"
            )] //define infro on swagger?
        public async Task<ActionResult<List<ExpenseRecordDto>>> GetAsync()  //get has no request body, only respond body
        {
            var result = await _ExpenseRecordService.GetAsync(); // get all items from service
            return Ok(result); //respnose from backend
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExpenseRecordDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Get By Id",
            Description = "Get to-do item by id"
            )]
        public async Task<ActionResult<ExpenseRecordDto>> GetAsync(string id)
        {
            var result = await _ExpenseRecordService.GetAsync(id);
            if (result == null)
            {
                return NotFound($"The item with id {id} does not exist.");
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ExpenseRecordDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Create New Item",
            Description = "Create a new to-do item"
            )]

        public async Task<ActionResult<ExpenseRecordDto>> PostAsync([FromBody] ExpenseRecordCreateRequest ExpenseRecordCreateRequest)
        {
            var ExpenseRecordDto = new ExpenseRecordDto //new a new item and define its property
            { //actually need to new ExpenseRecord, but it is equivalent to ExpenseRecorddto
                Description = ExpenseRecordCreateRequest.Description, //request property from frontend
                Type = ExpenseRecordCreateRequest.Type,
                Amount = ExpenseRecordCreateRequest.Amount,
                CreatedTime = DateTime.Now
            };
            await _ExpenseRecordService.CreateAsync(ExpenseRecordDto); //create item in database
            return Created("", ExpenseRecordDto); //respond new item to frontend
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ExpenseRecordDto), 200)]
        [ProducesResponseType(typeof(ExpenseRecordDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Upsert Item",
            Description = "Create or replace a to-do item by id"
            )]
        public async Task<ActionResult<ExpenseRecordDto>> PutAsync(string id, [FromBody] ExpenseRecordDto ExpenseRecordDto) //second tooitemdto是传进函数的变量
        { //frombody=found using request body
            bool isCreate = false; //no need to create
            var existingItem = await _ExpenseRecordService.GetAsync(id);
            if (existingItem is null)
            {
                isCreate = true; //need to create
                await _ExpenseRecordService.CreateAsync(ExpenseRecordDto); //ExpenseRecordDto defines the property/格式 of an item
            }
            else
            {
                await _ExpenseRecordService.ReplaceAsync(id, ExpenseRecordDto);
            }

            return isCreate ? Created("", ExpenseRecordDto) : Ok(ExpenseRecordDto);
            //? means if isCreate true, execute the first one, if false, execute the second one
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Delete Item",
            Description = "Delete a to-do item by id"
            )]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var isSuccessful = await _ExpenseRecordService.RemoveAsync(id); //remove()是数据库的删，delete是controller/api里面的删
            if (!isSuccessful) // !means if not successful
            {
                return NotFound($"The item with id {id} does not exist.");
            }
            _logger.LogInformation($"To-do item {id} deleted."); //deliver infromation to log?
            return NoContent(); //dont return anything bc deleted
        }
    }
}
