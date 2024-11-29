using System.ComponentModel;

namespace LemonPlatform.Core.Enums
{
    public enum PluginType
    {
        [Description("Data generation")]
        DataGeneration,

        [Description("Convert tools")]
        ConverterTools,

        [Description("Text convert & handler")]
        TextTools,

        [Description("Json handler tools")]
        JsonTools,

        [Description("Image handler tools")]
        ImageTools,

        [Description("Data structure learning")]
        DataStructures,

        [Description("Else tools")]
        Else
    }
}