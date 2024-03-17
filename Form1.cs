using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1Part2Gagan2
{
    public partial class MainForm : Form
    {
        public List<string> programCommands = new List<string>();
        public Graphics graphics;
        public Pen pen = new Pen(Color.Black);
        public PointF currentPosition = PointF.Empty;
        public bool fillShape = false;
        public Dictionary<string, int> variables = new Dictionary<string, int>();

        public MainForm()
        {
            InitializeComponent();
            graphics = drawingPanel.CreateGraphics();
        }


        
        public void button1_Click_1(object sender, EventArgs e)
        {
            Task.Run(() => ExecuteProgram(txtProgram)); // Start a new task for Program window 1
            Task.Run(() => ExecuteProgram(txtProgram1));// Start a new task for Program window 2
            Task.Run(() => ExecuteProgram(textBox1));
        }

        public void button2_Click(object sender, EventArgs e)
        {
            LoadProgramFromFile();
        }

       
        public void button3_Click_1(object sender, EventArgs e)
        {
            SaveProgramToFile();
        }

    

        public async Task ExecuteProgram(TextBoxBase programTextBox)
        {
            await Task.Run(() =>
            {
                lock (graphics) // Lock the Graphics object to ensure thread safety
                {
                    currentPosition = PointF.Empty;

                    foreach (string cmd in programTextBox.Lines)
                    {
                        try
                        {
                            ParseAndExecuteCommand(cmd, graphics);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error executing command: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            });
        }


        public void ParseAndExecuteCommand(string command, Graphics graphics)
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
                
                    break;
                case "method":
                  
                    break;
                case "endmethod":
                    
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

        public void MoveTo(string[] parts)
        {
            float x, y;
            if (parts.Length >= 3 && float.TryParse(parts[1], out x) && float.TryParse(parts[2], out y))
            {
                currentPosition = new PointF(x, y);
            }
            else
            {
                throw new ArgumentException("Invalid input command format for position.");
            }
        }

        public PointF GetCurrentPosition()
        {
            return currentPosition;
        }

        public void SetFillColor(string[] parts)
        {
            string colorName = parts[1];
            pen.Color = Color.FromName(colorName); // Assuming brush is a global variable
        }

        public void SetPenColor(string[] parts)
        {
            string colorName = parts[1];
            pen.Color = Color.FromName(colorName);
        }

        public void DrawTo(string[] parts)
        {
            float x = float.Parse(parts[1]);
            float y = float.Parse(parts[2]);
            //currentPosition = new PointF(x, y);
            MoveTo(new string[] { "position", x.ToString(), y.ToString() });
        }

        public void SetLineWidth(string[] parts)
        {
            int width = int.Parse(parts[1]);
            pen.Width = width;
        }

        public void DrawSquare(string[] parts)
        {
            int side = int.Parse(parts[1]);

            if (fillShape)
                graphics.FillRectangle(new SolidBrush(pen.Color), currentPosition.X, currentPosition.Y, side, side);
            else
                graphics.DrawRectangle(pen, currentPosition.X, currentPosition.Y, side, side);
        }

        public void DrawRectangle(string[] parts)
        {
            int width = int.Parse(parts[1]);
            int height = int.Parse(parts[2]);

            MoveTo(parts); // Call MoveTo method before drawing the rectangle

            if (fillShape)
                graphics.FillRectangle(new SolidBrush(pen.Color), currentPosition.X, currentPosition.Y, width, height);
            else
                graphics.DrawRectangle(pen, currentPosition.X, currentPosition.Y, width, height);
        }

        public void DrawCircle(string[] parts)
        {
            int radius = int.Parse(parts[1]);

            MoveTo(parts); // Call MoveTo method before drawing the circle

            if (fillShape)
                graphics.FillEllipse(new SolidBrush(pen.Color), currentPosition.X - radius, currentPosition.Y - radius, radius * 2, radius * 2);
            else
                graphics.DrawEllipse(pen, currentPosition.X - radius, currentPosition.Y - radius, radius * 2, radius * 2);
        }

        public void DrawEllipse(string[] parts)
        {
            int width = int.Parse(parts[1]);
            int height = int.Parse(parts[2]);

            if (fillShape)
                graphics.FillEllipse(new SolidBrush(pen.Color), currentPosition.X, currentPosition.Y, width, height);
            else
                graphics.DrawEllipse(pen, currentPosition.X, currentPosition.Y, width, height);
        }

        public void DrawTriangle()
        {
            // Implement draw triangle functionality
            PointF[] points = new PointF[]
            {
        new PointF(currentPosition.X, currentPosition.Y),
        new PointF(currentPosition.X + 50, currentPosition.Y + 100),
        new PointF(currentPosition.X - 50, currentPosition.Y + 100)
            };

            // Calculate the bounding box of the triangle
            float minX = points.Min(p => p.X);
            float minY = points.Min(p => p.Y);
            float maxX = points.Max(p => p.X);
            float maxY = points.Max(p => p.Y);

            // Ensure that the triangle is fully visible within the drawing panel
            if (minX < 0 || minY < 0 || maxX > drawingPanel.Width || maxY > drawingPanel.Height)
            {
                // Adjust the triangle's position to fit within the drawing panel
                float offsetX = 0, offsetY = 0;
                if (minX < 0) offsetX = -minX;
                if (minY < 0) offsetY = -minY;
                if (maxX > drawingPanel.Width) offsetX = drawingPanel.Width - maxX;
                if (maxY > drawingPanel.Height) offsetY = drawingPanel.Height - maxY;

                // Adjust the positions of the triangle's vertices
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = new PointF(points[i].X + offsetX, points[i].Y + offsetY);
                }
            }

            // Move the current position to the first vertex of the triangle
            MoveTo(new string[] { "position", points[0].X.ToString(), points[0].Y.ToString() });

            // Draw the triangle
            if (fillShape)
                graphics.FillPolygon(new SolidBrush(pen.Color), points);
            else
                graphics.DrawPolygon(pen, points);
        }
        public void ClearDrawingArea()
        {
            graphics.Clear(Color.White);
        }

        public void ResetPenPosition()
        {
            currentPosition = PointF.Empty;
        }

        public void SetFillMode(string[] parts)
        {
            fillShape = parts[1].ToLower() == "on";
        }

        public void SaveProgramToFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (string cmd in txtProgram.Lines)
                    {
                        writer.WriteLine(cmd);
                    }
                }

                MessageBox.Show($"Program saved to {filePath}", "Save Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        public void LoadProgramFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                txtProgram.Lines = File.ReadAllLines(filePath);
                MessageBox.Show($"Program loaded from {filePath}", "Load Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

      

        public void DefineVariable(string[] parts)
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

        public void ExecuteIfStatement(string command)
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
                   // ParseAndExecuteCommand(programCommands[i, graphics]);
                }
            }
        }

        public bool EvaluateCondition(string condition)
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

        public void ExecuteLoop(string loopStartCommand)
        {
            // Extract condition from loop command
            string condition = loopStartCommand.Substring(loopStartCommand.IndexOf("loop") + 4);

            // Execute loop until condition is false
            while (EvaluateCondition(condition))
            {
                // Find the index of the loop start command
                int startIndex = programCommands.IndexOf(loopStartCommand);

                // Find the index of the endloop command
                int endIndex = programCommands.IndexOf("endloop");

                // If the endloop command is not found or it comes before the loop start command, break the loop
                if (endIndex == -1 || endIndex < startIndex)
                {
                    MessageBox.Show("Endloop command not found or misplaced.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

                // Execute lines between loop and endloop
                for (int i = startIndex + 1; i < endIndex; i++)
                {
                    try
                    {
                        ParseAndExecuteCommand(programCommands[i], graphics);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error executing command: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public Dictionary<string, Func<string[], bool>> syntaxRules = new Dictionary<string, Func<string[], bool>>
        {
            { "position", parts => parts.Length == 3 && float.TryParse(parts[1], out _) && float.TryParse(parts[2], out _) },
            { "pen", parts => parts.Length == 2 },
            { "draw", parts => parts.Length == 3 && float.TryParse(parts[1], out _) && float.TryParse(parts[2], out _) },
            { "rectangle", parts => parts.Length == 3 && int.TryParse(parts[1], out _) && int.TryParse(parts[2], out _) },
            { "circle", parts => parts.Length == 4 && int.TryParse(parts[1], out _) },
            { "triangle", parts => parts.Length == 4 && int.TryParse(parts[1], out _) },
            { "clear", parts => true }, // No parameters for clear
            { "reset", parts => true }, // No parameters for reset
            { "fill", parts => parts.Length == 2 && (parts[1].ToLower() == "on" || parts[1].ToLower() == "off") },
            { "if", parts => parts.Length >= 4 && parts.Contains("endif") }, // Simple check for if statement
            { "loop", parts => parts.Length >= 3 && parts.Contains("endloop") }, // Simple check for loop statement
            { "method", parts => false }, // Not implemented, always returns false
            { "endmethod", parts => false }, // Not implemented, always returns false
            { "variable", parts => parts.Length == 3 && int.TryParse(parts[2], out _) },
            { "square", parts => parts.Length == 2 && int.TryParse(parts[1], out _) },
            { "linewidth", parts => parts.Length == 2 && int.TryParse(parts[1], out _) },
            { "ellipse", parts => parts.Length == 3 && int.TryParse(parts[1], out _) && int.TryParse(parts[2], out _) },
            { "fillcolor", parts => parts.Length == 2 } // Assuming fillcolor command takes only one argument
        };

 

        public void SyntaxCheck()
        {
            // Perform syntax check for txtProgram
            if (!SyntaxCheckForInput(txtProgram, "Program"))
                return;

            // Perform syntax check for txtProgram1
            if (!SyntaxCheckForInput(txtProgram1, "Program 1"))
                return;

            // Perform syntax check for textBox1
            if (!SyntaxCheckForInput(textBox1, "TextBox 1"))
                return;

            MessageBox.Show("All commands passed syntax check.", "Syntax Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public bool SyntaxCheckForInput(TextBoxBase textBox, string inputName)
        {
            string[] allCommands = textBox.Lines;

            for (int i = 0; i < allCommands.Length; i++)
            {
                string[] parts = allCommands[i].Split(' ');
                string keyword = parts[0].ToLower();

                if (!syntaxRules.ContainsKey(keyword))
                {
                    MessageBox.Show($"Syntax error in {inputName} at line {i + 1}: Unknown command '{keyword}'", "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!syntaxRules[keyword](parts))
                {
                    MessageBox.Show($"Syntax error in {inputName} at line {i + 1}: Invalid syntax for command '{keyword}'", "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        public void MainForm_Load(object sender, EventArgs e)
        {

        }

        public void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public void button4_Click(object sender, EventArgs e)
        {
            SyntaxCheck();
        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

