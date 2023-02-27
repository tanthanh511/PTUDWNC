using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
    //< cái này chỉ mới tạo hàm để khởi tạo và chạo đa luồng >
{   // CancellationToken cancellationToken = default ( mã thông báo hủy) >_<

    // tìm bài viết có định danh là slug 
    // được đăn vào tháng và năm 
    Task<Post> GetPostAsync(
        int year,
        int month,
        string slug,
        CancellationToken cancellationToken = default);

    // tim top N bai viet pho bien duoc nhiu nguoi xem nha
    // task chức năng giống thread cũng là xử lí đa luồng nhưng task hỗ trợ thư viện sẵn nên dùng task lun ^_^
    Task<IList<Post>> GetPopularArticlesAsync(
        int numPosts,
        CancellationToken cancellationToken = default);

    // kiểm tra xem tên của bài viết đã có hay chưa 
    Task<bool> IsPostSlugExistedAsync(
        int postID, string slug,
        CancellationToken cancellationToken = default);

    // tăng số lượng xem của một bài viết 
    Task IncreaseViewCountAsync(
        int postID,
        CancellationToken cancellationToken = default);
       
    //

        
}
