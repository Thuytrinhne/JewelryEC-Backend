
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Moq;
using StackExchange.Redis;
using System.Data;
using System.Security.Authentication;

namespace JewelryEC_Backend.Tests.JewelryEC_Backend.Tests.Service
{
    public  class AuthServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserService> _mockUserService;

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
                _mockUnitOfWork.Object,
                _mockUserService.Object);
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
                Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                UserName = registrationDto.Email.ToLower(),
                Email = registrationDto.Email,
                NormalizedEmail = registrationDto.Email.ToUpper(),
                Name = registrationDto.Name,
                PhoneNumber = registrationDto.PhoneNumber
            };
          
            mockCheckOTP(registrationDto.OTP, DateTime.UtcNow);

            _mockUnitOfWork.Setup(uow => uow.Users.AddUserByUserManager(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            mockGetUserById(applicationUser);
            mockAssignRoleForUser(true);

            // Act
            var result = await _authService.Register(registrationDto);

            // Assert
            Assert.True(result);
        }

        private  void mockAssignRoleForUser(bool IsSuccess)
        {
              _mockUnitOfWork.Setup(uow => uow.Users.AssignRoleForUser(It.IsAny<ApplicationUser>(), It.IsAny<Guid>())).ReturnsAsync(IsSuccess);
        }

        private void mockGetUserById(ApplicationUser applicationUser)
        {
            _mockUnitOfWork.Setup(uow => uow.Users.GetUserById(It.IsAny<Guid>())).Returns(applicationUser);
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
            Assert.Equal($"Could not create user {registrationDto.Email.ToLower()}: Database error", exception.Message);
        }
        #endregion

        #region Test case for Login
        [Fact]
        public async Task Login_WithValidCredentials_ReturnsLoginResponseDto()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
            var applicationUser = new ApplicationUser
            {
                Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                UserName = loginDto.Email.ToLower(),
                Email = loginDto.Email,
                NormalizedEmail = loginDto.Email.ToUpper(),
                Name = "Thuy Trinh",
                PhoneNumber = "0928262522"
            };
            var userDto = new UserDto
            {
                Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                Email = loginDto.Email,
                Name = "Thuy Trinh",
                PhoneNumber = "0928262522"
            };
            var roles = new List<string> { "User"};
            var token = "mocked_jwt_token";

            _mockUnitOfWork.Setup(uow => uow.Users.GetUserByEmail(loginDto.Email)).ReturnsAsync(applicationUser);
            _mockUnitOfWork.Setup(uow => uow.Users.Login(applicationUser, loginDto.Password)).ReturnsAsync(true);
            _mockUnitOfWork.Setup(uow => uow.Users.GetRoleAsync(applicationUser)).ReturnsAsync(roles);
            _mockJwtTokenGenerator.Setup(generator => generator.GenerateToken(applicationUser, roles)).Returns(token);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(applicationUser)).Returns(userDto);

            // Act
            var result = await _authService.Login(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.User);
            Assert.Equal(token, result.Token);
        }

        [Fact]
        public async Task Login_WithInvalidEmailOrPassword_ShouldThrowsAuthenticationException()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
            _mockUnitOfWork.Setup(uow => uow.Users.GetUserByEmail(loginDto.Email)).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            var result = await Assert.ThrowsAsync<AuthenticationException>(() => _authService.Login(loginDto));
            Assert.Equal("Email hoặc mật khẩu không chính xác.", result.Message);

        }

        [Fact]
        public async Task Login_WithInvalidPassword_ShouldThrowsAuthenticationException()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
            var applicationUser = new ApplicationUser
            {
                Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                UserName = loginDto.Email.ToLower(),
                Email = loginDto.Email,
                NormalizedEmail = loginDto.Email.ToUpper(),
                Name = "Thuy Trinh",
                PhoneNumber = "0928262522"
            };
            _mockUnitOfWork.Setup(uow => uow.Users.GetUserByEmail(loginDto.Email)).ReturnsAsync(applicationUser);
            _mockUnitOfWork.Setup(uow => uow.Users.Login(applicationUser, loginDto.Password)).ReturnsAsync(false);

            // Act & Assert
            var result = await Assert.ThrowsAsync<AuthenticationException>(() => _authService.Login(loginDto));
            Assert.Equal("Email hoặc mật khẩu không chính xác.", result.Message);

        }

        [Fact]
        public async Task Login_WithUserWithoutRoles_ThrowsAuthenticationException()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
            var applicationUser = new ApplicationUser
            {
                Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                UserName = loginDto.Email.ToLower(),
                Email = loginDto.Email,
                NormalizedEmail = loginDto.Email.ToUpper(),
                Name = "Thuy Trinh",
                PhoneNumber = "0928262522"
            };
            _mockUnitOfWork.Setup(uow => uow.Users.GetUserByEmail(loginDto.Email)).ReturnsAsync(applicationUser);
            _mockUnitOfWork.Setup(uow => uow.Users.Login(applicationUser, loginDto.Password)).ReturnsAsync(true);
            _mockUnitOfWork.Setup(uow => uow.Users.GetRoleAsync(applicationUser)).ReturnsAsync(new List<string>());


            // Act & Assert
           var result =  await Assert.ThrowsAsync<AuthenticationException>(() => _authService.Login(loginDto));
            Assert.Equal("Người dùng không có quyền truy cập.", result.Message);
        }
        #endregion

        //#region Test case for AssignRole
        //[Fact]
        //public async Task AssignRole_UserExistsAndRoleAssigned_ReturnsTrue()
        //{
        //    // Arrange
        //    var userId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
        //    var roleId = Guid.NewGuid();
        //    var applicationUser = new ApplicationUser
        //    {
        //        Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
        //        UserName = "username",
        //        Email = "username",
        //        NormalizedEmail = "username",
        //        Name = "Thuy Trinh",
        //        PhoneNumber = "0928262522"
        //    };
        //    mockGetUserById(applicationUser);
        //    mockAssignRoleForUser(true);

        //    // Act
        //    var result = await _authService.AssignRole(userId, roleId);

        //    // Assert
        //    Assert.True(result);
        //}

        //[Fact]
        //public async Task AssignRole_UserExistsAndRoleAssignmentFails_ReturnsFalse()
        //{
        //    // Arrange
        //    var userId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
        //    var roleId = Guid.NewGuid();
        //    var applicationUser = new ApplicationUser
        //    {
        //        Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
        //        UserName = "username",
        //        Email = "username",
        //        NormalizedEmail = "username",
        //        Name = "Thuy Trinh",
        //        PhoneNumber = "0928262522"
        //    };
        //    mockGetUserById(applicationUser);
        //    mockAssignRoleForUser(false);

        //    // Act
        //    var result = await _authService.AssignRole(userId, roleId);

        //    // Assert
        //    Assert.False(result);
        //}

        //[Fact]
        //public async Task AssignRole_UserDoesNotExist_ReturnsFalse()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var roleId = Guid.NewGuid();
        //    mockGetUserById((ApplicationUser)null);


        //    // Act
        //    var result = await _authService.AssignRole(userId, roleId);

        //    // Assert
        //    Assert.False(result);
        //}

        //[Fact]
        //public async Task AssignRole_WhenAssignRoleThrowException_ShouldThrowsException()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var roleId = Guid.NewGuid();
        //    var exceptionMessage = "Database connection failed";

        //    _mockUnitOfWork.Setup(u => u.Users.GetUserById(userId))
        //                   .Throws(new Exception(exceptionMessage));

        //    // Act & Assert
        //    var exception = await Assert.ThrowsAsync<Exception>(() => _authService.AssignRole(userId, roleId));
        //    Assert.Contains("An error occurred while assigning role:", exception.Message);
        //    Assert.Contains(exceptionMessage, exception.InnerException.Message);
        //}
        //#endregion

        #region Test case for SendingOTP
        [Fact]
        public async Task SendingOTP_ValidEmail_SendsOTPAndSavesVerification_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            _mockEmailSender.Setup(e => e.SendEmailAsync(email, It.IsAny<string>(), It.IsAny<string>()))
                            .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.EmailVerifications.Add(It.IsAny<EmailVerification>()));
            _mockUnitOfWork.Setup(u => u.Save()).Returns(1);

            // Act
            var result = await _authService.SendingOTP(email);

            // Assert
            Assert.True(result);
            _mockEmailSender.Verify(e => e.SendEmailAsync(email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.EmailVerifications.Add(It.IsAny<EmailVerification>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public async Task SendingOTP_EmailSendingFails_ThrowsException()
        {
            // Arrange
            var email = "test@example.com";
            _mockEmailSender.Setup(e => e.SendEmailAsync(email, It.IsAny<string>(), It.IsAny<string>()))
                            .ThrowsAsync(new Exception("SMTP server error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _authService.SendingOTP(email));
            Assert.Contains("An error occurred while sending OTP:", exception.Message);
        }

        [Fact]
        public async Task SendingOTP_SaveFails_ThrowsException()
        {
            // Arrange
            var email = "test@example.com";
            _mockEmailSender.Setup(e => e.SendEmailAsync(email, It.IsAny<string>(), It.IsAny<string>()))
                            .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.EmailVerifications.Add(It.IsAny<EmailVerification>()));
            _mockUnitOfWork.Setup(u => u.Save()).Throws(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _authService.SendingOTP(email));
            Assert.Contains("An error occurred while sending OTP:", exception.Message);
        }
        #endregion

        #region Test case for ForgotPassword
        [Fact]
        public async Task ForgotPassword_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var email = "test@example.com";
            _mockUnitOfWork.Setup(u => u.Users.GetUserByEmail(email))
                           .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _authService.ForgotPassword(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ForgotPassword_UserExistsButNoRoles_ThrowsAuthenticationException()
        {
            // Arrange
            var email = "test@example.com";
            var applicationUser = mockApplicationUser();
            _mockUnitOfWork.Setup(u => u.Users.GetUserByEmail(email))
                           .ReturnsAsync(applicationUser);
            _mockUnitOfWork.Setup(u => u.Users.GetRoleAsync(applicationUser))
                           .ReturnsAsync(new List<string>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _authService.ForgotPassword(email));
            Assert.Contains("Người dùng không có quyền truy cập.", exception.Message);
        }
        [Fact]
        public async Task ForgotPassword_EmailSendingFails_ThrowsException()
        {
            // Arrange
            var email = "test@example.com";
            var applicationUser = mockApplicationUser();
            var roles = new string[] { "User" };
            var resetToken = "resetToken";

            _mockUnitOfWork.Setup(u => u.Users.GetUserByEmail(email))
                           .ReturnsAsync(applicationUser);
            _mockUnitOfWork.Setup(u => u.Users.GetRoleAsync(applicationUser))
                           .ReturnsAsync(roles);
            _mockJwtTokenGenerator.Setup(j => j.GenerateToken(applicationUser, roles))
                                  .Returns(resetToken);
            _mockEmailSender.Setup(e => e.SendEmailAsync(email, "Reset Password", It.IsAny<string>()))
                            .ThrowsAsync(new Exception("SMTP error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _authService.ForgotPassword(email));
            Assert.Contains("An error occurred while processing ForgotPassword:", exception.Message);
        }

        [Fact]
        public async Task ForgotPassword_UserExistsAndHasRoles_SendsResetEmail_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var applicationUser = mockApplicationUser();
            var roles = new string[] { "User" };
            var resetToken = "resetToken";

            _mockUnitOfWork.Setup(u => u.Users.GetUserByEmail(email))
                           .ReturnsAsync(applicationUser);
            _mockUnitOfWork.Setup(u => u.Users.GetRoleAsync(applicationUser))
                           .ReturnsAsync(roles);
            _mockJwtTokenGenerator.Setup(j => j.GenerateToken(applicationUser, roles))
                                  .Returns(resetToken);

            _mockEmailSender.Setup(e => e.SendEmailAsync(email, "Reset Password", It.IsAny<string>()))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await _authService.ForgotPassword(email);

            // Assert
            Assert.True(result);
            _mockEmailSender.Verify(e => e.SendEmailAsync(email, "Reset Password", It.Is<string>(m => m.Contains(resetToken))), Times.Once);
        }

        #endregion

        #region  Test case for  ResetPassword
        [Fact]
        public async Task ResetPassword_ValidToken_Success()
        {
            // Arrange
            var resetToken = "validToken";
            var newPass = "newPassword";
            var user = mockApplicationUser();
            _mockJwtTokenGenerator.Setup(x => x.ValidateToken(resetToken)).Returns(user);
            var userFromDb = mockApplicationUser();
            mockGetUserById(userFromDb);
            _mockUnitOfWork.Setup(x => x.Users.ResetPassword(userFromDb, newPass))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.ResetPassword(resetToken, newPass);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ResetPassword_InvalidToken_Failure()
        {
            // Arrange
            var resetToken = "invalidToken";
            var newPass = "newPassword";

            _mockJwtTokenGenerator.Setup(x => x.ValidateToken(resetToken)).Returns((ApplicationUser)null);

            // Act
            var result = await _authService.ResetPassword(resetToken, newPass);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ResetPassword_UserNotFound_Failure()
        {
            // Arrange
            var resetToken = "validToken";
            var newPass = "newPassword";
            var user = mockApplicationUser();
            _mockJwtTokenGenerator.Setup(x => x.ValidateToken(resetToken)).Returns(user);
            _mockUnitOfWork.Setup(x => x.Users.GetUserById(user.Id)).Returns((ApplicationUser)null);

            // Act
            var result = await _authService.ResetPassword(resetToken, newPass);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ResetPassword_ResetFailed_Failure()
        {
            // Arrange
            var resetToken = "validToken";
            var newPass = "newPassword";
            var user = mockApplicationUser();
            _mockJwtTokenGenerator.Setup(x => x.ValidateToken(resetToken)).Returns(user);
            var userFromDb = mockApplicationUser();
            _mockUnitOfWork.Setup(x => x.Users.GetUserById(user.Id)).Returns(userFromDb);
            _mockUnitOfWork.Setup(x => x.Users.ResetPassword(userFromDb, newPass))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "ResetFailed", Description = "Reset password failed" }));

            // Act
            var result = await _authService.ResetPassword(resetToken, newPass);

            // Assert
            Assert.False(result);
        }
        #endregion 
        private ApplicationUser mockApplicationUser()
        {
            return new ApplicationUser
            {
                Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                UserName = "username",
                Email = "username",
                NormalizedEmail = "username",
                Name = "Thuy Trinh",
                PhoneNumber = "0928262522"
            };
        }
    }

}
