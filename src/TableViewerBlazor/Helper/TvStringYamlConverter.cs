using TableViewerBlazor.Public;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace TableViewerBlazor.Helper;

public class TvStringYamlConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return type.GetCustomAttribute<TvStringAttribute>() != null;
    }

    public object ReadYaml(IParser parser, Type type)
    {
        throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        emitter.Emit(new Scalar(value?.ToString() ?? string.Empty));
    }
}