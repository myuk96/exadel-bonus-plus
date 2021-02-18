﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExadelBonusPlus.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SampleDataGenerator;
using Xunit;
using IHistoryRepository = ExadelBonusPlus.Services.Models.IHistoryRepository;

namespace ExadelBonusPlus.Services.Tests
{
    public class HistoryServiceTest
    {
        private Mock<IHistoryRepository> _historyRepo;
        private IHistoryRepository _mockhistoryRepository;
        private HistoryService _historyService;
        private IMapper _mapper;
        private List<History> _fakeHistoryDtos;


        private Mock<IBonusRepository> _bonusRepo;
        private IBonusRepository _mockBonusRepository;


        public HistoryServiceTest()
        {
            
        }
        [Fact]
        public async Task<List<HistoryDto>> History_FindAllHistoryAsync_Return_ListHistoryDTO()
        {
            CreateDefaultHistoryServiceInstance();

            var historyList = await _historyService.GetAllHistory(default(CancellationToken));

            Assert.True(Equals(10, historyList.Count()));
            return (List<HistoryDto>)historyList;
        }
        [Fact]
        public async Task<AddHistoryDTO> History_AddHistoryAsync_Return_AddHistoryDTO()
        {
            CreateDefaultHistoryServiceInstance();
            var history = _mapper.Map<AddHistoryDTO>(_fakeHistoryDtos[0]);

            var bonus = await _historyService.AddHistory(history, default(CancellationToken));

            Assert.NotNull(history);
            return history;
        }

        [Fact]
        public async Task<HistoryDto> History_DeleteHistoryAsync_Return_HistoryDTO()
        {
            CreateDefaultHistoryServiceInstance();
            
            var idHistory = _fakeHistoryDtos[0].Id;
            var historyDto = await _historyService.DeleteHistory(idHistory, default(CancellationToken));


            Assert.NotNull(historyDto);
            return historyDto;
        }

        [Fact]
        public async Task<HistoryDto> History_FindByIdHistoryAsync_Return_HistoryDTO()
        {
            CreateDefaultHistoryServiceInstance();
            var idHistory = _fakeHistoryDtos[0].Id;
            var history = await _historyService.GetHistoryById(idHistory, default(CancellationToken));

            Assert.NotNull(history);
            return history;
        }

        [Fact]
        public async Task<List<BonusHistoryDto>> History_GetBonusAllHistoryAsync_Return_ListBonusHistoryDto()
        {
            CreateDefaultHistoryServiceInstance();
            var idHistory = _fakeHistoryDtos[0].BonusId;
            var history = await _historyService.GetBonusAllHistory(idHistory, default(CancellationToken));

            Assert.NotNull(history);
            return (List<BonusHistoryDto>)history;
        }
        [Fact]
        public async Task<List<UserHistoryDto>> History_GetUserHistoryAsync_Return_ListUserHistoryDto()
        {
            CreateDefaultHistoryServiceInstance();
            var idHistory = _fakeHistoryDtos[0].CreatorId;
            var history = await _historyService.GetUserAllHistory(idHistory, default(CancellationToken));

            Assert.NotNull(history);
            return (List<UserHistoryDto>)history;
        }

        [Fact]
        public async Task<List<BonusHistoryDto>> History_GetBonusHistoryByUsageDateAsync_Return_ListBonusHistoryDto()
        {
            CreateDefaultHistoryServiceInstance();
            var bonusIdy = _fakeHistoryDtos[0].BonusId;
            var history = await _historyService.GetBonusHistoryByUsageDate(bonusIdy, DateTime.MinValue, DateTime.MaxValue, default(CancellationToken));

            Assert.NotNull(history);
            return (List<BonusHistoryDto>)history;
        }
        [Fact]
        public async Task<List<UserHistoryDto>> History_GetUserHistoryByUsageDateAsync_Return_ListUserHistoryDto()
        {
            CreateDefaultHistoryServiceInstance();
            var creatorId = _fakeHistoryDtos[0].CreatorId;
            var history = await _historyService.GetUserHistoryByUsageDate(creatorId, DateTime.MinValue, DateTime.MaxValue, default(CancellationToken));

            Assert.NotNull(history);
            return (List<UserHistoryDto>)history;
        }
        
        
        [Fact]
        public async Task<HistoryDto> HistoryId_GetUserHistoryByUsageDateAsync_Return_HistoryDto()
        {
            CreateDefaultHistoryServiceInstance();
            var Id = _fakeHistoryDtos[0].Id;
            var history = await _historyService.EstimateBonus(Id, 5,  default(CancellationToken));

            Assert.NotNull(history);
            return _mapper.Map<HistoryDto>(history);
        }



        private void CreateDefaultHistoryServiceInstance()
        {
            var myProfile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            var historyGenerator = Generator
                    .For<History>()
                    .For(x => x.Id)
                    .ChooseFrom(Guid.NewGuid())
                    .For(x => x.CreatorId)
                    .ChooseFrom(Guid.NewGuid())
                    .For(x => x.BonusId)
                    .ChooseFrom(Guid.NewGuid())
                    .For(x=>x.CreatedDate)
                    .ChooseFrom(DateTime.Now);
            _fakeHistoryDtos = historyGenerator.Generate(10).ToList();

            _historyRepo = new Mock<IHistoryRepository>();
            _historyRepo.Setup(s => s.GetAllAsync( default(CancellationToken))).ReturnsAsync(_mapper.Map<List<History>>(_fakeHistoryDtos));
            _historyRepo.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<History>(_fakeHistoryDtos[1])); 
            _historyRepo.Setup(s=>s.AddAsync(It.IsAny<History>(), default(CancellationToken)));
            _historyRepo.Setup(s=>s.RemoveAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<History>(_fakeHistoryDtos[0]));
            _historyRepo.Setup(s => s.GetBonusHistory(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<List<History>>(_fakeHistoryDtos));
            _historyRepo.Setup(s => s.GetUserHistory(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<List<History>>(_fakeHistoryDtos));
            _historyRepo.Setup(s=>s.GetBonusHistoryByUsageDate(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<List<History>>(_fakeHistoryDtos));
            _historyRepo.Setup(s=>s.GetUserHistoryByUsageDate(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<List<History>>(_fakeHistoryDtos));
            _historyRepo.Setup(x => x.GetCountHistoryByBonusIdAsync(It.IsAny<Guid>(), default(CancellationToken)))
                .ReturnsAsync(5);
            _bonusRepo = new Mock<IBonusRepository>();
            _bonusRepo.Setup(x =>
                    x.UpdateBonusRatingAsync(It.IsAny<Guid>(), It.IsAny<double>(), default(CancellationToken)))
                .ReturnsAsync(new Bonus());




            _mockhistoryRepository = _historyRepo.Object;
            _mockBonusRepository = _bonusRepo.Object;
            _historyService = new HistoryService(_mockhistoryRepository, _mockBonusRepository, _mapper);
        }
    }
    //public partial class Startup
    //{
    //    public Startup()
    //    {
    //        var a = 1;
    //    }

    //    protected void ConfigureServices(IServiceCollection services)
    //    {
    //        var myProfile = new MapperProfile();
    //        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
    //        var _mapper = new Mapper(configuration);

    //        var _vendorRep = new Mock<IVendorRepository>();
    //        _vendorRep.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Vendor>(new Vendor()));
    //        var _mockVendorRep = _vendorRep.Object;

    //        var _vendorService = new VendorService(_mockVendorRep, _mapper);
    //        services.AddSingleton<IVendorService>(_vendorService);
    //    }
    //}
}
