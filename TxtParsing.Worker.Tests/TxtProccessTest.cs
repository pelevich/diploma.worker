using Microsoft.Extensions.Logging;
using Moq;

namespace TxtParsing.Worker.Tests
{
    public class TxtProccessTest
    {
        [Fact]
        public void CountLineAndChar_CorrectPath_ReturnTrueAnswer()
        {
            var mockMetrics = new Mock<IServiceMetrics>();
            var mockLogger = new Mock<ILogger<TxtProccess>>();
            var obj = new TxtProccess(mockMetrics.Object, mockLogger.Object);

            string path = "C:\\Users\\USER\\source\\repos\\TxtParsing.Worker\\TxtParsing.Worker.Tests\\test.txt";

            var result = obj.CountLineAndChar(path);

            Assert.Equal((1,1), result);
        }

        [Fact]
        public void CountLineAndChar_EmptyArg_ArgumentException()
        {
            var mockMetrics = new Mock<IServiceMetrics>();
            var mockLogger = new Mock<ILogger<TxtProccess>>();
            string filePath = "";
            var expectedException = new ArgumentException("аргумент не может быть пустым", nameof(filePath));

            var obj = new TxtProccess(mockMetrics.Object, mockLogger.Object);

            var exception = Assert.Throws<ArgumentException>(()=>obj.CountLineAndChar(filePath));

            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Fact]
        public void CountLineAndChar_NoCorrectPath_FileNotFoundException()
        {
            var mockMetrics = new Mock<IServiceMetrics>();
            var mockLogger = new Mock<ILogger<TxtProccess>>();
            string filePath = "C:\\NoCorect.txt";
            var expectedException = new FileNotFoundException($"Файл не найден по пути: {filePath}", filePath);

            var obj = new TxtProccess(mockMetrics.Object, mockLogger.Object);

            var exception = Assert.Throws<FileNotFoundException>(() => obj.CountLineAndChar(filePath));

            Assert.Equal(expectedException.Message, exception.Message);
        }
    }
}
