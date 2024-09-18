using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace api.Interfaces
{
    public interface ICommentLikePortfolioRepository
    {
        Task<List<Comment>> GetUserCommentLikePortfolio(AppUser user);
        Task<CommentLikePortfolio> LikeCommendAsync(CommentLikePortfolio commentLikePortfolio);
        Task<CommentLikePortfolio> DislikeCommendAsync(AppUser user, int IdOfCommand);

        Task<Comment> UpdateLikesAsync(int commentId);
        Task<Comment> UpdateDislikesAsync(int commentId);



    }
}