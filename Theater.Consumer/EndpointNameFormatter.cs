using MassTransit;

namespace Theater.Consumer;

internal sealed class EndpointNameFormatter : DefaultEndpointNameFormatter
{
    private const string PrefixQueue = "Theater-";

    public override string SanitizeName(string name)
        => PrefixQueue + name;
}