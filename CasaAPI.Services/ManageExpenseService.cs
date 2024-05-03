using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Services
{
    public class ManageExpenseService : IManageExpenseService
    {
        private IManageExpenseRepository _manageExpenseRepository;

        public ManageExpenseService(IManageExpenseRepository manageExpenseRepository)
        {
            _manageExpenseRepository = manageExpenseRepository;
        }

        #region Expense
        public async Task<int> SaveExpense(Expense_Request parameters)
        {
            return await _manageExpenseRepository.SaveExpense(parameters);
        }
        public async Task<IEnumerable<Expense_Response>> GetExpenseList(Expense_Search parameters)
        {
            return await _manageExpenseRepository.GetExpenseList(parameters);
        }
        public async Task<Expense_Response?> GetExpenseById(int id)
        {
            return await _manageExpenseRepository.GetExpenseById(id);
        }
        #endregion

        #region Expense Details
        public async Task<int> SaveExpenseDetails(ExpenseDetails_Request parameters)
        {
            return await _manageExpenseRepository.SaveExpenseDetails(parameters);
        }
        public async Task<IEnumerable<ExpenseDetails_Response>> GetExpenseDetailsList(ExpenseDetails_Search parameters)
        {
            return await _manageExpenseRepository.GetExpenseDetailsList(parameters);
        }
        public async Task<ExpenseDetails_Response?> GetExpenseDetailsById(int id)
        {
            return await _manageExpenseRepository.GetExpenseDetailsById(id);
        }
        public async Task<int> ExpenseDetailsApproveNReject(Expense_ApproveNReject parameters)
        {
            return await _manageExpenseRepository.ExpenseDetailsApproveNReject(parameters);
        }
        #endregion

    }
}
