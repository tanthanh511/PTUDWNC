using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Contracts
{
    //chứa kết quả phân trang 
    public interface IPagedList
    {
        // tổng số trang
        int PageCount { get; }

        // tổng sô phần tử trả về
        int TotalItemCount { get; }

        // chỉ số trang hiện tại <0>
        int PageIndex { get; }

        // vị trí trang hiện tại <1>
        int PageNumber { get; }

        // số lượng phần tử tối đa trên 1 trang
        int PageSize { get; }

        // kiểm tra trang trước có hay 0
        bool HasPreviousPage { get; }

        // kiểm tra trang tiếp theo hay không 
        bool HasNextPage { get; }

        // trang hiện tại có phải trang đầu tiên ??
        bool IsFirstPage { get; }

        // trang hiện tại có phải trang cuối không  
        bool IsLastPage { get; }

        // thứ tự phần tử đầu trang
        int FirstItemIndex { get; }

        //thứ tự phần tử cuối trang 
        int LastItemIndex { get; }
    }

    public interface IPagedList<out T> : IPagedList, IEnumerable<T>
    {
        // lay pt tai
        T this [int index] { get; }

        // dem luong phan tu trong trang 
        int Count { get; }
    }    
}
