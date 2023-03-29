using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestWpf.Services
{
    internal class ApplicationHostService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (App.Current.MainWindow == null)
            {
                App.GetService<MainWindow>().Show();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
