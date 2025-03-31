namespace LegacyApp
{
    public class UserDataAccessAdapter : IUserSaver
    {
        public void SaveUser(User user)
        {
            UserDataAccess.AddUser(user);
        }
    }
}