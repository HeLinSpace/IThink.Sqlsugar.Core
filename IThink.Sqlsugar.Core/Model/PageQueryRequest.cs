/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 分页查询模型
    /// </summary>
    public class PageQueryRequest<T>
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        private int _pageSize = 20;
        public int? PageSize
        {
            get { return this._pageSize; }
            set
            {
                if (value != null)
                {
                    this._pageSize = value.Value;
                }
            }
        }

        private int _pageNumber = 1;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int? PageNumber
        {
            get { return this._pageNumber; }
            set
            {
                if (value != null && value > 0)
                {
                    this._pageNumber = value.Value;
                }
            }
        }

        /// <summary>
        /// 请求数据
        /// </summary>
        public T RequestData { get; set; }

    }

    /// <summary>
    /// 分页查询模型
    /// </summary>
    public class PageQueryRequest: PageQueryRequest<string>
    {
    }
}
