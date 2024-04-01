using NUnit.Framework;
using System.Drawing;
using WindowsFormsApp1Part2Gagan2;

namespace UnitTests
{
    [TestFixture]
    public class MainFormTests
    {
        private MainForm mainForm;

        [SetUp]
        public void Setup()
        {
            mainForm = new MainForm();
        }

        [Test]
        public void TestDrawRectangle_ValidInput()
        {
            // Arrange
            string[] parts = { "rectangle", "100", "200" };

            // Act
            mainForm.DrawRectangle(parts);

            // Assert
            // No direct way to assert graphics operations, consider adding assertion if feasible
        }

        [Test]
        public void TestDrawCircle_ValidInput()
        {
            // Arrange
            string[] parts = { "circle", "50" };

            // Act
            mainForm.DrawCircle(50);

            // Assert
            // No direct way to assert graphics operations, consider adding assertion if feasible
        }

        [Test]
        public void TestDrawEllipse_ValidInput()
        {
            // Arrange
            string[] parts = { "ellipse", "100", "50" };

            // Act
            mainForm.DrawEllipse(parts);

            // Assert
            // No direct way to assert graphics operations, consider adding assertion if feasible
        }

        [Test]
        public void TestDrawTriangle_ValidInput()
        {
            // Act
            mainForm.DrawTriangle();

            // Assert
            // No direct way to assert graphics operations, consider adding assertion if feasible
        }

        [Test]
        public void TestSetFillColor_ValidInput()
        {
            // Arrange
            string[] parts = { "fillcolor", "Red" };

            // Act
            mainForm.SetFillColor(parts);

            // Assert
            Assert.AreEqual(Color.Red, mainForm.pen.Color);
        }

        [Test]
        public void TestSetFillMode_On()
        {
            // Arrange
            string[] parts = { "fill", "on" };

            // Act
            mainForm.SetFillMode(parts);

            // Assert
            Assert.IsTrue(mainForm.fillShape);
        }

        [Test]
        public void TestSetFillMode_Off()
        {
            // Arrange
            string[] parts = { "fill", "off" };

            // Act
            mainForm.SetFillMode(parts);

            // Assert
            Assert.IsFalse(mainForm.fillShape);
        }

        [Test]
        public void TestDefineVariable_ValidInput()
        {
            // Act
            mainForm.DefineVariable(new string[] { "variable", "testVar", "10" });

            // Assert
            Assert.IsTrue(mainForm.variables.ContainsKey("testVar"));
            Assert.AreEqual(10, mainForm.variables["testVar"]);
        }

        [Test]
        public void TestDefineVariable_InvalidInput()
        {
            // Act
            mainForm.DefineVariable(new string[] { "variable", "testVar", "notAnInteger" });

            // Assert
            Assert.IsFalse(mainForm.variables.ContainsKey("testVar"));
        }

        [Test]
        public void TestEvaluateCondition_ValidInput()
        {
            // Arrange
            mainForm.variables.Add("testVar", 5);

            // Act
            bool result = mainForm.EvaluateCondition("testVar == 5");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TestEvaluateCondition_InvalidInput()
        {
            // Act
            bool result = mainForm.EvaluateCondition("testVar == 10");

            // Assert
            Assert.IsFalse(result);
        }
        [TestFixture]
        public class MainFormTests
        {
            private MainForm mainForm;

            [SetUp]
            public void Setup()
            {
                mainForm = new MainForm();
            }

            [Test]
            public void TestExecuteWhileLoop_ValidInput()
            {
                // Arrange
                mainForm.variables.Add("a", 0);
                string loopCommand = "variable a 0 while a < 100 a = a + 10 endwhile";

                // Act
                mainForm.ExecuteWhileLoop(loopCommand);

                // Assert
                Assert.IsTrue(mainForm.variables.ContainsKey("a"));
                Assert.AreEqual(100, mainForm.variables["a"]);
            }

            [Test]
            public void TestExecuteIfStatement_IfConditionTrue()
            {
                // Arrange
                mainForm.variables.Add("a", 5);
                string ifCommand = "if a == 5 draw 50 50";

                // Act
                mainForm.ExecuteIfStatement(ifCommand);

                // Assert
                // No direct way to assert graphics operations, consider adding assertion if feasible
            }

            [Test]
            public void TestExecuteIfStatement_IfConditionFalse()
            {
                // Arrange
                mainForm.variables.Add("a", 10);
                string ifCommand = "if a == 5 draw 50 50";

                // Act
                mainForm.ExecuteIfStatement(ifCommand);

                // Assert
                // No direct way to assert graphics operations, consider adding assertion if feasible
            }

            [Test]
            public void TestExecuteIfElseStatement_IfConditionTrue()
            {
                // Arrange
                mainForm.variables.Add("a", 5);
                string ifElseCommand = "if a == 5 draw 50 50 else draw 100 100 endif";

                // Act
                mainForm.ExecuteIfStatement(ifElseCommand);

                // Assert
                // No direct way to assert graphics operations, consider adding assertion if feasible
            }

            [Test]
            public void TestExecuteIfElseStatement_IfConditionFalse()
            {
                // Arrange
                mainForm.variables.Add("a", 10);
                string ifElseCommand = "if a == 5 draw 50 50 else draw 100 100 endif";

                // Act
                mainForm.ExecuteIfStatement(ifElseCommand);

                // Assert
                // No direct way to assert graphics operations, consider adding assertion if feasible
            }

            [Test]
            public void TestExecuteElseIfStatement_FirstConditionTrue()
            {
                // Arrange
                mainForm.variables.Add("a", 5);
                string elseIfCommand = "if a == 5 draw 50 50 elseif a == 10 draw 100 100 endif";

                // Act
                mainForm.ExecuteIfStatement(elseIfCommand);

                // Assert
                // No direct way to assert graphics operations, consider adding assertion if feasible
            }

            [Test]
            public void TestExecuteElseIfStatement_SecondConditionTrue()
            {
                // Arrange
                mainForm.variables.Add("a", 10);
                string elseIfCommand = "if a == 5 draw 50 50 elseif a == 10 draw 100 100 endif";

                // Act
                mainForm.ExecuteIfStatement(elseIfCommand);

                // Assert
                // No direct way to assert graphics operations, consider adding assertion if feasible
            }

            [Test]
            public void TestExecuteMethod_ValidInput()
            {
                // Arrange
                string methodCommand = "method method1 variable b 10 variable c 20 endmethod";

                // Act
                mainForm.ParseAndExecuteCommand(methodCommand, mainForm.graphics);

                // Assert
                Assert.IsTrue(mainForm.variables.ContainsKey("b"));
                Assert.IsTrue(mainForm.variables.ContainsKey("c"));
                Assert.AreEqual(10, mainForm.variables["b"]);
                Assert.AreEqual(20, mainForm.variables["c"]);
            }
        }
    }
}
