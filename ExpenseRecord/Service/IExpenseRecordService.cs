using ExpenseRecord.Models;

namespace ExpenseRecord.Services
{
    public interface IExpenseRecordService
    {
        Task CreateAsync(ExpenseRecordDto newExpenseRecord);
        Task<List<ExpenseRecordDto>> GetAsync();
        Task<ExpenseRecordDto?> GetAsync(string id);
        Task<bool> RemoveAsync(string id);
        Task ReplaceAsync(string id, ExpenseRecordDto updatedExpenseRecord);
    }
}