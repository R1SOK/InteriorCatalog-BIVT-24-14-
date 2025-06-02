namespace Model.Core
{
    public interface ISortable
    {
        void Sort(bool ascending = true);
        void SortByName(bool ascending = true);
        void SortByPrice(bool ascending = true);
    }
}
