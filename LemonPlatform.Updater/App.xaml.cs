using LemonPlatform.Updater.Models;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace LemonPlatform.Updater
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (e.Args.Length > 0)
            {
                var zipFilePath = e.Args[0];
                //var zipFilePath = "C:\\Data\\Tmp\\LemonPlatform\\LemonPlatform.Wpf\\bin\\Debug\\net9.0-windows\\update.json";
                if (!File.Exists(zipFilePath)) throw new Exception($"can not find file: {zipFilePath}");
                var content = File.ReadAllText(zipFilePath);
                var models = JsonSerializer.Deserialize<List<UpdateModel>>(content);
                var main = new MainWindow(models);

                main.ShowDialog();
            }
        }
    }
}