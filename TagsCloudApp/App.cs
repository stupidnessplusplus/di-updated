using Autofac;
using RectanglesCloudPositioning;
using TagsCloudApp.Configs;
using TagsCloudCreation;
using TagsCloudCreation.Configs;
using TagsCloudCreation.TagsDrawers;
using TagsCloudCreation.TagsDrawingDecorators;
using TagsCloudCreation.WordSizesGetters;
using WordsFiltration;

namespace TagsCloudApp;

public class App
{
    public void Run(
        IIOConfig ioConfig,
        IDrawingAlgorithmsConfig algorithmsConfig,
        IWordSizesGetterConfig wordSizesGetterConfig,
        ITagsColorConfig colorConfig,
        ITagsFontConfig fontConfig)
    {
        var builder = new ContainerBuilder();

        builder.RegisterInstance(wordSizesGetterConfig).As<IWordSizesGetterConfig>().SingleInstance();
        builder.RegisterInstance(colorConfig).As<ITagsColorConfig>().SingleInstance();
        builder.RegisterInstance(fontConfig).As<ITagsFontConfig>().SingleInstance();

        builder.RegisterType<TagsDrawer>().As<ITagsDrawer>().SingleInstance();
        builder.RegisterType<TagsCloudCreator>().AsSelf().SingleInstance();
        builder.RegisterType<TextSplitter>().AsSelf().SingleInstance();
        RegisterDrawingAlgorithms(builder, algorithmsConfig);

        var container = builder.Build();
        var tagsCloudCreator = container.Resolve<TagsCloudCreator>();
        var textSplitter = container.Resolve<TextSplitter>();

        Run(ioConfig, textSplitter, tagsCloudCreator);
    }

    private void Run(
        IIOConfig ioConfig,
        TextSplitter textSplitter,
        TagsCloudCreator tagsCloudCreator)
    {
        var text = File.ReadAllText(ioConfig.InputPath);
        var words = textSplitter.SplitToWords(text);
        var image = tagsCloudCreator.DrawTagsCloud(words);
        image.Save(ioConfig.OutputPath, ioConfig.ImageFormat);
    }

    private void RegisterDrawingAlgorithms(
        ContainerBuilder builder,
        IDrawingAlgorithmsConfig config)
    {
        builder.RegisterType(config.RectanglesLayouterType).As<ICircularCloudLayouter>().SingleInstance();
        builder.RegisterType(config.WordSizesGetterType).As<IWordSizesGetter>().SingleInstance();

        builder.RegisterType<SingleSolidColorTagsDecorator>().As<ITagsDrawingDecorator>().SingleInstance();
        builder.RegisterType<SingleFontTagsDecorator>().As<ITagsDrawingDecorator>().SingleInstance();

        foreach (var type in config.AdditionalSettingsSetterTypes)
        {
            builder.RegisterType(type).As<ITagsDrawingDecorator>().SingleInstance();
        }
    }
}
