using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;

namespace TxtParsing.Worker
{

    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Ошибка: необходимо указать название пайпа");
                Console.WriteLine("Использование: Worker.exe <pipe_name>");
                return;
            }

            var pipeName = args[0];

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddLogging();
            builder.Services.AddSingleton<IPipeClient, Pipe>();
            builder.Services.AddSingleton<TxtProccess>();
            builder.Services.AddSingleton<IServiceMetrics, ServiceMetrics>();

            builder.Services.AddHostedService<Worker>(provider =>
            {
                var pipeClient = provider.GetRequiredService<IPipeClient>();
                var txtProcess = provider.GetRequiredService<TxtProccess>();
                var logger = provider.GetRequiredService<ILogger<Worker>>();

                return new Worker(pipeClient, txtProcess, logger, pipeName);
            });

            builder.Services.AddOpenTelemetry()
                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddOtlpExporter()
                    .AddMeter("TxtProccess"));

            builder.Logging.AddOpenTelemetry(options =>
            {
                options.AddOtlpExporter();
            });

            using IHost host = builder.Build();
            host.Run();
        }
    }
}