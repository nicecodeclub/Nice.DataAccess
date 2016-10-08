namespace Grit.Net.Common.Models.Page
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo
    {
        private int startIndex;
        private int pageSize;
        private string orderColName;
        private string orderStr;
        /// <summary>
        /// 查询起始索引
        /// </summary>
        public int StartIndex
        {
            get
            {
                return startIndex;
            }
            set
            {
                startIndex = value;
            }
        }
        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize
        {
            get
            {
                return pageSize;
            }

            set
            {
                pageSize = value;
            }
        }

        private int totalCount;
        /// <summary>
        /// 排序列名
        /// </summary>
        public string OrderColName
        {
            get
            {
                return orderColName;
            }

            set
            {
                orderColName = value;
            }
        }
        /// <summary>
        ///排序方式 ASC或DESC
        /// </summary>
        public string OrderStr
        {
            get
            {
                return orderStr;
            }

            set
            {
                orderStr = value;
            }
        }
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount
        {
            get
            {
                return totalCount;
            }

            set
            {
                totalCount = value;
            }
        }

        public PageInfo(int startIndex, int pageSize)
        {
            this.startIndex = startIndex;
            this.pageSize = pageSize < 1 ? 10 : pageSize;
        }

        private int pageIndex;
        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageIndex
        {
            get
            {
                return pageIndex;
            }

            set
            {
                pageIndex = value;
                this.startIndex = this.pageIndex * this.pageSize;
            }
        }
    }
}
