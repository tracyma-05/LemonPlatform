using LemonPlatform.Core.Exceptions;
using LemonPlatform.Core.Infrastructures.Denpendency;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Threading;

namespace LemonPlatform.Wpf.Exceptions
{
    public class LemonExceptionHandler : ISingletonDependency
    {
        private readonly ILogger _logger;
        public LemonExceptionHandler(ILogger<LemonExceptionHandler> logger)
        {
            _logger = logger;
        }

        public void ApplicationExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ExceptionHandler(e.Exception);
            e.Handled = true;
        }

        public void DomainExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                ExceptionHandler((Exception)e.ExceptionObject);
            }
            catch
            { }

        }

        public void UnobservedTaskExceptionHandler(object? sender, UnobservedTaskExceptionEventArgs args)
        {
            try
            {
                ExceptionHandler(args.Exception);
            }
            catch (Exception)
            { }
        }

        private void ExceptionHandler(Exception exception)
        {
            if (exception is LemonException)
            {
                _logger.LogWarning(exception.Message);
                MessageBox.Show(exception.Message, "Next Platform Error.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (exception.InnerException != null)
                {
                    ExceptionHandler(exception.InnerException);
                }
                else
                {
                    _logger.LogError(exception.StackTrace);
                    MessageBox.Show(exception.Message, "Unknown Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}