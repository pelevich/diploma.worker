using System.IO.Pipes;
using System.Text;
using Microsoft.Extensions.Logging;

namespace TxtParsing.Worker
{
    public class PipeForWindow : IPipeClient
    {
        private NamedPipeClientStream _client;
        public bool IsConnected { get; set; }
        private readonly ILogger<PipeForWindow> _logger;

        public PipeForWindow(ILogger<PipeForWindow> logger)
        {
            _logger = logger;
        }


        public void ConnectedServer(string server_name)
        {
            try
            {
                _client = new NamedPipeClientStream(".", server_name, PipeDirection.InOut);
                _client.Connect();

                IsConnected = true;
                _logger.LogInformation("Успешное подключение к пайпу '{server_name}'", server_name);
            }
            catch (Exception ex)
            {
                IsConnected = false;
                _logger.LogError(ex, "Ошибка подключения к пайпу '{server_name}'", server_name);
                throw new IOException($"Ошибка подключения к пайпу: {ex.Message}", ex);
            }
        }

        public void Write(string message)
        {
            if (!IsConnected)
            {
                _logger.LogError("Клиент не подключен к серверу");
                throw new InvalidOperationException("Клиент не подключен к серверу");
            }

            if (string.IsNullOrEmpty(message))
            {
                _logger.LogError("Сообщение не может быть пустым");
                throw new ArgumentException("Сообщение не может быть пустым", nameof(message));
            }

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                _client.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка отправки сообщения '{ex.Message}'", ex.Message);
                throw new IOException($"Ошибка отправки сообщения: {ex.Message}", ex);
            }
        }

        public string Read(int bufferSize = 4096)
        {
            if (!IsConnected){
                _logger.LogError("Клиент не подключен к серверу");
                throw new InvalidOperationException("Клиент не подключен к серверу");
            }

            try
            {
                byte[] buffer = new byte[bufferSize];
                int bytesRead = _client.Read(buffer, 0, buffer.Length);

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка чтения сообщения: '{ex.Message}'", ex.Message);
                throw new IOException($"Ошибка чтения сообщения: {ex.Message}", ex);
            }
        }
    }
}
