using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Theater.Abstractions.Piece;
using Theater.Abstractions.Piece.Models;
using Theater.Contracts.Theater;

namespace Theater.Sql.Repositories
{
    public class PieceRepository : IPieceRepository
    {
        private readonly TheaterDbContext _theaterDbContext;

        public PieceRepository(TheaterDbContext theaterDbContext)
        {
            _theaterDbContext = theaterDbContext;
        }

        public async Task<IReadOnlyCollection<PieceShortInformationDto>> GetPieceShortInformation()
        {
            return await _theaterDbContext.Pieces.AsNoTracking()
                .Include(piece => piece.PieceDates)
                .Include(piece => piece.Genre)
                .Include(piece => piece.PieceWorkers)
                    .ThenInclude(x=>x.TheaterWorker)
                .Where(x => x.PieceDates.Any(c => c.Date >= DateTime.UtcNow))
                .Select(x=>new PieceShortInformationDto
                {
                    Id = x.Id, 
                    PieceGenre = x.Genre.GenreName,
                    PieceName = x.PieceName,
                    WorkerShortInformation = x.PieceWorkers.Select(c=>new TheaterWorkerShortInformationDto
                    {
                        FullName = c.TheaterWorker.LastName + " " + c.TheaterWorker.FirstName + " " + c.TheaterWorker.LastName,
                        Id = c.TheaterWorkerId,
                        PositionName = c.TheaterWorker.Position.PositionName,
                        PositionTypeName = c.TheaterWorker.Position.PositionType

                    }).ToList()
                })
                .ToListAsync();

        }
    }
}
