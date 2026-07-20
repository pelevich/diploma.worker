namespace TxtParsing.Worker
{
    public interface IServiceMetrics
    {
        void CallServiceCounterLine(int lines);
        void CallServiceCounterChar(int chars);
    }
}
