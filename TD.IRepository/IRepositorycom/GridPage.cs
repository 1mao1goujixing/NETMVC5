using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.IRepository
{
    public sealed class GridPage
    {

        /// <summary>
        /// 是否分页
        /// </summary>
        public bool ispaging { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageIndex { get; set; }

        /// <summary>
        /// 当前页起始记录索引
        /// </summary>
        public int BeginIndex
        {
            get
            {
                return (pageIndex - 1) * pageSize;
            }
        }

        /// <summary>
        /// 当前页结束记录索引
        /// </summary>
        public int EndIndex
        {
            get
            {
                return pageSize * pageIndex + 1;
            }
        }
    }
}
