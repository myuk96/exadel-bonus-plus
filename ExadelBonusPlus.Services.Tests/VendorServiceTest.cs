using Autofac;
using Autofac.Extras.Moq;
using AutoMapper;
using Bogus;
using ExadelBonusPlus.Services.Models;
using FluentValidation;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExadelBonusPlus.Services.Tests
{
    public class VendorServiceTest
    {
        private IMapper _mapper;
        private Task<IEnumerable<VendorDto>> _fakeVendorDtos;
        private Task<IEnumerable<Vendor>> _fakeVendors;
        private VendorService _vendorService;
        private IValidator<VendorDto> _vendorDtoValidator;
        private IVendorRepository _vendorRepository;
        private IValidator<AddVendorDto> _addVendorDtoValidator;

        public VendorServiceTest()
        {
            _fakeVendorDtos = GenerateSampleVendors();
            _fakeVendors = MapDtoListToDomainObjectList(_fakeVendorDtos.Result);

        }
        [Fact]
        public void GetAllVendorAsync_ShouldReturn_ListVendorDtos()
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_mapper).As<IMapper>()))
            {
                mock.Mock<IVendorRepository>()
                    .Setup(x => x.GetAllAsync(default))
                    .Returns(_fakeVendors);

                var serviceInstance = mock.Create<VendorService>();

                var expected = _fakeVendorDtos;
                var actual = serviceInstance.GetAllVendorsAsync(default);

                Xunit.Assert.NotNull(actual);
                Xunit.Assert.Equal(expected.Result.Count(), actual.Result.Count());
            }
        }


        [Fact]
        public void DeleteVendorAsync_ShouldDeleteInstance()
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_mapper).As<IMapper>()))
            {
                var vendor = _fakeVendors.Result.First();
                mock.Mock<IVendorRepository>()
                    .Setup(x => x.RemoveAsync(vendor.Id, default))
                    .Returns(Task.FromResult(vendor));

                var serviceInstance = mock.Create<VendorService>();

                var expected = vendor;
                var actual = serviceInstance.DeleteVendorAsync(vendor.Id,default).Result;

                mock.Mock<IVendorRepository>()
                   .Verify(v => v.RemoveAsync(vendor.Id, default),
                                       Times.Exactly(1));
                Xunit.Assert.NotNull(actual);
                Xunit.Assert.Equal(expected.Id, actual.Id);
            }
        }
        [Fact]
        public void GetVendorByIdAsync_ShouldReturnInstance()
        {
            using (var mock = AutoMock.GetLoose(cfg => cfg.RegisterInstance(_mapper).As<IMapper>()))
            {
                var vendor = _fakeVendors.Result.First();
                mock.Mock<IVendorRepository>()
                    .Setup(x => x.GetByIdAsync(vendor.Id, default))
                    .Returns(Task.FromResult(vendor));

                var serviceInstance = mock.Create<VendorService>();

                var expected = vendor;
                var actual = serviceInstance.GetVendorByIdAsync(vendor.Id, default).Result;

                mock.Mock<IVendorRepository>()
                   .Verify(v => v.GetByIdAsync(vendor.Id, default),
                                       Times.Exactly(1));
                Xunit.Assert.NotNull(actual);
                Xunit.Assert.Equal(expected.Id, actual.Id);
            }
        }

        /// <summary>
        /// Generates realistic fake data
        /// </summary>
        /// <returns>Vendor objects</returns>
        private async Task<IEnumerable<VendorDto>> GenerateSampleVendors()
        {
            var vendorsGenerator = new Faker<VendorDto>()
                .RuleFor(v => v.Id, f => Guid.NewGuid())
                .RuleFor(v => v.Name, f => f.Company.CompanyName())
                .RuleFor(v => v.Email, (f, v) => f.Internet.Email(v.Name));

            return await Task.Run(() => vendorsGenerator.Generate(10));
        }

        private async Task<IEnumerable<Vendor>> MapDtoListToDomainObjectList(IEnumerable<VendorDto> source)
        {
            var myProfile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            var result = await Task.Run(() => _mapper.Map<IEnumerable<Vendor>>(source));
            return result;
        }
    }
}
