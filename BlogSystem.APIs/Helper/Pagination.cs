namespace BlogSystem.APIs.Helper
{
    public class Pagination <T>
    {
        public Pagination(int pageSize, int index, int count, IEnumerable<T> data)
        {
            PageSize = pageSize;
            this.index = index;
            this.count = count;
            Data = data;
        }

        public int PageSize { get; set; }
        public int index { get; set; }
        public int count { get; set; }
        public IEnumerable<T> Data{ get; set; }
    }
}
