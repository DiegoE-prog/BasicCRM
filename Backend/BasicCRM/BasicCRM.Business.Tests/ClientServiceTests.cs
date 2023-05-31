using Xunit;
using Moq;
using BasicCRM.Data.Repository.Interfaces;
using AutoMapper;
using BasicCRM.Business.Profiles;
using BasicCRM.Business.Services;
using BasicCRM.Data.Entities;
using BasicCRM.Business.Dtos.ClientDto;
using BasicCRM.Business.Exceptions;

namespace BasicCRM.Business.Tests
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _mockClientRepository;
        private readonly Mock<IAddressRepository> _mockAddressRepository;
        private readonly ClientService _service;

        public ClientServiceTests()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _mockClientRepository = new Mock<IClientRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg =>
                cfg.AddProfile<AutoMapperProfile>());

            var mapper = new Mapper(mapperConfiguration);

            _service = new ClientService(
                _mockClientRepository.Object,
                _mockAddressRepository.Object,
                mapper);
        }

        [Fact]
        public async Task Should_ListContainThreeClients_When_GetAllClientsAsyncReturnThreeClients()
        {
            // Arrange
            _mockClientRepository.Setup(repo => repo.GetAllClientsWithAddress())
                .ReturnsAsync(new List<ClientWithAddress>() { new ClientWithAddress(), new ClientWithAddress(), new ClientWithAddress() });

            // Act
            var result = await _service.GetAllClientsAsync();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task Should_TypeOfReturnBeListOfGetClientDto_When_GetAllClientsAsyncReturnAGetClientDto()
        {
            // Arrange
            _mockClientRepository.Setup(repo => repo.GetAllClientsWithAddress())
                .ReturnsAsync(new List<ClientWithAddress>() { new ClientWithAddress() });

            // Act
            var result = await _service.GetAllClientsAsync();

            // Assert
            Assert.IsType<List<GetClientDto>>(result);
        }

        [Fact]
        public async Task Should_ThrowErrorForEmptyList_When_GetAllClientsAsyncReturnsAnEmptyList()
        {
            // Arrange
            _mockClientRepository.Setup(repo => repo.GetAllClientsWithAddress())
                .ReturnsAsync(new List<ClientWithAddress>());
            
                var expectedMessage = "There are not clients registered";

            // Act
            var result = await Assert.ThrowsAsync<NotFoundException>(
                async () => await _service.GetAllClientsAsync());

            // Assert
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        public async Task Should_ReturnAnClientByIsId_When_IdIsValid()
        {
            // Arrange
            _mockClientRepository.Setup(repo => repo.GetByIdAsync(Guid.Parse("6f4a3a51-a386-401e-ab4e-c020e44fe20b")))
                .ReturnsAsync(new Client() {
                    ClientID = Guid.Parse("6f4a3a51-a386-401e-ab4e-c020e44fe20b"),
                    FirstName = "Diego",
                    LastName = "Escutia",
                    DateOfBirth = DateTime.Parse("07/05/1997"),
                    Email = "Diego.Escutia@gmail.com",
                    PhoneNumber = "1234567890",
                    AddressID = Guid.Parse("4e4f7a91-d6f4-42d7-979d-dddfc1c07d88")
                });

            var expectedResult = new GetClientDto()
            {
                ClientID = Guid.Parse("6f4a3a51-a386-401e-ab4e-c020e44fe20b"),
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = DateTime.Parse("07/05/1997"),
                Email = "Diego.Escutia@gmail.com",
                PhoneNumber = "1234567890",
                AddressID = Guid.Parse("4e4f7a91-d6f4-42d7-979d-dddfc1c07d88")
            };

            // Act
            var result = await _service.GetClientAsync(Guid.Parse("6f4a3a51-a386-401e-ab4e-c020e44fe20b"));

            // Assert
            Assert.Equal(expectedResult.ClientID, result.ClientID);
        }

        [Fact]
        public async Task Should_ReturnAnGetClientDto_When_GetClientByIdIsCalled()
        {
            // Arrange
            _mockClientRepository.Setup(repo => repo.GetByIdAsync(Guid.Parse("cdba4725-d1e5-4173-ab05-63cfd82fc98e")))
                .ReturnsAsync(new Client()
                {
                    ClientID = Guid.Parse("cdba4725-d1e5-4173-ab05-63cfd82fc98e")
                });

            // Act
            var result = await _service.GetClientAsync(Guid.Parse("cdba4725-d1e5-4173-ab05-63cfd82fc98e"));

            // Assert
            Assert.IsType<GetClientDto>(result);
        }

        [Fact]
        public async Task Should_ThrowNotFoundException_When_ClientForTheIdProvidedIsNotFounded()
        {
            // Arrage
            _mockClientRepository.Setup(repo => repo.GetByIdAsync(Guid.Parse("cdba4725-d1e5-4173-ab05-63cfd82fc98e")))
                .ReturnsAsync(new Client());

            var expectedMessage = "There is not a client with that ID";

            // Act
            var result = await Assert.ThrowsAsync<NotFoundException>
                (async ()=> await _service.GetClientAsync(Guid.Parse("cdba4725-d1e5-4173-ab05-63cfd82fc98e")));
        
            // Assert
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException_When_ClientToCreateDtoIsNotValid()
        {
            // Arrage
            var InvalidClientToCreateDto = new ClientToCreateDto(){};

            var expectedErrors = new Dictionary<string, string[]>()
            {
                {"FirstName", new string[]{"First Name is required"} },
                {"LastName", new string []{"Last Name is required"} },
                { "DateOfBirth", new string[]{"Date Of Birth is required" } },
                { "Email", new string[]{"Email is required"} },
                { "PhoneNumber", new string[]{"Phone Number is required"} }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.CreateClientAsync(InvalidClientToCreateDto));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException_When_ClientToCreateDtoHaveAnInvalidDateOfBirth()
        {
            // Arrage
            var InvalidClientToCreateDto = new ClientToCreateDto() 
            {
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(2050,07,05),
                Email = "Diego.Escutia@test.com",
                PhoneNumber = "1234567890",
            };

            var expectedErrors = new Dictionary<string, string[]>()
            {
                { "DateOfBirth", new string[]{ "You have to enter a valid date" } } 
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.CreateClientAsync(InvalidClientToCreateDto));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException_When_ClientToCreateDtoHaveAnInvalidEmail()
        {
            // Arrage
            var InvalidClientToCreateDto = new ClientToCreateDto()
            {
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(1997, 07, 05),
                Email = "Diego.Escutia.test.com",
                PhoneNumber = "1234567890",
            };

            var expectedErrors = new Dictionary<string, string[]>()
            {
                { "Email", new string[]{ "Email needs to have a valid format" } }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.CreateClientAsync(InvalidClientToCreateDto));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException_When_ClientToCreateDtoHaveAnInvalidPhoneNumber()
        {
            // Arrage
            var InvalidClientToCreateDto = new ClientToCreateDto()
            {
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(1997, 07, 05),
                Email = "Diego.Escutia@test.com",
                PhoneNumber = "123456789",
            };

            var expectedErrors = new Dictionary<string, string[]>()
            {
                { "PhoneNumber", new string[]{ "Length needs to be lower that 15 and higher than 10" } }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.CreateClientAsync(InvalidClientToCreateDto));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException_When_ClientToCreateDtoHaveAnInvalidAddressID()
        {
            // Arrage
            var InvalidClientToCreateDto = new ClientToCreateDto()
            {
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(1997, 07, 05),
                Email = "Diego.Escutia@test.com",
                PhoneNumber = "1234567890",
                AddressID = Guid.NewGuid()
            };

            _mockAddressRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Address());

            var expectedErrors = new Dictionary<string, string[]>()
            {
                { "AddressID", new string[]{ "There is not an address with that ID" } }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.CreateClientAsync(InvalidClientToCreateDto));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ReturnAGuid_When_ClientToCreateDtoIsValid()
        {
            // Arrage
            var clientID = Guid.NewGuid();
            var ValidClientToCreateDto = new ClientToCreateDto()
            {
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(1997, 07, 05),
                Email = "Diego.Escutia@test.com",
                PhoneNumber = "1234567890",
                AddressID = Guid.NewGuid()
            };

            _mockClientRepository.Setup(repo => repo.CreateAsync(It.IsAny<Client>())).
                ReturnsAsync(clientID);

            _mockAddressRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Address() { AddressID = Guid.NewGuid()});

            var expectedResult = clientID;

            // Act
            var result = await _service.CreateClientAsync(ValidClientToCreateDto);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException_When_ClientToUpdateDtoIsNotValid()
        {
            // Arrage
            var InvalidClientToUpdateDto = new ClientToUpdateDto() { };

            var expectedErrors = new Dictionary<string, string[]>()
            {
                {"ClientID", new string[]{"Client ID is required"}},
                {"FirstName", new string[]{"First Name is required"} },
                {"LastName", new string []{"Last Name is required"} },
                { "DateOfBirth", new string[]{"Date Of Birth is required" } },
                { "Email", new string[]{"Email is required"} },
                { "PhoneNumber", new string[]{"Phone Number is required"} }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.UpdateClientAsync(InvalidClientToUpdateDto));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException_When_ClientToUpdateDtoHaveAnInvalidDateOfBirth()
        {
            // Arrage
            var InvalidClientToUpdateDto = new ClientToUpdateDto()
            {
                ClientID = Guid.NewGuid(),
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(2050, 07, 05),
                Email = "Diego.Escutia@test.com",
                PhoneNumber = "1234567890",
            };

            _mockClientRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Client() { ClientID = Guid.NewGuid() });

            var expectedErrors = new Dictionary<string, string[]>()
            {
                { "DateOfBirth", new string[]{ "You have to enter a valid date" } }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.UpdateClientAsync(InvalidClientToUpdateDto));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException_When_ClientToUpdateDtoHaveAnInvalidEmail()
        {
            // Arrage
            var InvalidClientToUpdateDto = new ClientToUpdateDto()
            {
                ClientID = Guid.NewGuid(),
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(1997, 07, 05),
                Email = "Diego.Escutia.test.com",
                PhoneNumber = "1234567890",
            };

            _mockClientRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Client() { ClientID = Guid.NewGuid() });

            var expectedErrors = new Dictionary<string, string[]>()
            {
                { "Email", new string[]{ "Email needs to have a valid format" } }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.UpdateClientAsync(InvalidClientToUpdateDto));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException_When_ClientToUpdateDtoHaveAnInvalidPhoneNumber()
        {
            // Arrage
            var InvalidClientToUpdateDto = new ClientToUpdateDto()
            {
                ClientID = Guid.NewGuid(),
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(1997, 07, 05),
                Email = "Diego.Escutia@test.com",
                PhoneNumber = "123456789",
            };

            _mockClientRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Client() { ClientID = Guid.NewGuid() });

            var expectedErrors = new Dictionary<string, string[]>()
            {
                { "PhoneNumber", new string[]{ "Length needs to be lower that 15 and higher than 10" } }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.UpdateClientAsync(InvalidClientToUpdateDto));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException_When_ClientToUpdateDtoHaveAnInvalidAddressID()
        {
            // Arrage
            var InvalidClientToUpdateDto = new ClientToUpdateDto()
            {
                ClientID = Guid.NewGuid(),
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(1997, 07, 05),
                Email = "Diego.Escutia@test.com",
                PhoneNumber = "1234567890",
                AddressID = Guid.NewGuid()
            };

            _mockAddressRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Address());

            _mockClientRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Client() { ClientID = Guid.NewGuid()});

            var expectedErrors = new Dictionary<string, string[]>()
            {
                { "AddressID", new string[]{ "There is not an address with that ID" } }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.UpdateClientAsync(InvalidClientToUpdateDto));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_ThrowBadRequestException_When_ClientToUpdateDtoHaveAnInvalidClientID()
        {
            // Arrage
            var InvalidClientToUpdateDto = new ClientToUpdateDto()
            {
                ClientID = Guid.NewGuid(),
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(1997, 07, 05),
                Email = "Diego.Escutia@test.com",
                PhoneNumber = "1234567890"
            };

            _mockClientRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Client());

            var expectedErrors = new Dictionary<string, string[]>()
            {
                { "ClientID", new string[]{ "There is not a client with that ID" } }
            };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>
                (async () => await _service.UpdateClientAsync(InvalidClientToUpdateDto));

            // Assert
            Assert.Equal(expectedErrors, result.ValidationErrors);
        }

        [Fact]
        public async Task Should_UpdateClient_WhenClientToUpdateDtoIsValid()
        {
            // Arrage
            var clientID = Guid.NewGuid();
            var addressID = Guid.NewGuid();

            var ValidClientToUpdateDto = new ClientToUpdateDto()
            {
                ClientID = clientID,
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(1997, 07, 05),
                Email = "Diego.Escutia@test.com",
                PhoneNumber = "1234567890",
                AddressID = addressID
            };

            var expectedResult = new ClientToUpdateDto()
            {
                ClientID = clientID,
                FirstName = "Diego",
                LastName = "Escutia",
                DateOfBirth = new DateTime(1997, 07, 05),
                Email = "Diego.Escutia@test.com",
                PhoneNumber = "1234567890",
                AddressID = addressID
            };


            _mockAddressRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Address() { AddressID = addressID});

            _mockClientRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Client() { ClientID = clientID });

            // Act
            await _service.UpdateClientAsync(ValidClientToUpdateDto);

            // Assert
            Assert.Equal(expectedResult.FirstName +" "+ expectedResult.LastName, ValidClientToUpdateDto.FirstName + " " + ValidClientToUpdateDto.LastName);
        }

        [Fact]
        public async Task Should_ThrowNotFoundException_WhenIdIsInvalid()
        {
            // Arrange
            _mockClientRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Client());

            var expectedMessage = "There is not a client with that ID";

            //Act
            var result = await Assert.ThrowsAsync<NotFoundException>
                (async () => await _service.DeleteClientAsync(Guid.NewGuid()));

            //
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        public async Task Should_DeleteAClient_WhenIdIsValid()
        {
            // Arrange
            _mockClientRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Client() { ClientID = Guid.NewGuid()});

            //Act
            await _service.DeleteClientAsync(Guid.NewGuid());
        }
    }
}
