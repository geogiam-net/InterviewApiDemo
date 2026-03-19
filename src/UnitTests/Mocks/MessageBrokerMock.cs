using Demo.Domain.Interfaces;
using Demo.Domain.Models;

namespace Demo.UnitTests.Mocks;

public class MessageBrokerMock : IMessageBroker
{
    public MessageBrokerMock()
    {
    }

    public async Task SendUserCreatedAsync(User user)
    {		
    }
}