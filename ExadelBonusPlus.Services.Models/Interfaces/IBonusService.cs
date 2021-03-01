using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExadelBonusPlus.Services.Models
{
    public interface IBonusService
    {
        Task<BonusDto> AddBonusAsync(AddBonusDto model, Guid userId, CancellationToken cancellationToken = default);
        Task<List<BonusDto>> FindAllBonusesAsync(CancellationToken cancellationToken = default);
        Task<List<BonusDto>> FindBonusesAsync(BonusFilter bonusFilter, CancellationToken cancellationToken = default);
        Task<BonusDto> FindBonusByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<BonusDto> UpdateBonusAsync(Guid id, UpdateBonusDto model, CancellationToken cancellationToken = default);
        Task<BonusDto> DeleteBonusAsync(Guid id, CancellationToken cancellationToken = default);
        Task<BonusDto> ActivateBonusAsync(Guid id, CancellationToken cancellationToken = default);
        Task<BonusDto> DeactivateBonusAsync(Guid id, CancellationToken cancellationToken = default);
        Task<BonusDto> UpdateBonusRatingAsync(Guid id, double rating, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetBonusTagsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetCitiesAsync(CancellationToken cancellationToken = default);
        Task<List<BonusStatisticDto>> GetBonusStatisticAsync(BonusFilter bonusFilter, CancellationToken cancellationToken = default);
    }
}
