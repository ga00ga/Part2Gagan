using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1Part2Gagan2
{
    public partial class MainForm : Form
    {
        private List<string> programCommands = new List<string>();
        private Graphics graphics;
        private Pen pen = new Pen(Color.Black);
        private PointF currentPosition = PointF.Empty;
        private bool fillShape = false;
        private Dictionary<string, int> variables = new Dictionary<string, int>();

        public MainForm()
        {
            InitializeComponent();
            graphics = drawingPanel.CreateGraphics();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            ExecuteProgram();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            LoadProgramFromFile();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveProgramToFile();
        }



        private void ExecuteProgram()
        {
            ClearDrawingArea();
            currentPosition = PointF.Empty;

            foreach (string cmd in programCommands)
            {
                try
                {
                    textBox1.Text = cmd;
                    ParseAndExecuteCommand(cmd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Input string was in correct format", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ParseAndExecuteCommand(string command)
        {
            string[] parts = command.Split(' ');

            string keyword = parts[0].ToLower();
            switch (keyword)
            {
                case "position":
                    MoveTo(parts);
                    break;
                case "pen":
                    SetPenColor(parts);
                    break;
                case "draw":
                    DrawTo(parts);
                    break;
                case "rectangle":
                    DrawRectangle(parts);
                    break;
                case "circle":
                    DrawCircle(parts);
                    break;
                case "triangle":
                    DrawTriangle();
                    break;
                case "clear":
                    ClearDrawingArea();
                    break;
                case "reset":
                    ResetPenPosition();
                    break;
                case "fill":
                    SetFillMode(parts);
                    break;
                case "if":
                    ExecuteIfStatement(command);
                    break;
                case "loop":
                    ExecuteLoop(command);
                    break;
                case "endloop":
                    // End of loop, do nothing here, handled in ExecuteLoop method
                    break;
                case "method":
                    // Define method, not implemented in this example
                    break;
                case "endmethod":
                    // End of method, not implemented in this example
                    break;
                case "variable":
                    DefineVariable(parts);
                    break;
                case "square":
                    DrawSquare(parts);
                    break;
                case "linewidth":
                    SetLineWidth(parts);
                    break;
                case "ellipse":
                    DrawEllipse(parts);
                    break;
                case "fillcolor":
                    SetFillColor(parts);
                    break;
                default:
                    // Handle method calls or other custom functionalities
                    break;
            }
        }

        private void MoveTo(string[] parts)
        {
            float x, y;
            if (parts.Length >= 3 && float.TryParse(parts[1], out x) && float.TryParse(parts[2], out y))
            {
                currentPosition = new PointF(x, y);
            }
            else
            {
                // Handle invalid input command format
                MessageBox.Show("Invalid input command format for position.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetFillColor(string[] parts)
        {
            string colorName = parts[1];
            pen.Color = Color.FromName(colorName); // Assuming brush is a global variable
        }

        private void SetPenColor(string[] parts)
        {
            string colorName = parts[1];
            pen.Color = Color.FromName(colorName);
        }

        private void DrawTo(string[] parts)
        {
            float x = float.Parse(parts[1]);
            float y = float.Parse(parts[2]);

            // Move the current position to the specified coordinates
            MoveTo(new string[] { "position", x.ToString(), y.ToString() });
        }

        private void SetLineWidth(string[] parts)
        {
            int width = int.Parse(parts[1]);
            pen.Width = width;
        }

        private void DrawSquare(string[] parts)
        {
            int side = int.Parse(parts[1]);

            if (fillShape)
                graphics.FillRectangle(new SolidBrush(pen.Color), currentPosition.X, currentPosition.Y, side, side);
            else
                graphics.DrawRectangle(pen, currentPosition.X, currentPosition.Y, side, side);
        }

        private void DrawRectangle(string[] parts)
        {
            int width = int.Parse(parts[1]);
            int height = int.Parse(parts[2]);

            if (fillShape)
                graphics.FillRectangle(new SolidBrush(pen.Color), currentPosition.X, currentPosition.Y, width, height);
            else
                graphics.DrawRectangle(pen, currentPosition.X, currentPosition.Y, width, height);
        }

        private void DrawCircle(string[] parts)
        {
            int radius = int.Parse(parts[1]);

            if (fillShape)
                graphics.FillEllipse(new SolidBrush(pen.Color), currentPosition.X - radius, currentPosition.Y - radius, radius * 2, radius * 2);
            else
                graphics.DrawEllipse(pen, currentPosition.X - radius, currentPosition.Y - radius, radius * 2, radius * 2);
        }

        private void DrawEllipse(string[] parts)
        {
            int width = int.Parse(parts[1]);
            int height = int.Parse(parts[2]);

            if (fillShape)
                graphics.FillEllipse(new SolidBrush(pen.Color), currentPosition.X, currentPosition.Y, width, height);
            else
                graphics.DrawEllipse(pen, currentPosition.X, currentPosition.Y, width, height);
        }

        private void DrawTriangle()
        {
            // Implement draw triangle functionality
            PointF[] points = new PointF[]
            {
                new PointF(currentPosition.X, currentPosition.Y),
                new PointF(currentPosition.X + 50, currentPosition.Y + 100),
                new PointF(currentPosition.X - 50, currentPosition.Y + 100)
            };

            if (fillShape)
                graphics.FillPolygon(new SolidBrush(pen.Color), points);
            else
                graphics.DrawPolygon(pen, points);
        }

        private void ClearDrawingArea()
        {
            graphics.Clear(Color.White);
        }

        private void ResetPenPosition()
        {
            currentPosition = PointF.Empty;
        }

        private void SetFillMode(string[] parts)
        {
            fillShape = parts[1].ToLower() == "on";
        }

        private void SaveProgramToFile()
        {
            using (StreamWriter writer = new StreamWriter("program.txt"))
            {
                foreach (string cmd in programCommands)
                {
                    writer.WriteLine(cmd);
                }
            }

            MessageBox.Show("Program saved to program.txt", "Save Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void LoadProgramFromFile()
        {
            ClearDrawingArea();
            currentPosition = PointF.Empty;
            programCommands.Clear();

            await Task.Run(() =>
            {
                LoadProgram("program.txt", txtProgram);
                LoadProgram("program1.txt", txtProgram1);
            });

            MessageBox.Show("Programs loaded", "Load Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadProgram(string fileName, TextBox textBox)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    textBox.Invoke((MethodInvoker)delegate {
                        textBox.AppendText(line + Environment.NewLine);
                    });

                    programCommands.Add(line);
                }
            }
        }



        private void DefineVariable(string[] parts)
        {
            if (parts.Length != 3)
            {
                MessageBox.Show("Invalid variable definition.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string variableName = parts[1];
            int value;
            if (int.TryParse(parts[2], out value))
            {
                if (variables.ContainsKey(variableName))
                    variables[variableName] = value;
                else
                    variables.Add(variableName, value);
            }
            else
            {
                MessageBox.Show("Invalid value for variable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteIfStatement(string command)
        {
            // Extract condition from if statement
            string condition = command.Substring(command.IndexOf("if") + 2, command.IndexOf("endif") - command.IndexOf("if") - 2);

            // Evaluate condition
            if (EvaluateCondition(condition))
            {
                // Execute lines between if and endif
                int startIndex = programCommands.IndexOf(command) + 1;
                int endIndex = programCommands.IndexOf("endif");

                for (int i = startIndex; i < endIndex; i++)
                {
                    ParseAndExecuteCommand(programCommands[i]);
                }
            }
        }

        private bool EvaluateCondition(string condition)
        {
            // Evaluate the condition
            // For simplicity, assuming condition is in the form "<variable> <operator> <value>"
            string[] parts = condition.Split(' ');
            string variableName = parts[0];
            string comparisonOperator = parts[1];
            int value = int.Parse(parts[2]);

            if (!variables.ContainsKey(variableName))
            {
                MessageBox.Show($"Variable '{variableName}' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int variableValue = variables[variableName];

            switch (comparisonOperator)
            {
                case "==":
                    return variableValue == value;
                case "!=":
                    return variableValue != value;
                case ">":
                    return variableValue > value;
                case "<":
                    return variableValue < value;
                case ">=":
                    return variableValue >= value;
                case "<=":
                    return variableValue <= value;
                default:
                    MessageBox.Show("Invalid comparison operator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
            }
        }

        private void ExecuteLoop(string loopStartCommand)
        {
            // Extract condition from loop command
            string condition = loopStartCommand.Substring(loopStartCommand.IndexOf("loop") + 4);

            // Execute loop until condition is false
            while (EvaluateCondition(condition))
            {
                // Execute lines between loop and endloop
                int startIndex = programCommands.IndexOf(loopStartCommand) + 1;
                int endIndex = programCommands.IndexOf("endloop");

                for (int i = startIndex; i < endIndex; i++)
                {
                    ParseAndExecuteCommand(programCommands[i]);
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

