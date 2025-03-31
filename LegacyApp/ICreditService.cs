using System;

namespace LegacyApp
{
    public interface ICreditService
    {
        int GetCreditLimit(string lastName, DateTime dateOfBirth);
    }
}