namespace RecipeManagement.Extensions.Services.ProducerRegistrations;

using MassTransit;
using MassTransit.RabbitMqTransport;
using SharedKernel.Messages;
using RabbitMQ.Client;

public static class AddRecipeProducerEndpointRegistration
{
    public static void AddRecipeProducerEndpoint(this IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Message<RecipeAdded>(e => e.SetEntityName("recipe-added")); // name of the primary exchange
        cfg.Publish<RecipeAdded>(e => e.ExchangeType = ExchangeType.Fanout); // primary exchange type
    }
}