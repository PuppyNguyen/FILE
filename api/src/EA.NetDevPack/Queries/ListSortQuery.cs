 
using System.Linq;

namespace EA.NetDevPack.Queries
{
    public class ListSortQuery
    {
        protected ListSortQuery(SortingInfo[] sorts)
        {
            if (sorts != null)
            {
                Sorts = new Sorts(sorts.Select(sort => new Sort
                {
                    SortBy = sort.Selector,
                    SortDirection = sort.Desc ? "DESC" : "ASC"
                }));
            }
            else
            {
                Sorts = null;
            }
        }

        public Sorts Sorts { get; }
    }
}
