using Operation;

public abstract class Request
{
    public OperationCode OpCode { get; private set; }

    public Request(OperationCode opCode)
    {
        OpCode = opCode;
    }

    public abstract void DoRequest(byte[] data);

    public abstract void DoRequest<T>(T obj);

    public abstract void OnOperationResponse(ReturnCode returnCode);
}