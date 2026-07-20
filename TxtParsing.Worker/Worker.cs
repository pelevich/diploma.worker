using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace TxtParsing.Worker
{
    public class Worker : IHostedService
    {
        private readonly IPipeClient _pipeClient;
        private readonly TxtProccess _txtProcess;
        private readonly ILogger<Worker> _logger;
        private readonly string _pipe_name;

        public Worker(IPipeClient pipeClient, TxtProccess txtProcess, ILogger<Worker> logger, string pipe_name)
        {
            _pipeClient = pipeClient;
            _txtProcess = txtProcess;
            _logger = logger;
            _pipe_name = pipe_name;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker запустился");
            _pipeClient.ConnectedServer(_pipe_name);
            var path = _pipeClient.Read();

            var result = _txtProcess.CountLineAndChar(path);
            string message = result.ToString();

            _pipeClient.Write(message);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker остановлен");
        }
    }
}
