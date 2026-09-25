namespace AgenciaViajes.Application.Interfaces.Services
{
    public interface IPasswordHasherService
    {
        string Hash(string password);

        bool Validate(string password, string hashedPassword);
    }
}
