using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using Ninject;
using Ninject.Extensions.Factory;
using Ninject.Extensions.Conventions;

namespace FractalPainting.App
{
    public static class DIContainerTask
    {
        public static MainForm CreateMainForm()
        {
            var container = ConfigureContainer();

            return container.Get<MainForm>();
        }

        public static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();

            //container.Bind<IUiAction>().To<ImageSettingsAction>();
            //container.Bind<IUiAction>().To<SaveImageAction>();
            //container.Bind<IUiAction>().To<PaletteSettingsAction>();
            //container.Bind<IUiAction>().To<DragonFractalAction>();
            //container.Bind<IUiAction>().To<KochFractalAction>();

            container.Bind(c => c.FromThisAssembly()
            .SelectAllClasses().InheritedFrom<IUiAction>().BindAllInterfaces());

            container.Bind<Palette>().To<Palette>().InSingletonScope();
            container.Bind<IImageHolder, PictureBoxImageHolder>()
                .To<PictureBoxImageHolder>().InSingletonScope();

            container.Bind<IObjectSerializer>().To<XmlObjectSerializer>()
                .WhenInjectedInto<SettingsManager>();
            container.Bind<IBlobStorage>().To<FileBlobStorage>()
                .WhenInjectedInto<SettingsManager>();

            container.Bind<AppSettings>().ToMethod(c => c.Kernel.Get<SettingsManager>().Load())
                .InSingletonScope();
            container.Bind<ImageSettings>().ToMethod(c => c.Kernel.Get<AppSettings>().ImageSettings)
                .InSingletonScope();

            container.Bind<IDragonPainterFactory>().ToFactory();

            return container;
        }
    }

    public class DragonFractalAction : IUiAction
    {
        public MenuCategory Category => MenuCategory.Fractals;
        public string Name => "Дракон";
        public string Description => "Дракон Хартера-Хейтуэя";

        //private readonly IDragonPainterFactory dragonPainterFactory;

        //public DragonFractalAction(IDragonPainterFactory dragonPainterFactory)
        //{
        //    this.dragonPainterFactory = dragonPainterFactory;
        //}

        private readonly Func<DragonSettings, DragonPainter> factory;

        public DragonFractalAction(Func<DragonSettings, DragonPainter> factory)
        {
            this.factory = factory;
        }

        public void Perform()
        {
            var dragonSettings = CreateRandomSettings();
            // редактируем настройки:
            SettingsForm.For(dragonSettings).ShowDialog();
            // создаём painter с такими настройками
            var painter = factory(dragonSettings);
            // var painter = dragonPainterFactory.CreateDragonPainter(dragonSettings);
            painter.Paint();
        }

        private static DragonSettings CreateRandomSettings()
        {
            return new DragonSettingsGenerator(new Random()).Generate();
        }
    }

    public interface IDragonPainterFactory
    { 
        DragonPainter CreateDragonPainter(DragonSettings settings);
    }

    public class KochFractalAction : IUiAction
    {
        public MenuCategory Category => MenuCategory.Fractals;
        public string Name => "Кривая Коха";
        public string Description => "Кривая Коха";

        private readonly Lazy<KochPainter> painter;

        public KochFractalAction(Lazy<KochPainter> painter)
        {
            this.painter = painter;
        }

        //private readonly Func<KochPainter> painter;

        //public KochFractalAction(Func<KochPainter> painter)
        //{
        //    this.painter = painter;
        //}

        public void Perform()
        {
            painter.Value.Paint();
            //painter().Paint();
        }
    }

    public class DragonPainter
    {
        private readonly IImageHolder imageHolder;
        private readonly Palette palette;
        private readonly DragonSettings settings;

        public DragonPainter(IImageHolder imageHolder, DragonSettings settings, Palette palette)
        {
            this.imageHolder = imageHolder;
            this.settings = settings;
            this.palette = palette;
        }

        public void Paint()
        {
            using (var graphics = imageHolder.StartDrawing())
            {
                using (var backgroundBrush = new SolidBrush(palette.BackgroundColor))
                {
                    graphics.FillRectangle(backgroundBrush, 0, 0, imageHolder.GetImageSize().Width, imageHolder.GetImageSize().Height);
                }
                var r = new Random();
                var cosa = (float)Math.Cos(settings.Angle1);
                var sina = (float)Math.Sin(settings.Angle1);
                var cosb = (float)Math.Cos(settings.Angle2);
                var sinb = (float)Math.Sin(settings.Angle2);
                var shiftX = settings.ShiftX * Math.Min(imageHolder.GetImageSize().Width, imageHolder.GetImageSize().Height) / 2.1f * 0.8f;
                var shiftY = settings.ShiftY * Math.Min(imageHolder.GetImageSize().Width, imageHolder.GetImageSize().Height) / 2.1f * 0.8f;
                var scale = settings.Scale;
                var p = new PointF(0, 0);
                using (var primaryColor = new SolidBrush(palette.PrimaryColor))
                {
                    foreach (var i in Enumerable.Range(0, settings.IterationsCount))
                    {
                        graphics.FillRectangle(primaryColor, imageHolder.GetImageSize().Width / 3f + p.X, imageHolder.GetImageSize().Height / 2f + p.Y, 1, 1);
                        if (r.Next(0, 2) == 0)
                            p = new PointF(scale * (p.X * cosa - p.Y * sina), scale * (p.X * sina + p.Y * cosa));
                        else
                            p = new PointF(scale * (p.X * cosb - p.Y * sinb) + shiftX, scale * (p.X * sinb + p.Y * cosb) + shiftY);
                        if (i % 100 == 0) imageHolder.UpdateUi();
                    }
                }
            }

            imageHolder.UpdateUi();
        }
    }
}
