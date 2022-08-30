using MC_DataHelper.Models;

namespace ModelTests;

public class ModConfigTest
{
    [Fact]
    public void ValidModName()
    {
        //Arrange
        var modConfig = new ModConfig();
        const string validModName = "testmod";

        //Act
        modConfig.Name = validModName;

        //Assert
        Assert.Equal(validModName, modConfig.Name);
    }

    [Fact]
    public void InvalidModName()
    {
        //Arrange
        var modConfig = new ModConfig();
        const string invalidModName = "Test Mod";

        //Act
        modConfig.Name = invalidModName;

        //Assert
        Assert.NotEqual(invalidModName, modConfig.Name);
    }

    [InlineData("testmod", "testmod")]
    [InlineData("TestMod", "testmod")]
    [InlineData("Test Mod", "test_mod")]
    [Theory]
    public void TestModNames(string inputName, string expectedName)
    {
        //Arrange
        var modConfig = new ModConfig();
        
        //Act
        modConfig.Name = inputName;

        //Assert
        Assert.Equal(expectedName, modConfig.Name);
    }
}