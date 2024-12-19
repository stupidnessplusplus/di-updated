using Autofac;
using CommandLine;
using RectanglesCloudPositioning;
using System.Drawing;
using TagsCloud.Config;
using TagsCloudCreation;
using TagsCloudCreation.TagsDrawers;
using TagsCloudCreation.TagsDrawingDecorators;
using TagsCloudCreation.WordSizesGetters;
using WordsFiltration;

namespace TagsCloud;

public static class Program
{
    private static readonly ContainerBuilder _builder = new();

    public static void Main(params string[] args)
    {
        var a = Parser.Default
            .ParseArguments<ProgramConfig>(args)
            .WithParsed(Run);
    }

    private static void Run(ProgramConfig config)
    {
        _builder.RegisterInstance(config).AsImplementedInterfaces().SingleInstance();
        _builder.RegisterType<TagsDrawer>().As<ITagsDrawer>().SingleInstance();
        _builder.RegisterType<TagsCloudCreator>().AsSelf().SingleInstance();
        _builder.RegisterType<TextSplitter>().AsSelf().SingleInstance();
        RegisterDrawingAlgorithms(config);

        var container = _builder.Build();
        var tagsCloudCreator = container.Resolve<TagsCloudCreator>();
        var textSplitter = container.Resolve<TextSplitter>();

        var text = File.ReadAllText(config.InputPath);
        var words = textSplitter.SplitToWords(text);
        var image = tagsCloudCreator.DrawTagsCloud(words);
        image.Save(config.OutputPath, config.ImageFormat);

        Console.WriteLine($"Image saved to '{config.OutputPath}'.");
    }

    private static void RegisterDrawingAlgorithms(ProgramConfig config)
    {
        _builder.RegisterType(config.RectanglesLayouterType).As<ICircularCloudLayouter>().SingleInstance();
        _builder.RegisterType(config.WordSizesGetterType).As<IWordSizesGetter>().SingleInstance();

        _builder.RegisterType<SingleSolidColorTagsDecorator>().As<ITagsDrawingDecorator>().SingleInstance();
        _builder.RegisterType<SingleFontTagsDecorator>().As<ITagsDrawingDecorator>().SingleInstance();

        foreach (var type in config.AdditionalSettingsSetterTypes)
        {
            _builder.RegisterType(type).As<ITagsDrawingDecorator>().SingleInstance();
        }
    }
}
