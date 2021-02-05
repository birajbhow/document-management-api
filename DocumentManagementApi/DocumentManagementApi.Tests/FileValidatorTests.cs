using DocumentManagementApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace DocumentManagementApi.Tests
{
    public class FileValidatorTests
    {
        private FileValidator _fileValidator;

        [SetUp]
        public void Setup()
        {   
            var mockConfiguration = new Mock<IConfiguration>();
            var mockFileSizeSection = new Mock<IConfigurationSection>();
            mockConfiguration.Setup(x => x.GetSection("FileSizeLimitInBytes")).Returns(mockFileSizeSection.Object);
            mockFileSizeSection.Setup(x => x.Value).Returns("5242880");
            var mockExtensionSection = new Mock<IConfigurationSection>();
            mockConfiguration.Setup(x => x.GetSection("PermittedFileExtensioins")).Returns(mockExtensionSection.Object);
            mockExtensionSection.Setup(x => x.Value).Returns(".pdf,.txt");
            _fileValidator = new FileValidator(mockConfiguration.Object);
        }

        [Test]
        public void Validate_When_File_Size_Zero()
        {
            // arrange
            var file = new FormFile(null, 0, 0, "test", "test.pdf");

            // act
            var result = _fileValidator.IsValid(file);

            // assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Validate_Max_File_Size()
        {
            // arrange
            var file = new FormFile(null, 0, 5242881, "test", "test.pdf");

            // act
            var result = _fileValidator.IsValid(file);

            // assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Validate_File_Extension_Jpg()
        {
            // arrange
            var file = new FormFile(null, 0, 5242880, "test", "test.jpg");

            // act
            var result = _fileValidator.IsValid(file);

            // assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Validate_File_Extension_pdf()
        {
            // arrange
            var file = new FormFile(null, 0, 5242880, "test", "test.pdf");

            // act
            var result = _fileValidator.IsValid(file);

            // assert
            Assert.IsTrue(result);
        }
    }
}