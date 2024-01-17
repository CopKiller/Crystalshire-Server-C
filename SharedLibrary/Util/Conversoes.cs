namespace SharedLibrary.Util;

public static class Conversoes
{
    //public static Conversoes() { }
    public static bool ByteToBoolean(byte value)
    {
        if (value == 0)
            return false;
        if (value == 1)
            return true;
        throw new ArgumentException("O valor deve ser 0 ou 1.", nameof(value));
    }

    public static byte BooleanToByte(bool value)
    {
        if (!value)
            return 0;
        if (value)
            return 1;
        throw new ArgumentException("O valor deve ser true ou false.", nameof(value));
    }
}