using Demo.Domain.Models;

namespace Demo.Domain.Interfaces
{
    public interface IMessageBroker
    {
        public Task SendUserCreatedAsync(User user);
    }
}