using DocumentManagementApi.Controllers;
using DocumentManagementApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DocumentManagementApi.Tests
{
    [TestFixture]
    public class DocumentControllerTests
    {
        private Mock<IFileValidator> _mockFileValidator;
        private Mock<IDocumentRepository> _mockDocumentRepository;
        private Mock<ILogger<DocumentController>> _mockLogger;
        private DocumentController _subject;

        [SetUp]
        public void Setup()
        {
            _mockFileValidator = new Mock<IFileValidator>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockLogger = new Mock<ILogger<DocumentController>>();
            _subject = new DocumentController(_mockFileValidator.Object, 
                _mockDocumentRepository.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task Upload_Returns_BadRequest_If_Invalid_File()
        {
            // arrange
            _mockFileValidator.Setup(x => x.IsValid(It.IsAny<IFormFile>())).Returns(false);

            // act
            var result = await _subject.Upload(null);

            // assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);            
        }

        [Test]
        public async Task Upload_Returns_CreatedRequest_If_Valid_File()
        {
            // arrange
            var file = new FormFile(null, 0, 5242880, "test", "test.pdf");
            _mockFileValidator.Setup(x => x.IsValid(file)).Returns(true).Verifiable();
            _mockDocumentRepository.Setup(x => x.Process(file)).ReturnsAsync(true).Verifiable();

            // act
            var result = await _subject.Upload(file);

            // assert            
            Assert.IsInstanceOf<CreatedResult>(result);

            _mockFileValidator.Verify(x => x.IsValid(file), Times.Once);
            _mockDocumentRepository.Verify(x => x.Process(file), Times.Once);

        }

        [Test]
        public async Task Upload_Returns_ProblemRequest_When_Error_Occurred()
        {
            // arrange
            var file = new FormFile(null, 0, 5242880, "test", "test.pdf");
            _mockFileValidator.Setup(x => x.IsValid(file)).Returns(true).Verifiable();
            _mockDocumentRepository.Setup(x => x.Process(file)).ReturnsAsync(false).Verifiable();

            // act
            var result = await _subject.Upload(file);

            // assert            
            Assert.IsInstanceOf<ProblemDetails>((result as ObjectResult).Value);
            
            _mockFileValidator.Verify(x => x.IsValid(file), Times.Once);
            _mockDocumentRepository.Verify(x => x.Process(file), Times.Once);

        }

        /*
         TODO: Add tests for other methods on the controller.
         */
    }
}
