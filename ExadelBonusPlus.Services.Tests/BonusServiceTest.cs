using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using AutoMapper;
using ExadelBonusPlus.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SampleDataGenerator;
using Xunit;
using Assert = Xunit.Assert;

namespace ExadelBonusPlus.Services.Tests
{
    public class BonusServiceTest
    {
        private Mock<IBonusRepository> _bonusRep;
        private IBonusRepository _mockBonusRep;
        private IBonusService _bonusService;
        private Mock<IVendorRepository> _vendorRep;
        private IVendorRepository _mockVendorRep;
        private VendorService _vendorService;
        private IMapper _mapper;
        private List<BonusDto> _fakeBonuseDtos;

        public BonusServiceTest()
        {
            
        }

        public void Startup()
        {

        }

       [Fact]
        public async Task Bonus_AddBonusAsync_Return_BonusDto()
        {
            CreateDefaultBonusServiceInstance();
            var bonusDto = _mapper.Map<AddBonusDto>(_fakeBonuseDtos[0]);

            var bonus = await _bonusService.AddBonusAsync(bonusDto, Guid.Empty, default(CancellationToken));

            Assert.NotNull(bonus);
        }

        [Fact]
        public async Task Bonus_FindAllBonusAsync_Return_ListBonusDTO()
        {
            CreateDefaultBonusServiceInstance();

            var bonusList = await _bonusService.FindAllBonusesAsync(default(CancellationToken));

            Assert.NotNull(bonusList);
        }

        [Fact]
        public async Task Bonus_FindBonusesAsync_Return_ListBonusDTO()
        {
            CreateDefaultBonusServiceInstance();

            var bonusList = await _bonusService.FindBonusesAsync(new BonusFilter(), default(CancellationToken));

            Assert.NotNull(bonusList);
        }

        [Fact]
        public async Task Bonus_FindBonusByIdAsync_Return_BonusDTO()
        {
            CreateDefaultBonusServiceInstance();
            var idBonus = _fakeBonuseDtos[0].Id;

            var bonus = await _bonusService.FindBonusByIdAsync(idBonus, default(CancellationToken));

            Assert.NotNull(bonus);
        }

        [Fact]
        public async Task Bonus_UpdateBonusAsync_Return_BonusDTO()
        {
            CreateDefaultBonusServiceInstance();
            var idBonus = _fakeBonuseDtos[0].Id;
            var bonusDto = _mapper.Map<UpdateBonusDto>(_fakeBonuseDtos[0]);

            var bonus = await _bonusService.UpdateBonusAsync(idBonus, bonusDto, default(CancellationToken));

            Assert.NotNull(bonus);
        }

        [Fact]
        public async Task Bonus_DeleteBonusAsync_Return_BonusDTO()
        {
            CreateDefaultBonusServiceInstance();
            var idBonus = _fakeBonuseDtos[0].Id;

            var bonus = await _bonusService.DeleteBonusAsync(idBonus, default(CancellationToken));

            Assert.NotNull(bonus);
        }

        [Fact]
        public async Task Bonus_ActivateBonusAsync_Return_BonusDTO()
        {
            CreateDefaultBonusServiceInstance();
            var idBonus = _fakeBonuseDtos[0].Id;

            var bonus = await _bonusService.ActivateBonusAsync(idBonus, default(CancellationToken));

            Assert.True(bonus.IsActive);
        }

        [Fact]
        public async Task Bonus_DeactivateBonusAsync_Return_BonusDTO()
        {
            CreateDefaultBonusServiceInstance();
            var idBonus = _fakeBonuseDtos[0].Id;

            var bonus = await _bonusService.DeactivateBonusAsync(idBonus, default(CancellationToken));

            Assert.False(bonus.IsActive);
        }

        [Fact]
        public async Task Bonus_UpdateBonusRatingAsync_Return_BonusDTO()
        {
            CreateDefaultBonusServiceInstance();
            var idBonus = _fakeBonuseDtos[0].Id;
            double rating = 4.55;

            var bonus = await _bonusService.UpdateBonusRatingAsync(idBonus, rating, default(CancellationToken));

            Assert.True(bonus.Rating > 0);
        }

        [Fact]
        public async Task Bonus_GetBonusTagsAsync_Return_ListString()
        {
            CreateDefaultBonusServiceInstance();
            
            var tags = await _bonusService.GetBonusTagsAsync(default(CancellationToken));

            Assert.True(tags.Count()>0);
        }

        private void CreateDefaultBonusServiceInstance()
        {
            var bonusGenerator = Generator
                    .For<BonusDto>()
                    .For(x => x.Id)
                    .ChooseFrom(Guid.NewGuid())
                    .For(x => x.Title)
                    .ChooseFrom(StaticData.LoremIpsum)
                    .For(x => x.Description)
                    .ChooseFrom(StaticData.LoremIpsum)
                    .For(x => x.DateStart)
                    .ChooseFrom(DateTime.Now)
                    .For(x => x.DateEnd)
                    .ChooseFrom(DateTime.Now)
                    .For(x => x.IsActive)
                    .ChooseFrom(true)
                ;

            _fakeBonuseDtos = bonusGenerator.Generate(10).ToList();

            var vendorService = new Mock<IVendorService>();
            vendorService.Setup(x => x.GetVendorByIdAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(new VendorDto());
            var historyService = new Mock<IHistoryService>();
            historyService.Setup(x => x.GetCountHistoryByBonusIdAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(It.IsAny<int>());
            var bonusService = new Mock<IBonusService>();

            IServiceCollection services = new ServiceCollection();
            services.AddTransient<IVendorService>(sp => vendorService.Object);
            services.AddTransient<IHistoryService>(sp => historyService.Object);
            services.AddTransient<IBonusService>(sp => bonusService.Object);

            services.AddAutoMapper(typeof(MapperProfile));

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            _mapper = serviceProvider.GetService<IMapper>();

            var mock = AutoMock.GetLoose(cfg =>
            {
                cfg.RegisterInstance(_mapper).As<IMapper>();
                cfg.RegisterInstance(vendorService.Object).As<IVendorService>();
                cfg.RegisterInstance(historyService.Object).As<IHistoryService>();
            });
            _bonusRep = mock.Mock<IBonusRepository>();
            _bonusRep.Setup(s => s.AddAsync(It.IsAny<Bonus>(), default(CancellationToken)));
            _bonusRep.Setup(s => s.GetAllAsync(default(CancellationToken))).ReturnsAsync(_mapper.Map<List<Bonus>>(_fakeBonuseDtos));
            _bonusRep.Setup(s => s.GetBonusesAsync(It.IsAny<BonusFilter>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<List<Bonus>>(_fakeBonuseDtos));
            _bonusRep.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Bonus>(_fakeBonuseDtos[0]));
            _bonusRep.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Bonus>(), default(CancellationToken)));
            _bonusRep.Setup(s => s.RemoveAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Bonus>(_fakeBonuseDtos[0]));
            _bonusRep.Setup(s => s.ActivateBonusAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Bonus>(_fakeBonuseDtos[0]));
            _fakeBonuseDtos[9].IsActive = false;
            _bonusRep.Setup(s => s.DeactivateBonusAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Bonus>(_fakeBonuseDtos[9]));
            _fakeBonuseDtos[0].Rating = 3.00;
            _bonusRep.Setup(s => s.UpdateBonusRatingAsync(It.IsAny<Guid>(), It.IsAny<double>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Bonus>(_fakeBonuseDtos[0]));
            _bonusRep.Setup(s => s.GetBonusTagsAsync(default(CancellationToken))).ReturnsAsync(new List<string>(){"Pizza","Coffee"});
            
            _bonusService = mock.Create<BonusService>();
        }
    }
}
