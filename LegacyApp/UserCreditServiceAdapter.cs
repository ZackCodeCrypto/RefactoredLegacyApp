using System;

namespace LegacyApp
{
    public class UserCreditServiceAdapter : ICreditService
    {
        public int GetCreditLimit(string lastName, DateTime dateOfBirth)
        {
            using var service = new UserCreditService();
            return service.GetCreditLimit(lastName, dateOfBirth);
        }
    }
}