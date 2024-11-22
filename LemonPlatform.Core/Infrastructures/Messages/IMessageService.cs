using LemonPlatform.Core.Infrastructures.Denpendency;

namespace LemonPlatform.Core.Infrastructures.Messages
{
    public interface IMessageService : ITransientDependency
    {
        void ShowSnackMessage(string message);
    }
}