using System;
using System.Collections.Generic;
using System.Linq;
using AutoMoq.Helpers;
using AutoMoq.TestFixture.Samples.Code;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AutoMoq.TestFixture.Samples.Tests
{
    public class AccountControllerTests : AutoMoqTestFixture<AccountController>
    {
        public AccountControllerTests()
        {
            ResetSubject();
        }

        [Fact]
        public void ShouldListAllAccountsFromRepository()
        {
            Mocked<IAccountRepository>().Setup(
                x => x.Find()).Returns(
                    new[] {new Account(), new Account()});

            ViewResult result = Subject.ListAllAccounts() as ViewResult;

            var model = result.ViewData.Model as IEnumerable<Account>;

            Assert.Equal(2, model.Count());

            Mocked<IAccountRepository>()
                .Verify(x => x.SomethingElse(), Times.Once());
        }

        [Fact]
        public void ShouldShowTheErrorPageWhenRepositoryHasErrors()
        {
            Mocked<IAccountRepository>().Setup(
                    x => x.Find()).Throws(new Exception());

            ViewResult result = Subject.ListAllAccounts() as ViewResult;

            Assert.Equal("Error", result.ViewName);
        }
    }
}
