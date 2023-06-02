using BasicCRM.API.Controllers;
using BasicCRM.API.Models;
using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Business.Exceptions;
using BasicCRM.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace BasicCRM.API.Tests
{
    public class AddressControllerTests
    {
        private readonly Mock<IAddressService> _addressServiceMock;
        private readonly AddressController _controller;

        public AddressControllerTests()
        {
            _addressServiceMock = new Mock<IAddressService>();
            _controller = new AddressController(_addressServiceMock.Object);
        }

        [Fact]
        private async Task Should_ReturnOkStatusCode_WhenGetAllIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.GetAllAddressAsync())
                .ReturnsAsync(new List<GetAddressDto>() { new GetAddressDto(), new GetAddressDto() });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }

        [Fact]
        private async Task Should_ReturnResponse_WhenGetAllIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.GetAllAddressAsync())
                .ReturnsAsync(new List<GetAddressDto>() { new GetAddressDto(), new GetAddressDto() });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);

            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.IsType<Response>(okObjectResult.Value);
        }

        [Fact]
        private async Task Should_ReturnSuccessTrueInResponse_WhenGetAllIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.GetAllAddressAsync())
                .ReturnsAsync(new List<GetAddressDto>() { new GetAddressDto(), new GetAddressDto() });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsAssignableFrom<Response>(okObjectResult.Value);

            Assert.Equal(response.Success, true);
        }

        [Fact]
        private async Task Should_ReturnListGetAddressDtoInResponse_WhenGetAllIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.GetAllAddressAsync())
                .ReturnsAsync(new List<GetAddressDto>() { new GetAddressDto(), new GetAddressDto() });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsAssignableFrom<Response>(okObjectResult.Value);

            Assert.IsAssignableFrom<List<GetAddressDto>>(response.Content);
        }

        [Fact]
        private async Task Should_ReturnOkStatus_When_CallByIDIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.GetAddressAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new GetAddressDto());

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }

        [Fact]
        private async Task Should_ReturnResponseType_When_GetByIdIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.GetAddressAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new GetAddressDto());

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);

            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.IsType<Response>(okObjectResult.Value);
        }

        [Fact]
        private async Task Should_ReturnSuccessTrueInResponse_When_GetByIdIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.GetAddressAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new GetAddressDto());

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsAssignableFrom<Response>(okObjectResult.Value);

            Assert.True(response.Success);
        }

        [Fact]
        private async Task Should_ReturnGetAddressDtoInResponse_When_GetByIdIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.GetAddressAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new GetAddressDto());

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsAssignableFrom<Response>(okObjectResult.Value);

            Assert.IsAssignableFrom<GetAddressDto>(response.Content);
        }

        [Fact]
        private async Task Should_ReturnOkStatus_When_CreateAddressIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.CreateAddressAsync(It.IsAny<AddressToCreateDto>()))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _controller.CreateAddress(new AddressToCreateDto());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }

        [Fact]
        private async Task Should_ReturnResponseType_When_CreateAddressIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.CreateAddressAsync(It.IsAny<AddressToCreateDto>()))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _controller.CreateAddress(new AddressToCreateDto());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);

            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.IsType<Response>(okObjectResult.Value);
        }

        [Fact]
        private async Task Should_ReturnSuccessAndMessageInResponse_When_CreateAddressIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.CreateAddressAsync(It.IsAny<AddressToCreateDto>()))
                .ReturnsAsync(It.IsAny<Guid>());

            var expectedMessage = "Address created successfully";

            // Act
            var result = await _controller.CreateAddress(new AddressToCreateDto());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsAssignableFrom<Response>(okObjectResult.Value);

            Assert.True(response.Success);
            Assert.Equal(expectedMessage, response.Message);
        }

        [Fact]
        private async Task Should_ReturnOkStatus_When_UpdateAddressIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.UpdateAddressAsync(It.IsAny<AddressToUpdateDto>()));

            // Act
            var result = await _controller.UpdateAddress(new AddressToUpdateDto());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }

        [Fact]
        private async Task Should_ReturnResponseType_When_UpdateAddressIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.UpdateAddressAsync(It.IsAny<AddressToUpdateDto>()));

            // Act
            var result = await _controller.UpdateAddress(new AddressToUpdateDto());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);

            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.IsType<Response>(okObjectResult.Value);
        }

        [Fact]
        private async Task Should_ReturnSuccessAndMessageInResponse_When_UpdateAddressIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.UpdateAddressAsync(It.IsAny<AddressToUpdateDto>()));

            var expectedMessage = "Address updated successfully";

            // Act
            var result = await _controller.UpdateAddress(new AddressToUpdateDto());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsAssignableFrom<Response>(okObjectResult.Value);

            Assert.True(response.Success);
            Assert.Equal(expectedMessage, response.Message);
        }

        [Fact]
        private async Task Should_ReturnOkStatus_When_DeleteAddressIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.DeleteAddressAsync(It.IsAny<Guid>()));

            // Act
            var result = await _controller.DeleteAddress(Guid.NewGuid());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }

        [Fact]
        private async Task Should_ReturnResponseType_When_DeleteAddressIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.DeleteAddressAsync(It.IsAny<Guid>()));

            // Act
            var result = await _controller.DeleteAddress(Guid.NewGuid());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);

            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.IsType<Response>(okObjectResult.Value);
        }

        [Fact]
        private async Task Should_ReturnSuccessAndMessageInResponse_When_DeleteAddressIsCalled()
        {
            // Arrange
            _addressServiceMock.Setup(serv => serv.DeleteAddressAsync(It.IsAny<Guid>()));

            var expectedMessage = "Address deleted successfully";

            // Act
            var result = await _controller.DeleteAddress(Guid.NewGuid());

            // Assert
            var actionResult = Assert.IsType<ActionResult<Response>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsAssignableFrom<Response>(okObjectResult.Value);

            Assert.True(response.Success);
            Assert.Equal(expectedMessage, response.Message);
        }
    }
}
