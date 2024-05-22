
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using Microsoft.AspNetCore.Identity;

namespace JewelryEC_Backend.Tests.JewelryEC_Backend.Tests.Service
{
    public  class AuthServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly AuthService _authService;
        public AuthServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            _mockEmailSender = new Mock<IEmailSender>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _authService = new AuthService(
                _mockMapper.Object,
                _mockJwtTokenGenerator.Object,
                _mockEmailSender.Object,
                _mockUnitOfWork.Object);
        }
        #region Test case for Register
        [Fact]
        public async Task Register_WithValidOtp_ShouldReturnTrue()
        {
            // Arrange
            var registrationDto = new RegistrationDto
            {
                Email = "test@example.com",
                OTP = "123456",
                Password = "Password123!",
                Name = "Test User",
                PhoneNumber = "1234567890"
            };

            var applicationUser = new ApplicationUser
            {
                UserName = registrationDto.Email.ToLower(),
                Email = registrationDto.Email,
                NormalizedEmail = registrationDto.Email.ToUpper(),
                Name = registrationDto.Name,
                PhoneNumber = registrationDto.PhoneNumber
            };
          
            mockCheckOTP(registrationDto.OTP, DateTime.UtcNow);

            _mockUnitOfWork.Setup(uow => uow.Users.AddUserByUserManager(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);


            // Act
            var result = await _authService.Register(registrationDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Register_WithInvalidOtp_ShouldReturnFalse()
        {
            // Arrange
            var registrationDto = new RegistrationDto
            {
                Email = "test@example.com",
                OTP = "928272",
                Password = "Password123!",
                Name = "Test User",
                PhoneNumber = "1234567890"
            };
            mockCheckOTP("123456", DateTime.UtcNow);

            // Act
            var result = await _authService.Register(registrationDto);

            // Assert
            Assert.False(result);
        }

        private void mockCheckOTP(string OTP, DateTime expiry)
        {
            var emailVerification = new EmailVerification
            {
                Otp = OTP,
                Created_at = expiry,
            };
            _mockUnitOfWork.Setup(uow => uow.EmailVerifications.GetEntityByEmail(It.IsAny<string>())).Returns(emailVerification);
        }

        [Fact]
        public async Task Register_WhenUserCreationFails_ShouldThrowExceptionWithErrorDescription()
        {
            // Arrange
            var registrationDto = new RegistrationDto
            {
                Email = "test@example.com",
                OTP = "123456",
                Password = "Password123!",
                Name = "Test User",
                PhoneNumber = "1234567890"
            };
            mockCheckOTP(registrationDto.OTP, DateTime.UtcNow);

            var identityError = new IdentityError { Description = "User creation failed" };
            var identityResult = IdentityResult.Failed(identityError);

            _mockUnitOfWork.Setup(uow => uow.Users.AddUserByUserManager(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _authService.Register(registrationDto));
            Assert.Equal($"Could not create user {registrationDto.Email.ToLower()}: {identityError.Description}", exception.Message);
        }

        [Fact]
        public async Task Register_WhenExceptionIsThrown_ShouldThrowExceptionWithGeneralMessage()
        {
            // Arrange
            var registrationDto = new RegistrationDto
            {
                Email = "test@example.com",
                OTP = "123456",
                Password = "Password123!",
                Name = "Test User",
                PhoneNumber = "1234567890"
            };

            _mockUnitOfWork.Setup(uow => uow.Users.AddUserByUserManager(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database error"));

            mockCheckOTP(registrationDto.OTP, DateTime.UtcNow);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _authService.Register(registrationDto));
            Assert.Equal($"Could not create user {registrationDto.Email.ToLower()}: One or more errors occurred. (Database error)", exception.Message);
        }
        #endregion
    }

}
