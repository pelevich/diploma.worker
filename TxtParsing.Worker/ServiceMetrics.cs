using System.Diagnostics.Metrics;

namespace TxtParsing.Worker
{
    public class ServiceMetrics : IServiceMetrics
    {
        private readonly Counter<int> _txtFilesTotalLine;
        private readonly Counter<int> _txtFilesTotalChar;

        public ServiceMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create("TxtProccess");

            _txtFilesTotalLine = meter.CreateCounter<int>(
            "txt.files.total.line",
            unit: "{file}",
            description: "количество посчитанных символов");

            _txtFilesTotalChar = meter.CreateCounter<int>(
            "txt.files.total.line",
            unit: "{file}",
            description: "количество посчитанных строк");
        }

        public void CallServiceCounterLine(int quantity)
        {
            _txtFilesTotalLine.Add(quantity);
        }

        public void CallServiceCounterChar(int quantity)
        {
            _txtFilesTotalChar.Add(quantity);
        }
    }
}
