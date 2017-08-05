namespace Astral.Data
{
    public interface IDeliveryRepositoryProvider
    {
        IDeliveryRepository GetDeliveryRepository();
    }
}