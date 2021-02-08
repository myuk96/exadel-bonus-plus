﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExadelBonusPlus.Services.Models;
using ExadelBonusPlus.Services.Properties;
using Moq;
using SampleDataGenerator;
using Xunit;

namespace ExadelBonusPlus.Services.Tests
{
    public class BonusServiceTest
    {
        private Mock<IBonusRepository> _bonusRep;
        private IBonusRepository _mockBonusRep;
        private Mock<IBonusTagRepository> _bonusTagRep;
        private IBonusTagRepository _mockBonusTagRep;
        private BonusService _bonusService;
        private IMapper _mapper;
        private List<BonusDto> _fakeBonuseDtos;

        public BonusServiceTest()
        {
        }

        [Fact]
        public async Task<BonusDto> Bonus_AddBonusAsync_Return_BonusDto()
        {
            CreateDefaultBonusServiceInstance();
            var bonusDto = _mapper.Map<AddBonusDto>(_fakeBonuseDtos[0]);

            var bonus = await _bonusService.AddBonusAsync(bonusDto, default(CancellationToken));

            Assert.NotNull(bonus);
            return bonus;
        }

        [Fact]
        public async Task<List<BonusDto>> Bonus_FindAllBonusAsync_Return_ListBonusDTO()
        {
            CreateDefaultBonusServiceInstance();

            var bonusList = await _bonusService.FindAllBonusAsync(default(CancellationToken));

            Assert.NotNull(bonusList);
            return bonusList;
        }

        [Fact]
        public async Task<List<BonusDto>> Bonus_FindAllActiveBonusAsync_Return_ListBonusDTO()
        {
            CreateDefaultBonusServiceInstance();

            var bonusList = await _bonusService.FindAllActiveBonusAsync(default(CancellationToken));

            Assert.NotNull(bonusList);
            return bonusList;
        }

        [Fact]
        public async Task<BonusDto> Bonus_FindBonusByIdAsync_Return_BonusDTO()
        {
            CreateDefaultBonusServiceInstance();
            var idBonus = _fakeBonuseDtos[0].Id;

            var bonus = await _bonusService.FindBonusByIdAsync(idBonus, default(CancellationToken));

            Assert.NotNull(bonus);
            return bonus;
        }

        [Fact]
        public async Task<BonusDto> Bonus_UpdateBonusAsync_Return_BonusDTO()
        {
            CreateDefaultBonusServiceInstance();
            var idBonus = _fakeBonuseDtos[0].Id;
            var bonusDto = _fakeBonuseDtos[0];

            var bonus = await _bonusService.UpdateBonusAsync(idBonus, bonusDto, default(CancellationToken));

            Assert.NotNull(bonus);
            return bonus;
        }

        [Fact]
        public async Task<BonusDto> Bonus_DeleteBonusAsync_Return_BonusDTO()
        {
            CreateDefaultBonusServiceInstance();
            var idBonus = _fakeBonuseDtos[0].Id;

            var bonus = await _bonusService.DeleteBonusAsync(idBonus, default(CancellationToken));

            Assert.NotNull(bonus);
            return bonus;
        }

        [Fact]
        public async Task<BonusDto> Bonus_ActivateBonusAsync_Return_BonusDTO()
        {
            CreateDefaultBonusServiceInstance();
            var idBonus = _fakeBonuseDtos[0].Id;

            var bonus = await _bonusService.ActivateBonusAsync(idBonus, default(CancellationToken));

            Assert.True(bonus.IsActive);
            return bonus;
        }

        [Fact]
        public async Task<BonusDto> Bonus_DeactivateBonusAsync_Return_BonusDTO()
        {
            CreateDefaultBonusServiceInstance();
            var idBonus = _fakeBonuseDtos[0].Id;

            var bonus = await _bonusService.DeactivateBonusAsync(idBonus, default(CancellationToken));

            Assert.False(bonus.IsActive);
            return bonus;
        }

        private void CreateDefaultBonusServiceInstance()
        {
            var myProfile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

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

            _bonusRep = new Mock<IBonusRepository>();
            _bonusRep.Setup(s => s.AddAsync(It.IsAny<Bonus>(), default(CancellationToken)));
            _bonusRep.Setup(s => s.GetAllAsync(default(CancellationToken))).ReturnsAsync(_mapper.Map<List<Bonus>>(_fakeBonuseDtos));
            _bonusRep.Setup(s => s.GetAllActiveBonusAsync(default(CancellationToken))).ReturnsAsync(_mapper.Map<List<Bonus>>(_fakeBonuseDtos));
            _bonusRep.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Bonus>(_fakeBonuseDtos[0]));
            _bonusRep.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Bonus>(), default(CancellationToken)));
            _bonusRep.Setup(s => s.RemoveAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Bonus>(_fakeBonuseDtos[0]));
            _bonusRep.Setup(s => s.ActivateBonusAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Bonus>(_fakeBonuseDtos[0]));
            _fakeBonuseDtos[9].IsActive = false;
            _bonusRep.Setup(s => s.DeactivateBonusAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Bonus>(_fakeBonuseDtos[9]));

            _mockBonusRep = _bonusRep.Object;

            _bonusTagRep = new Mock<IBonusTagRepository>();
            _bonusTagRep.Setup(s => s.AddAsync(It.IsAny<BonusTag>(), default(CancellationToken)));

            _mockBonusTagRep = _bonusTagRep.Object;

            _bonusService = new BonusService(_mockBonusRep, _mockBonusTagRep, _mapper);
        }
}
}