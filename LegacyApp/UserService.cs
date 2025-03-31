using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ICreditService _creditService;
        private readonly IUserSaver _userSaver;
        
        public UserService() : this(new ClientRepositoryAdapter(), new UserCreditServiceAdapter(), new UserDataAccessAdapter())
        {
        }


        public UserService(IClientRepository clientRepository, ICreditService creditService, IUserSaver userSaver)
        {
            _clientRepository = clientRepository;
            _creditService = creditService;
            _userSaver = userSaver;
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!IsValidName(firstName, lastName) || !IsValidEmail(email) || !IsOfValidAge(dateOfBirth))
            {
                return false;
            }

            Client client;
            try
            {
                client = _clientRepository.GetById(clientId);
            }
            catch
            {
                return false;
            }

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                DateOfBirth = dateOfBirth,
                Client = client
            };

            ApplyCreditPolicy(client, user);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            _userSaver.SaveUser(user);
            return true;
        }

        private bool IsValidName(string firstName, string lastName)
        {
            return !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName);
        }

        private bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private bool IsOfValidAge(DateTime dob)
        {
            var today = DateTime.Now;
            int age = today.Year - dob.Year;
            if (today.Month < dob.Month || (today.Month == dob.Month && today.Day < dob.Day)) age--;
            return age >= 21;
        }

        private void ApplyCreditPolicy(Client client, User user)
        {
            switch (client.Type)
            {
                case "VeryImportantClient":
                    user.HasCreditLimit = false;
                    break;

                case "ImportantClient":
                    int doubledLimit = _creditService.GetCreditLimit(user.LastName, user.DateOfBirth) * 2;
                    user.HasCreditLimit = true;
                    user.CreditLimit = doubledLimit;
                    break;

                default:
                    int normalLimit = _creditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.HasCreditLimit = true;
                    user.CreditLimit = normalLimit;
                    break;
            }
        }
    }
}
