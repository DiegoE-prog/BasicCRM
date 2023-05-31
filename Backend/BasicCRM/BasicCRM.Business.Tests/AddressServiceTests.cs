using AutoMapper;
using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Business.Exceptions;
using BasicCRM.Business.Profiles;
using BasicCRM.Business.Services;
using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using Moq;
using Xunit;

namespace BasicCRM.Business.Tests
{
    public class AddressServiceTests
    {
        private readonly Mock<IAddressRepository> _mockRepo;
        private readonly AddressService _service;

        public AddressServiceTests()
        {
            _mockRepo = new Mock<IAddressRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg =>
                cfg.AddProfile<AutoMapperProfile>());

            var mapper = new Mapper(mapperConfiguration);

            _service = new AddressService(_mockRepo.Object, mapper);

           
        }

        [Fact]
        public async Task Should_ListCountTwo_When_GetAllAddressAsyncReturnAListWithTwoElements()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Address>() { new Address(), new Address() });

            // Act
            var result = await _service.GetAllAddressAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Should_TypeIsGetAddressDto_When_GetAllAddressAsyncReturnsAListOfAddress()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Address>() { new Address(), new Address() });

            // Act
            var result = await _service.GetAllAddressAsync();

            //Assert
            Assert.IsType<List<GetAddressDto>>(result);
        }

        [Fact]
        public async Task Should_ThrowNotFoundException_When_GetAllAddressAsyncReturnsAnEmptyList()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Address>());
            var expectedMessage = "There are not addresses register";

            // Act
            var result = await Assert.ThrowsAsync<NotFoundException>(async () => await _service.GetAllAddressAsync());

            //Assert
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        public async Task Should_GetAddressDtoIsEqual_When_GetAddressByIdInputAValidId()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(Guid.Parse("4e4f7a91-d6f4-42d7-979d-dddfc1c07d88")))
                .ReturnsAsync(new Address()
                {
                    AddressID = Guid.Parse("4e4f7a91-d6f4-42d7-979d-dddfc1c07d88"),
                    AddressLine = "16 De Septiembre",
                    AddressDetails = "Red Facade",
                    City = "Moroleon",
                    State = "Guanjuato",
                    Country = "Mexico",
                    ZipCode = 38887
                });

            var expectedResult = new GetAddressDto()
            {
                AddressID = Guid.Parse("4e4f7a91-d6f4-42d7-979d-dddfc1c07d88"),
                AddressLine = "16 De Septiembre",
                AddressDetails = "Red Facade",
                City = "Moroleon",
                State = "Guanjuato",
                Country = "Mexico",
                ZipCode = 38887
            };

            // Act
            var result = await _service.GetAddressAsync(Guid.Parse("4e4f7a91-d6f4-42d7-979d-dddfc1c07d88"));

            // Assert
            Assert.Equal(expectedResult.AddressLine, result.AddressLine);
        }

        [Fact]
        public async Task Should_ThrowNotFoundException_When_GetAddressByIdInputAnInvalidId()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(Guid.Parse("bb57bd97-621f-464b-a03c-a27e812a85ad")))
                .ReturnsAsync(new Address(){});

            var expectedMessage = "There is not an address with that ID";

            // Act
            var result = await Assert.ThrowsAsync<NotFoundException>(async () => 
                await _service.GetAddressAsync(Guid.Parse("bb57bd97-621f-464b-a03c-a27e812a85ad")));

            // Assert
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        public async Task Should_TypeIsGetAddressDto_WhenGetAddressByIdInputAValidId()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(Guid.Parse("4e4f7a91-d6f4-42d7-979d-dddfc1c07d88")))
                .ReturnsAsync(new Address()
                {
                    AddressID = Guid.Parse("4e4f7a91-d6f4-42d7-979d-dddfc1c07d88"),
                    AddressLine = "16 De Septiembre",
                    AddressDetails = "Red Facade",
                    City = "Moroleon",
                    State = "Guanjuato",
                    Country = "Mexico",
                    ZipCode = 38887
                });

            // Act
            var result = await _service.GetAddressAsync(Guid.Parse("4e4f7a91-d6f4-42d7-979d-dddfc1c07d88"));

            // Assert
            Assert.IsType<GetAddressDto>(result);
        }

        [Fact]
        public async Task Should_ReturnAGuid_When_AddressToCreateInputAValidAddressToCreateDto()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Address>()))
                .ReturnsAsync(Guid.Parse("7054a2d4-33b2-43de-b3b8-8f918617e3d7"));

            var addressToCreate = new AddressToCreateDto()
            {
                AddressID = Guid.Parse("7054a2d4-33b2-43de-b3b8-8f918617e3d7"),
                AddressLine = "5 de Mayo",
                City = "Moroleon",
                State = "Guanajuato",
                Country = "Mexico",
                ZipCode = 38800
            };

            var expectedResult = addressToCreate.AddressID;

            // Act
            var result = await _service.CreateAddressAsync(addressToCreate);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Should_ThrowBadRequestExeption_When_AddressToCreateDtoIsNotValid()
        {
            // Arrange
            var addressToCreate = new AddressToCreateDto()
            {
                AddressID = Guid.Parse("7054a2d4-33b2-43de-b3b8-8f918617e3d7"),
                AddressLine = "5 de Mayo",
                City = "Moroleon"
            };

            // Assert
            await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.CreateAddressAsync(addressToCreate));
        }

        [Fact]
        public async Task Should_ThrowBadRequestExceptionWithMessage_When_AddressToCreateDtoIsNotValid()
        {
            // Arrange
            var addressToCreate = new AddressToCreateDto();

            Dictionary<string, string[]> expectedErrors = new()
            {
                { "AddressLine", new string[]{"Address Line is required" } },
                { "City", new string[]{"City is required" } },
                { "State", new string[]{"State is required" } },
                { "ZipCode", new string[]{"Zip Code is required", "Zip Code need to be 5 digits" } },
                { "Country", new string[]{"Country is required"} }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.CreateAddressAsync(addressToCreate));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ThrowBadRequestExceptionWithMessageForZipCodeField_When_AddressToCreateDtoHaveANotValidZipCode()
        {
            // Arrange
            var addressToCreate = new AddressToCreateDto()
            {
                AddressLine = "20 de Noviembre",
                City = "Moroleon",
                State = "Guanajuato",
                Country = "Mexico",
                ZipCode = 123456
            };

            var expectedErrors = new Dictionary<string, string[]>
            {
                { "ZipCode", new string[]{"Zip Code need to be 5 digits" } }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.CreateAddressAsync(addressToCreate));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);

        }

        [Fact]
        public async Task Should_ThrowBadRequestExeption_When_AddressToUpdateDtoIsNotValid()
        {
            // Arrange
            var addressToUpdate = new AddressToUpdateDto();

            // Assert
            await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.UpdateAddressAsync(addressToUpdate));
        }

        [Fact]
        public async Task Should_ThrowBadRequestExceptionWithMessages_When_AddressToUpdateDtoIsNotValid()
        {
            // Arrange
            var addressToUpdate = new AddressToUpdateDto();

            Dictionary<string, string[]> expectedErrors = new()
            {
                { "AddressID", new string[]{"Address ID is required"} },
                { "AddressLine", new string[]{"Address Line is required" } },
                { "City", new string[]{"City is required" } },
                { "State", new string[]{"State is required" } },
                { "ZipCode", new string[]{"Zip Code is required", "Zip Code need to be 5 digits" } },
                { "Country", new string[]{"Country is required"} }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.UpdateAddressAsync(addressToUpdate));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ThrowBadRequestExceptionWithMessageForAddressID_When_AddressToUpdateDtoHaveAnInvalidAddressID()
        {
            // Arrange
            var id = Guid.Parse("ca2a55c2-3bcf-4f91-86d8-ed20913ec0f0");
            _mockRepo.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(new Address() { });

            var addressToUpdate = new AddressToUpdateDto
            {
                AddressID = id,
                AddressLine = "10 de Mayo",
                AddressDetails = "Red Facade",
                City = "Moroleon",
                State = "Guanajuato",
                Country = "Mexico",
                ZipCode = 12345
            };

            Dictionary<string, string[]> expectedErrors = new()
            {
                { "AddressID", new string[]{ "There is no address with that ID" } },
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.UpdateAddressAsync(addressToUpdate));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_UpdateAnAddress_When_AddressToUpdateDtoIsValid()
        {
            // Arrange
            var id = Guid.Parse("624f874c-f33c-4f13-b4fd-f2eb55e0b2e7");
            _mockRepo.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(new Address() 
                {
                    AddressID = id,
                });

            var addressToUpdate = new AddressToUpdateDto
            {
                AddressID = id,
                AddressLine = "10 de Mayo",
                AddressDetails = "Red Facade",
                City = "Moroleon",
                State = "Guanajuato",
                Country = "Mexico",
                ZipCode = 12345
            };

            // Act
            await _service.UpdateAddressAsync(addressToUpdate);
        }

        [Fact]
        public async Task Should_ThrowNotFoundException_When_IdIsInvalid()
        {
            // Arrange
            var id = Guid.Parse("74b1ea52-32cc-4c95-a9c3-8cbefdfb2db2");

            _mockRepo.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(new Address() { });

            // Assert
            await Assert.ThrowsAsync<NotFoundException>
                (async () => await _service.DeleteAddressAsync(id));
        }

        [Fact]
        public async Task Should_DeleteAnAddress_When_IdIsValid()
        {
            // Arrange
            var id = Guid.Parse("797ec187-5398-4cb6-9467-48558ccf747c");

            _mockRepo.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(new Address() 
                {
                    AddressID = id,
                });

            // Act
            await _service.DeleteAddressAsync(id);
        }
    }
}
