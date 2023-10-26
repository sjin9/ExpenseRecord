using ExpenseRecord.Models;

namespace ExpenseRecord.Services
{
    public class InMemoryExpenseRecordService : IExpenseRecordService
    {
        private static readonly List<ExpenseRecordDto> _ExpenseRecords = new();

        public Task CreateAsync(ExpenseRecordDto newExpenseRecord)
        {
            _ExpenseRecords.Add(newExpenseRecord);
            return Task.CompletedTask;
        }


        public Task<List<ExpenseRecordDto>> GetAsync()
        {
            return Task.FromResult(_ExpenseRecords);
            /*return _ExpenseRecords.Select(GenerateRecordDto).OrderByDescending(item => item.CreatedTime).ToList();*/

        }

        public Task<ExpenseRecordDto?> GetAsync(string id)
        {
            var ExpenseRecord = _ExpenseRecords.Find(x => x.Id == id);
            return Task.FromResult(ExpenseRecord);

        }



        public Task<bool> RemoveAsync(string id)
        {
            var itemToBeRemoved = _ExpenseRecords.Find(x => x.Id == id);
            if (itemToBeRemoved is null)
            {
                return Task.FromResult(false);
            }
            _ExpenseRecords.Remove(itemToBeRemoved);
            return Task.FromResult(true);
        }



        public Task ReplaceAsync(string id, ExpenseRecordDto updatedExpenseRecord)
        {
            var index = _ExpenseRecords.FindIndex(x => x.Id == id);
            if (index >= 0)
            {
                updatedExpenseRecord.CreatedTime = _ExpenseRecords[index].CreatedTime;
                _ExpenseRecords[index] = updatedExpenseRecord;
            }
            return Task.CompletedTask;
        }

        private ExpenseRecordDto GenerateRecordDto(ExpenseRecordDto expenseRecord)
        {
            return new ExpenseRecordDto
            {
                Description = expenseRecord.Description,
                Type = expenseRecord.Type,
                Amount = expenseRecord.Amount,
                CreatedTime = expenseRecord.CreatedTime,
                Id = expenseRecord.Id,
            };
        }
    }
}
