
using JewelryEC_Backend.Models.Roles.Entities;
using Microsoft.AspNetCore.Routing;

namespace JewelryEC_Backend.Tests.JewelryEC_Backend.Tests.Service
{
    public class RoleServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockIUnitOfWork;
        private readonly RoleService _roleService;
        public RoleServiceTests()
        {
            _mockIUnitOfWork = new Mock<IUnitOfWork>();
            _roleService = new RoleService(_mockIUnitOfWork.Object);
        }
        #region Test case for CreateRole
        [Fact]
        public void CreateRole_Success_ReturnsTrue()
        {
            // Arrange
            var role = new ApplicationRole();
            _mockIUnitOfWork.Setup(uow => uow.Roles.Add(role)).Verifiable();
            _mockIUnitOfWork.Setup(uow => uow.Save()).Verifiable();


            // Act
            var result = _roleService.CreateRole(role);

            // Assert
            Assert.True(result);
            _mockIUnitOfWork.VerifyAll();
        }

        [Fact]
        public void CreateRole_ErrorInAddingRole_ThrowsException()
        {
            // Arrange
            var role = new ApplicationRole();
            _mockIUnitOfWork.Setup(uow => uow.Roles.Add(role)).Throws(new Exception("Error adding role"));


            // Act & Assert
            Assert.Throws<Exception>(() => _roleService.CreateRole(role));
        }
        #endregion
        #region Test case for DeleteRole
        [Fact]
        public void DeleteRole_Success_ReturnsTrue()
        {
            // Arrange
            var role = new ApplicationRole();
            _mockIUnitOfWork.Setup(uow => uow.Roles.Remove(role)).Verifiable();
            _mockIUnitOfWork.Setup(uow => uow.Save()).Verifiable();


            // Act
            var result = _roleService.DeleteRole(role);

            // Assert
            Assert.True(result);
            _mockIUnitOfWork.VerifyAll();
        }

        [Fact]
        public void DeleteRole_ErrorInRemovingRole_ThrowsException()
        {
            // Arrange
            var role = new ApplicationRole();
            _mockIUnitOfWork.Setup(uow => uow.Roles.Remove(role)).Throws(new Exception("Error removing role"));


            // Act & Assert
            Assert.Throws<Exception>(() => _roleService.DeleteRole(role));
        }
        #endregion
        #region Test case for GetRoleById
        [Fact]
        public void GetRoleById_ValidId_ReturnsRole()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var expectedRole = new ApplicationRole { Id = roleId, Name = "Test Role" };

            _mockIUnitOfWork.Setup(uow => uow.Roles.GetById(roleId)).Returns(expectedRole);


            // Act
            var result = _roleService.GetRoleById(roleId);

            // Assert
            Assert.Equal(expectedRole, result);
        }

        [Fact]
        public void GetRoleById_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            _mockIUnitOfWork.Setup(uow => uow.Roles.GetById(invalidId)).Returns((ApplicationRole)null);

            // Act
            var result = _roleService.GetRoleById(invalidId);

            // Assert
            Assert.Null(result);
        }

        #endregion
        #region Test case for ListRoles

        [Fact]
        public void ListRoles_NotEmpty_ReturnsRoleList()
        {
            // Arrange
            var expectedRoles = new List<ApplicationRole>
        {
            new ApplicationRole { Id = Guid.NewGuid(), Name = "Role 1" },
            new ApplicationRole { Id = Guid.NewGuid(), Name = "Role 2" },
            new ApplicationRole { Id = Guid.NewGuid(), Name = "Role 3" }
        };

            _mockIUnitOfWork.Setup(uow => uow.Roles.GetAll()).Returns(expectedRoles);


            // Act
            var result = _roleService.ListRoles();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(expectedRoles, result);
        }

        [Fact]
        public void ListRoles_Empty_ReturnsEmptyList()
        {
            // Arrange
            _mockIUnitOfWork.Setup(uow => uow.Roles.GetAll()).Returns(new List<ApplicationRole>());

            // Act
            var result = _roleService.ListRoles();
            // Assert
            Assert.Empty(result);
        }
        #endregion
        #region Test case for UpdateRole
        [Fact]
        public void UpdateRole_ExistingRoleWithValidInfo_ReturnsTrueAndUpdatesRole()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var existingRole = new ApplicationRole { Id = roleId, Name = "Existing Role" };
            var updatedRole = new ApplicationRole { Id = roleId, Name = "Updated Role" };

            _mockIUnitOfWork.Setup(uow => uow.Roles.GetById(roleId)).Returns(existingRole);


            // Act
            var result = _roleService.UpdateRole(updatedRole);

            // Assert
            Assert.True(result);
            Assert.Equal(updatedRole.Name, existingRole.Name);
            _mockIUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }

        [Fact]
        public void UpdateRole_NonExistingRole_ReturnsFalse()
        {
            // Arrange
            var nonExistingRoleId = Guid.NewGuid();
            var updatedRole = new ApplicationRole { Id = nonExistingRoleId, Name = "Updated Role" };

            _mockIUnitOfWork.Setup(uow => uow.Roles.GetById(nonExistingRoleId)).Returns((ApplicationRole)null);


            // Act
            var result = _roleService.UpdateRole(updatedRole);

            // Assert
            Assert.False(result);
            _mockIUnitOfWork.Verify(uow => uow.Save(), Times.Never);
        }

        [Fact]
        public void UpdateRole_ExistingRoleWithEmptyName_ReturnsTrueAndDoesNotUpdateRole()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var existingRole = new ApplicationRole { Id = roleId, Name = "Existing Role" };
            var updatedRole = new ApplicationRole { Id = roleId, Name = "" };

            _mockIUnitOfWork.Setup(uow => uow.Roles.GetById(roleId)).Returns(existingRole);


            // Act
            var result = _roleService.UpdateRole(updatedRole);

            // Assert
            Assert.False(result);
            Assert.Equal(existingRole.Name, "Existing Role"); // Role name should remain unchanged
            _mockIUnitOfWork.Verify(uow => uow.Save(), Times.Never); // Save method should not be called
        }
    }
    #endregion
}

