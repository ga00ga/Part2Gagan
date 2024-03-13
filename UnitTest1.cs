using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp1Part2Gagan2;

namespace TestProjectPart2
{
    [TestClass]
    public class MainFormTests
    {
        [TestMethod]
        public void MoveTo_WhenValidCoordinatesProvided_ShouldUpdateCurrentPosition()
        {
            // Arrange
            var mainForm = new MainForm();
            string[] commandParts = { "position", "100", "200" };

            // Act
            mainForm.MoveTo(commandParts);

            // Assert
            Assert.AreEqual(mainForm.currentPosition.X, 100);
            Assert.AreEqual(mainForm.currentPosition.Y, 200);
        }
    }
}
