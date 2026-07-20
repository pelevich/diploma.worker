using Microsoft.Extensions.Logging;
using Moq;

namespace TxtParsing.Worker.Tests
{
    public class PipeTest
    {
        private readonly Mock<IPipeClient> _mockClient;

        public PipeTest()
        {
            _mockClient = new Mock<IPipeClient>();
        }

        [Fact]
        public void ConnectedServer_CorrectNamePipe_ReturnTrue()
        {
            var mockMetrics = new Mock<IServiceMetrics>();
            var mockLogger= new Mock<ILogger<Pipe>>();

            var mockClient = new Mock<IPipeClient>();
            mockClient.Setup(x => x.ConnectedServer("test"));
            mockClient.Setup(x => x.IsConnected).Returns(true);

            var MyPipe = new Pipe(mockLogger.Object, mockMetrics.Object);
            string name_pipe = "test";

            MyPipe.ConnectedServer(name_pipe);

            Assert.True(MyPipe.IsConnected);
        }

        [Fact]
        public void Write_CorrectNamePipe_ReturnTrue()
        {
            var mockMetrics = new Mock<IServiceMetrics>();
            var mockLogger = new Mock<ILogger<Pipe>>();

            var MyPipe = new Pipe(mockLogger.Object, mockMetrics.Object);
            string message = "test";

            var exception = Record.Exception(() => MyPipe.Write(message));

            Assert.Null(exception);
        }

        [Fact]
        public void Read_CorrectNamePipe_ReturnTrue()
        {
            var mockMetrics = new Mock<IServiceMetrics>();
            var mockLogger = new Mock<ILogger<Pipe>>();

            var MyPipe = new Pipe(mockLogger.Object, mockMetrics.Object);

            var result = MyPipe.Read();

            Assert.IsType<string>(result);
        }
    }
}
