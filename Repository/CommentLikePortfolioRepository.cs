using api.Interfaces;
using api.Model;
using Data;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentLikePortfolioRepository : ICommentLikePortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentLikePortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<CommentLikePortfolio> DislikeCommendAsync(AppUser user, int IdOfCommand)
        {
            var commentLikePortfolioModel = await _context.CommentLikePortfolios.FirstOrDefaultAsync(x => x.AppUserId == user.Id &&x.Comment.Id== IdOfCommand);
            if(commentLikePortfolioModel == null){
                return null;
            }
            _context.CommentLikePortfolios.Remove(commentLikePortfolioModel);
            await _context.SaveChangesAsync();
            return commentLikePortfolioModel;
        }

        public async Task<List<Comment>> GetUserCommentLikePortfolio(AppUser user)
        {
            return await _context.CommentLikePortfolios.Where(u => u.AppUserId == user.Id).
            Select(comment => new Comment
            {
                Id = comment.CommentId,
                StarRating = comment.Comment.StarRating,
                NumberOfLikes = comment.Comment.NumberOfLikes,
                Content = comment.Comment.Content,
                CreatedOn = comment.Comment.CreatedOn,
                FilmId = comment.Comment.FilmId,
                Film = comment.Comment.Film,

            }).ToListAsync();
        }

        public async Task<CommentLikePortfolio> LikeCommendAsync(CommentLikePortfolio commentLikePortfolio)
        {
            await _context.CommentLikePortfolios.AddAsync(commentLikePortfolio);
            await _context.SaveChangesAsync();
            return commentLikePortfolio;
        }

            public async Task<Comment> UpdateLikesAsync(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment != null)
        {
            comment.NumberOfLikes++;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
        return comment;
    }

       public async Task<Comment> UpdateDislikesAsync(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment != null)
        {
            comment.NumberOfLikes--;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
        return comment;
    }
    }
}