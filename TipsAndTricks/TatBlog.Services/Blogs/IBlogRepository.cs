using TatBlog.Core.Constants;
using TatBlog.Core.Entities;
using TatBlog.Core.DTO;
using TatBlog.Core.Contracts;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
//< cái này chỉ mới tạo hàm để khởi tạo và chạo đa luồng >
{   // CancellationToken cancellationToken = default ( mã thông báo hủy) >_<

    // tìm bài viết có định danh là slug 
    // được đăn vào tháng và năm 
    #region
    Task<Post> GetPostAsync(
        int year,
        int month,
        string slug,
        CancellationToken cancellationToken = default);
    #endregion

    // tim top N bai viet pho bien duoc nhiu nguoi xem nha
    // task chức năng giống thread cũng là xử lí đa luồng nhưng task hỗ trợ thư viện sẵn nên dùng task lun ^_^
    #region
    Task<IList<Post>> GetPopularArticlesAsync(
        int numPosts,
        CancellationToken cancellationToken = default);
    #endregion

    // kiểm tra xem tên của bài viết đã có hay chưa 
    #region
    Task<bool> IsPostSlugExistedAsync(
        int postID, string slug,
        CancellationToken cancellationToken = default);
    #endregion

    // tăng số lượng xem của một bài viết 
    #region
    Task IncreaseViewCountAsync(
        int postID,
        CancellationToken cancellationToken = default);
    #endregion

    #region"lấy danh sách chủ đề "
    Task<IList<CategoryItem>> GetCategoriesAsync(
        bool showOnMenu = false,
        CancellationToken cancellationToken = default);
    #endregion

    #region"lấy danh sách từ khóa/ thẻ và phân trang theo các tham số pagingParams"
    Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams,
        CancellationToken cancellationToken = default);
    #endregion

    #region"lấy tag có định danh bằng slug"
    Task<Tag> GetTagAsync(string slug, CancellationToken cancellationToken = default);
    #endregion

    #region"lấy danh sách tất cả tag kèm theo sô bài viết "
    Task<IList<TagItem>> GetAllByTagNumberAsync(CancellationToken cancellation = default);
    #endregion

    #region"xóa một thẻ theo mã cho trước "
    Task<bool> TagDeleteByID(
        int id,
        CancellationToken cancellationToken = default);
    #endregion

    #region"tìm chuyên mục category theo định danh slug"
    Task<Category> GetCategorybySlugAsync(
        string slug,
        CancellationToken cancellationToken = default);
    #endregion

    #region"tìm một chuyên mục theo mã cho trước"
    Task<Category> GetCategoryByID(
        int id,
        CancellationToken cancellationToken = default);
    #endregion

    #region"kiểm tra chuyên mục đã có hay chưa"
    Task<bool> IsCategorySlugExistedAsync(
        int id,
        string slug,
        CancellationToken cancellationToken = default);

    #endregion

    #region"ham them hoac sua category"
    Task AddOrUpdateCategory(
        Category category,
        CancellationToken cancellationToken = default);
    #endregion

    #region"xóa một chuyen muc theo mã cho trước"
    Task<bool> DeleteCategoryByID(
    int id,
    CancellationToken cancellationToken = default);
    #endregion

    #region Lấy và phân trang danh sách chuyên mục, kết quả trả về kiểu IPagedList<CategoryItem> >_<
    Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
        IPagingParams pagingParams,
        CancellationToken cancellationToken = default);

    #endregion

    #region tìm kiếm và phân trang của bài viết(post)
    IQueryable<Post> FilterPost(PostQuery condition);
    Task<IPagedList<Post>> GetPagedPostAsync(
        PostQuery condition,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
    Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery condition,
         IPagingParams pagingParams,
         CancellationToken cancellationToken = default);



    #endregion
}

