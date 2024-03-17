using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1Part2Gagan2;

namespace UnitTests
{
    [TestClass]
    public class MainFormTests
    {
        private MainForm mainForm;

        [TestInitialize]
        public void Setup()
        {
            mainForm = new MainForm();
        }

        [TestMethod]
        public void TestDrawRectangle_ValidInput()
        {
            // Arrange
            string[] parts = { "rectangle", "100", "200" };

            // Act
            mainForm.DrawRectangle(parts);

            // Assert
            // Check if rectangle drawn at correct position
            // No direct way to assert graphics operations, consider adding assertion if feasible
        }

        [TestMethod]
        public void TestDrawCircle_ValidInput()
        {
            // Arrange
            string[] parts = { "circle", "50" };

            // Act
            mainForm.DrawCircle(parts);

            // Assert
            // Check if circle drawn at correct position
            // No direct way to assert graphics operations, consider adding assertion if feasible
        }

        [TestMethod]
        public void TestDrawEllipse_ValidInput()
        {
            // Arrange
            string[] parts = { "ellipse", "100", "50" };

            // Act
            mainForm.DrawEllipse(parts);

            // Assert
            // Check if ellipse drawn at correct position
            // No direct way to assert graphics operations, consider adding assertion if feasible
        }

        [TestMethod]
        public void TestDrawTriangle_ValidInput()
        {
            // Arrange

            // Act
            mainForm.DrawTriangle();

            // Assert
            // Check if triangle drawn at correct position
            // No direct way to assert graphics operations, consider adding assertion if feasible
        }

        [TestMethod]
        public void TestSetFillColor_ValidInput()
        {
            // Arrange
            string[] parts = { "fillcolor", "Red" };

            // Act
            mainForm.SetFillColor(parts);

            // Assert
            Assert.AreEqual(Color.Red, mainForm.pen.Color);
        }

        [TestMethod]
        public void TestSetFillMode_On()
        {
            // Arrange
            string[] parts = { "fill", "on" };

            // Act
            mainForm.SetFillMode(parts);

            // Assert
            Assert.IsTrue(mainForm.fillShape);
        }

        [TestMethod]
        public void TestSetFillMode_Off()
        {
            // Arrange
            string[] parts = { "fill", "off" };

            // Act
            mainForm.SetFillMode(parts);

            // Assert
            Assert.IsFalse(mainForm.fillShape);
        }

        [TestMethod]
        public void TestDefineVariable_ValidInput()
        {
            // Arrange

            // Act
            mainForm.DefineVariable(new string[] { "variable", "testVar", "10" });

            // Assert
            Assert.IsTrue(mainForm.variables.ContainsKey("testVar"));
            Assert.AreEqual(10, mainForm.variables["testVar"]);
        }

        [TestMethod]
        public void TestDefineVariable_InvalidInput()
        {
            // Arrange

            // Act
            mainForm.DefineVariable(new string[] { "variable", "testVar", "notAnInteger" });

            // Assert
            Assert.IsFalse(mainForm.variables.ContainsKey("testVar"));
        }

        [TestMethod]
        public void TestEvaluateCondition_ValidInput()
        {
            // Arrange
            mainForm.variables.Add("testVar", 5);

            // Act
            bool result = mainForm.EvaluateCondition("testVar == 5");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestEvaluateCondition_InvalidInput()
        {
            // Arrange

            // Act
            bool result = mainForm.EvaluateCondition("testVar == 10");

            // Assert
            Assert.IsFalse(result);
        }
    }
}
