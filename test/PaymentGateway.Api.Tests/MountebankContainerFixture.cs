using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using JetBrains.Annotations;

using PaymentGateway.Api.Tests;

using Testcontainers.Xunit;

using Xunit.Sdk;

[assembly: AssemblyFixture(typeof(MountebankContainerFixture))]

namespace PaymentGateway.Api.Tests;

[UsedImplicitly]
public sealed class MountebankContainerFixture(IMessageSink messageSink) :
    ContainerFixture<ContainerBuilder, IContainer>(messageSink)
{

    protected override ContainerBuilder Configure(ContainerBuilder builder)
    {
        return builder.WithImage("bbyars/mountebank:2.8.1")
            .WithPortBinding(2525, 2525)
            .WithPortBinding(8080, 8080)
            .WithEntrypoint("mb")
            .WithCommand("--configfile", "/imposters/bank_simulator.ejs", "--allowInjection")
            .WithBindMount(Path.GetFullPath("./imposters"), "/imposters")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
            .WithCleanUp(true);
    }

}