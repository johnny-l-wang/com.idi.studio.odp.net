namespace IDI.Studio.ODP.FastReflection
{
    public interface IFastReflectionFactory<TKey, TValue>
    {
        TValue Create(TKey key);
    }
}
