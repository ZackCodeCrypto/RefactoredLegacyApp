namespace LegacyApp
{
    public class ClientRepositoryAdapter : IClientRepository
    {
        private readonly ClientRepository _repository = new ClientRepository();

        public Client GetById(int clientId)
        {
            return _repository.GetById(clientId);
        }
    }
}