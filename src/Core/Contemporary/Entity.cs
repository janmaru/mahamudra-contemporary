namespace Mahamudra.Patterns
{
    public abstract class BaseEntity<T>
    {
        public virtual T Id { get; protected set; }

        public abstract override string ToString();
    }
}
