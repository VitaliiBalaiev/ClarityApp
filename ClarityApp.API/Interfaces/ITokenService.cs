using ClarityApp.API.Models;

namespace ClarityApp.API.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}