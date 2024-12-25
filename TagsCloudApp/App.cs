using Autofac;
using RectanglesCloudPositioning;
using RectanglesCloudPositioning.Configs;
using System.Runtime.InteropServices;
using TagsCloudApp.Configs;
using TagsCloudCreation;
using TagsCloudCreation.Configs;
using TagsCloudCreation.TagsDrawers;
using TagsCloudCreation.TagsDrawingDecorators;
using TagsCloudCreation.WordSizesGetters;
using WordsFiltration;
using WordsFiltration.Configs;
using WordsFiltration.WordsSelectors;

namespace TagsCloudApp;

public class App
{
    public void Run(
        IIOConfig ioConfig,
        IDrawingAlgorithmsConfig algorithmsConfig,
        IWordsSelectionConfig wordsSelectionConfig,
        IWordSizesGetterConfig wordSizesGetterConfig,
        IRectanglesPositioningConfig rectanglesPositioningConfig,
        ITagsColorConfig colorConfig,
        ITagsFontConfig fontConfig)
    {
        var builder = new ContainerBuilder();

        builder.RegisterInstance(wordsSelectionConfig).As<IWordsSelectionConfig>().SingleInstance();
        builder.RegisterInstance(wordSizesGetterConfig).As<IWordSizesGetterConfig>().SingleInstance();
        builder.RegisterInstance(rectanglesPositioningConfig).As<IRectanglesPositioningConfig>().SingleInstance();
        builder.RegisterInstance(colorConfig).As<ITagsColorConfig>().SingleInstance();
        builder.RegisterInstance(fontConfig).As<ITagsFontConfig>().SingleInstance();

        builder.RegisterType<WordsStemmer>().As<IWordsSelector>().SingleInstance();
        builder.RegisterType<PartsOfSpeechFilter>().As<IWordsSelector>().SingleInstance();
        builder.RegisterType<WordsFilter>().As<IWordsSelector>().SingleInstance();
        builder.RegisterType<TextSplitter>().AsSelf().SingleInstance();

        RegisterDrawingAlgorithms(builder, algorithmsConfig);
        builder.RegisterType<TagsDrawer>().As<ITagsDrawer>().SingleInstance();
        builder.RegisterType<TagsCloudCreator>().AsSelf().SingleInstance();

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
        var outputDirectoryName = Path.GetDirectoryName(ioConfig.OutputPath);

        if (!string.IsNullOrEmpty(outputDirectoryName)
            && !Directory.Exists(outputDirectoryName))
        {
            Directory.CreateDirectory(outputDirectoryName!);
        }

        var text = File.ReadAllText(ioConfig.InputPath);
        var words = textSplitter.SplitToWords(text);
        var image = tagsCloudCreator.DrawTagsCloud(words);

        try
        {
            image.Save(ioConfig.OutputPath, ioConfig.ImageFormat);
        }
        catch (ExternalException ex)
        {
            throw new Exception($"Unable to save image to '{ioConfig.OutputPath}'.", ex);
        }
    }

    private void RegisterDrawingAlgorithms(
        ContainerBuilder builder,
        IDrawingAlgorithmsConfig config)
    {
        builder.RegisterType(config.RectanglesLayouterType).As<ICloudLayouter>().SingleInstance();
        builder.RegisterType(config.WordSizesGetterType).As<IWordSizesGetter>().SingleInstance();

        builder.RegisterType<SingleSolidColorTagsDecorator>().As<ITagsDrawingDecorator>().SingleInstance();
        builder.RegisterType<SingleFontTagsDecorator>().As<ITagsDrawingDecorator>().SingleInstance();

        foreach (var type in config.AdditionalSettingsSetterTypes)
        {
            builder.RegisterType(type).As<ITagsDrawingDecorator>().SingleInstance();
        }
    }
}
