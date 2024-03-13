using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1Part2Gagan2;

namespace TestProjectShapedrawing
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
        public void TestMoveTo_ValidInput()
        {
            string[] parts = { "position", "10", "20" };
            mainForm.MoveTo(parts);
            Assert.AreEqual(new PointF(10, 20), mainForm.GetCurrentPosition());
        }

        [Test]
        public void TestMoveTo_InvalidInput()
        {
            string[] parts = { "position", "invalid", "20" };
            mainForm.MoveTo(parts);
            Assert.AreEqual(PointF.Empty, mainForm.GetCurrentPosition());
        }

        [Test]
        public void TestSetFillColor()
        {
            string[] parts = { "fillcolor", "Red" };
            mainForm.SetFillColor(parts);
            Assert.AreEqual(Color.Red, mainForm.pen.Color);
        }

        [Test]
        public void TestSetPenColor()
        {
            string[] parts = { "pen", "Blue" };
            mainForm.SetPenColor(parts);
            Assert.AreEqual(Color.Blue, mainForm.pen.Color);
        }

        [Test]
        public void TestDrawTo()
        {
            string[] parts = { "draw", "50", "60" };
            mainForm.MoveTo(new string[] { "position", "10", "20" });
            mainForm.DrawTo(parts);
            Assert.AreEqual(new PointF(50, 60), mainForm.GetCurrentPosition());
        }

        [Test]
        public void TestSetLineWidth()
        {
            string[] parts = { "linewidth", "3" };
            mainForm.SetLineWidth(parts);
            Assert.AreEqual(3, mainForm.pen.Width);
        }

        [Test]
        public void TestDrawSquare()
        {
            string[] parts = { "square", "50" };
            mainForm.DrawSquare(parts);
            // No need to assert since it's drawing operation without return value
        }

        [Test]
        public void TestDrawRectangle()
        {
            string[] parts = { "rectangle", "50", "60" };
            mainForm.DrawRectangle(parts);
            // No need to assert since it's drawing operation without return value
        }

        [Test]
        public void TestDrawCircle()
        {
            string[] parts = { "circle", "30" };
            mainForm.DrawCircle(parts);
            // No need to assert since it's drawing operation without return value
        }

        [Test]
        public void TestDrawEllipse()
        {
            string[] parts = { "ellipse", "50", "60" };
            mainForm.DrawEllipse(parts);
            // No need to assert since it's drawing operation without return value
        }

        [Test]
        public void TestDrawTriangle()
        {
            mainForm.DrawTriangle();
            // No need to assert since it's drawing operation without return value
        }

        [Test]
        public void TestClearDrawingArea()
        {
            // Test whether the method throws any exception
            Assert.DoesNotThrow(() => mainForm.ClearDrawingArea());
        }

        [Test]
        public void TestResetPenPosition()
        {
            mainForm.ResetPenPosition();
            Assert.AreEqual(PointF.Empty, mainForm.GetCurrentPosition());
        }

        [Test]
        public void TestSetFillMode_On()
        {
            string[] parts = { "fill", "on" };
            mainForm.SetFillMode(parts);
            Assert.IsTrue(mainForm.fillShape);
        }

        [Test]
        public void TestSetFillMode_Off()
        {
            string[] parts = { "fill", "off" };
            mainForm.SetFillMode(parts);
            Assert.IsFalse(mainForm.fillShape);
        }

        [Test]
        public void TestSaveProgramToFile()
        {
            // Test whether the method throws any exception
            Assert.DoesNotThrow(() => mainForm.SaveProgramToFile());
        }

        [Test]
        public async Task TestLoadProgramFromFile()
        {
            // Test whether the method throws any exception
            await Task.Run(() =>
            {
                Assert.DoesNotThrow(() => mainForm.LoadProgramFromFile());
            });
        }

        [Test]
        public void TestDefineVariable()
        {
            string[] parts = { "variable", "var1", "10" };
            mainForm.DefineVariable(parts);
            Assert.IsTrue(mainForm.variables.ContainsKey("var1"));
            Assert.AreEqual(10, mainForm.variables["var1"]);
        }

        [Test]
        public void TestExecuteIfStatement_TrueCondition()
        {
            mainForm.programCommands.Add("if var1 == 10");
            mainForm.programCommands.Add("position 10 20");
            mainForm.programCommands.Add("endif");
            mainForm.variables["var1"] = 10;
            mainForm.ExecuteIfStatement("if var1 == 10");
            Assert.AreEqual(new PointF(10, 20), mainForm.GetCurrentPosition());
        }

        [Test]
        public void TestExecuteIfStatement_FalseCondition()
        {
            mainForm.programCommands.Add("if var1 == 10");
            mainForm.programCommands.Add("position 10 20");
            mainForm.programCommands.Add("endif");
            mainForm.variables["var1"] = 20;
            mainForm.ExecuteIfStatement("if var1 == 10");
            Assert.AreEqual(PointF.Empty, mainForm.GetCurrentPosition());
        }

        [Test]
        public void TestEvaluateCondition()
        {
            mainForm.variables["var1"] = 10;
            bool result = mainForm.EvaluateCondition("var1 == 10");
            Assert.IsTrue(result);
        }

        [Test]
        public void TestExecuteLoop()
        {
            mainForm.programCommands.Add("loop var1 < 3");
            mainForm.programCommands.Add("position 10 20");
            mainForm.programCommands.Add("endloop");
            mainForm.variables["var1"] = 0;
            mainForm.ExecuteLoop("loop var1 < 3");
            Assert.AreEqual(new PointF(10, 20), mainForm.GetCurrentPosition());
        }
    }
}
