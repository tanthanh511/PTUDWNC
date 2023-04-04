using TatBlog.Core.Constants;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IPostRepository
{
    //Task<Post> GetPostBySlugAsync(
    //    string slug,
    //    CancellationToken cancellationToken = default);

    //Task<Post> GetCachedPostBySlugAsync(
    //    string slug, CancellationToken cancellationToken = default);

    Task<Post> GetPostByIdAsync(int categoryId);

    Task<Post> GetCachedPostByIdAsync(int categoryId);



    Task<bool> AddOrUpdateAsync(
        Post post,
        CancellationToken cancellationToken = default);

    Task<bool> DeletePostAsync(
        int postId,
        CancellationToken cancellationToken = default);

    Task<bool> IsPostSlugExistedAsync(
        int postId, string slug,
        CancellationToken cancellationToken = default);

   // Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery condition, IPagingParams pagingParams, CancellationToken cancellationToken = default);

     Task<IPagedList<T>> GetPagedPostAsync<T>(
            PostQuery pq,
            IPagingParams pagingParams,
            Func<IQueryable<Post>, IQueryable<T>> mapper,
            CancellationToken cancellationToken = default);

    IQueryable<Post> FilterPost(PostQuery condition);

    Task<bool> SetImageUrlAsync(
        int authorId, string imageUrl,
        CancellationToken cancellationToken = default);

    //Task<IPagedList<T>> GetPagedPostsAsync<T>(
    //    PostQuery pq,
    //    IPagingParams pagingParams, 
    //    Func<IQueryable<Post>, 
    //    IQueryable<T>> mapper,
    //    CancellationToken cancellationToken = default);
}