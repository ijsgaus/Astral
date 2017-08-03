namespace Astral.Tools.Ex
{
    public interface IPredError<T>
    {
        string GetError(T value);
    }
}