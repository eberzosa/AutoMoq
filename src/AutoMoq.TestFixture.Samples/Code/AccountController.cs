using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AutoMoq.TestFixture.Samples.Code
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepos;

        public AccountController(IAccountRepository accountRepos)
        {
            _accountRepos = accountRepos;
        }

        public ActionResult ListAllAccounts()
        {
            try
            {
                _accountRepos.SomethingElse();

                return View(_accountRepos.Find());    
            }
            catch
            {
                return View("Error");
            }
        }
    }

    public interface IAccountRepository
    {
        IEnumerable<Account> Find();
        void SomethingElse();
    }

    public class Account
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
