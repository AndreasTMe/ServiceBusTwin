# ServiceBusTwin

While waiting on Testcontainers to release a package for the Service Bus ([which they did](https://github.com/testcontainers/testcontainers-dotnet/tree/develop/src/Testcontainers.ServiceBus)),
I played around with creating my custom implementation.

The implementation by Testcontainers is great, but I've also added a way to create the config.json file that's required using C# code.