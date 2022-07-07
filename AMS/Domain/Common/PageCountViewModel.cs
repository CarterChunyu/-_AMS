using System.Collections.Generic;

namespace Domain.Common
{
    public class PageCountViewModel
    {
        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotolCount;
        /// <summary>
        /// 每頁幾筆資料
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 當前頁碼
        /// </summary>
        public int CurrentPage { get; set; } = 1;
        /// <summary>
        /// 頁碼
        /// </summary>
        public List<int> Pages
        {
            get
            {
                List<int> pages = new List<int>();
                if (TotolCount != 0)
                {
                    int totolPage = ((TotolCount + PageSize) - 1) / (PageSize);

                    for (int i = 1; i <= totolPage; i++)
                    {
                        pages.Add(i);
                    }
                }
                return pages;
            }
        }
        public PageCountViewModel PageModel { get { return this; } }
    }
}
