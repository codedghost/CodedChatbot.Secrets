using System.Threading.Tasks;

namespace CoreCodedChatbot.Secrets
{
    public interface ISecretService
    {
        Task Initialize();

        T GetSecret<T>(string secretKey);
    }
}