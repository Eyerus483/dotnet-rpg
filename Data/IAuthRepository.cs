namespace dotnet_rpg.Data
{
    public interface IAuthRepository
    {

        Task<ServiceRespone<int>> Register(User user, string password);
        Task<ServiceRespone<string>> Login(string userName, string password);
        Task<bool> UserExistes(string userName);
    }
}
