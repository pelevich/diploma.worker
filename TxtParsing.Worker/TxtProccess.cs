using Microsoft.Extensions.Logging;

namespace TxtParsing.Worker
{
    public class TxtProccess
    {
        private readonly IServiceMetrics _metrics;
        private readonly ILogger<TxtProccess> _logger;

        public TxtProccess(IServiceMetrics metrics, ILogger<TxtProccess> logger)
        {
            _metrics = metrics;
            _logger = logger;
        }

        public (int lines, int chars) CountLineAndChar(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                _logger.LogError("Вызов метода с пустым аргументом");
                throw new ArgumentException("аргумент не может быть пустым", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                _logger.LogError($"Файл не найден по пути: {filePath}");
                throw new FileNotFoundException($"Файл не найден по пути: {filePath}", filePath);
            }

            int lineCount = 0;
            int charCount = 0;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lineCount++;
                    charCount += line.Length;
                }
            }

            _metrics.CallServiceCounterLine(lineCount);
            _metrics.CallServiceCounterChar(charCount);

            return (lineCount, charCount);
        }
    }
}
