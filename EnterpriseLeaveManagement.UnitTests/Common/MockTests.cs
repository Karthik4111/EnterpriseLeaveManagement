using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Common;

public interface IUserService
{
    string GetEmployeeName();
}

public class MockTests
{
    [Fact]
    public void Mock_Should_Return_Gopi()
    {
        // Arrange
        var mock = new Mock<IUserService>();

        mock.Setup(x => x.GetEmployeeName())
            .Returns("Gopi");

        // Act
        var employee = mock.Object.GetEmployeeName();

        // Assert
        Assert.Equal("Gopi", employee);
    }
}
