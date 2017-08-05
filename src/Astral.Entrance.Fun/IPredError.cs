namespace Astral.Fun
{
    public interface IPredError<T>
    {
        string GetError(T value);
    }
}