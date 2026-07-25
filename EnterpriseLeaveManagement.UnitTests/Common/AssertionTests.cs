using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Common;

public class AssertionTests
{
    [Fact]
    public void Assert_Equal_Test()
    {
        // Arrange
        int expected = 10;

        // Act
        int actual = 10;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Assert_True_Test()
    {
        // Arrange
        bool isLoggedIn = true;

        // Assert
        Assert.True(isLoggedIn);
    }

    [Fact]
    public void Assert_False_Test()
    {
        // Arrange
        bool isDeleted = false;

        // Assert
        Assert.False(isDeleted);
    }

    [Fact]
    public void Assert_Null_Test()
    {
        // Arrange
        string? employee = null;

        // Assert
        Assert.Null(employee);
    }

    [Fact]
    public void Assert_NotNull_Test()
    {
        // Arrange
        string employee = "Gopi";

        // Assert
        Assert.NotNull(employee);
    }
}