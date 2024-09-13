using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Model;
using Data;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePorfolio(AppUser user, string name)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == user.Id &&x.Film.Name.ToLower()== name.ToLower());
            if(portfolioModel == null){
                return null;
            }
            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Films>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(u => u.AppUserId == user.Id)
            .Select(films => new Films
            {
                Id = films.FilmId,
                Name = films.Film.Name,
                IMDbRating = films.Film.IMDbRating,
                Description = films.Film.Description,
                Genre = films.Film.Genre,
                Director = films.Film.Director,
                LeadActors = films.Film.LeadActors,
                ReleaseYear = films.Film.ReleaseYear,
                Duration = films.Film.Duration,
                Platform = films.Film.Platform,
                CoverImageUrl = films.Film.CoverImageUrl,
                TrailerUrl=films.Film.TrailerUrl,

                
            }).ToListAsync();
        }
    }
}