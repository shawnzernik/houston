namespace webapi.Database
{
    public interface CopyFrom<T>
    {
        void CopyFrom(T source);
    }
}
