using System;
using LegacyApp;
using NUnit.Framework;

namespace LegacyApp.Tests
{
    public class UserServiceTests
    {
        private class FakeClientRepository : IClientRepository
        {
            public Client GetById(int clientId) => new Client { ClientId = clientId, Type = "NormalClient", Name = "TestClient" };
        }

        private class FakeCreditService : ICreditService
        {
            public int CreditValue { get; set; } = 600;

            public int GetCreditLimit(string lastName, DateTime dateOfBirth)
            {
                return CreditValue;
            }
        }

        private class FakeUserSaver : IUserSaver
        {
            public bool Saved { get; private set; } = false;

            public void SaveUser(User user)
            {
                Saved = true;
            }
        }

        [Test]
        public void AddUser_ReturnsFalse_WhenUserIsTooYoung()
        {
            var service = new UserService(new FakeClientRepository(), new FakeCreditService(), new FakeUserSaver());

            var result = service.AddUser("Zosia", "Nowak", "zosia@example.com", DateTime.Today.AddYears(-20), 1);

            Assert.IsFalse(result);
        }

        [Test]
        public void AddUser_ReturnsFalse_WhenNameIsEmpty()
        {
            var service = new UserService(new FakeClientRepository(), new FakeCreditService(), new FakeUserSaver());

            var result = service.AddUser("", "Nowak", "zosia@example.com", DateTime.Today.AddYears(-25), 1);

            Assert.IsFalse(result);
        }

        [Test]
        public void AddUser_ReturnsFalse_WhenEmailIsInvalid()
        {
            var service = new UserService(new FakeClientRepository(), new FakeCreditService(), new FakeUserSaver());

            var result = service.AddUser("Zosia", "Nowak", "invalidemail", DateTime.Today.AddYears(-25), 1);

            Assert.IsFalse(result);
        }

        [Test]
        public void AddUser_ReturnsFalse_WhenCreditLimitTooLow()
        {
            var creditService = new FakeCreditService { CreditValue = 400 };
            var service = new UserService(new FakeClientRepository(), creditService, new FakeUserSaver());

            var result = service.AddUser("Zosia", "Nowak", "zosia@example.com", DateTime.Today.AddYears(-25), 1);

            Assert.IsFalse(result);
        }

        [Test]
        public void AddUser_ReturnsTrue_WhenAllIsValid()
        {
            var saver = new FakeUserSaver();
            var service = new UserService(new FakeClientRepository(), new FakeCreditService(), saver);

            var result = service.AddUser("Zosia", "Nowak", "zosia@example.com", DateTime.Today.AddYears(-25), 1);

            Assert.IsTrue(result);
            Assert.IsTrue(saver.Saved);
        }
    }
}
