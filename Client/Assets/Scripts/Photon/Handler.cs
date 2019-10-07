using Operation;

public abstract class Handler
{
    public OperationCode OpCode { get; private set; }

    public Handler(OperationCode opCode)
    {
        OpCode = opCode;
    }

    public abstract void OnOperationResponse(ReturnCode returnCode, byte[] returnData);
    public abstract void OnEvent(byte[] data);
}