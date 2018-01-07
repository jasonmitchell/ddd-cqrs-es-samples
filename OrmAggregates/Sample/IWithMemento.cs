namespace Sample
{
    public interface IWithMemento<out TMemento>
    {
        TMemento CreateMemento();
    }
}