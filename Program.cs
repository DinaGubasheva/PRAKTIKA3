using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CommandLineShapeEditor
{
    public partial class MainForm : Form
    {
        private Dictionary<string, Square> shapes = new Dictionary<string, Square>();
        private TextBox textBoxInputStream;
        private ListBox listBoxCommandHistory;
        private PictureBox pictureBoxCanvas;

        public MainForm()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Командная строка для управления квадратами";
            this.Size = new Size(1200, 800);
            this.BackColor = Color.White;
            SplitContainer splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 400,
                BackColor = Color.LightGray
            };
            this.Controls.Add(splitContainer);

            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };
            splitContainer.Panel1.Controls.Add(leftPanel);

            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10) 
            };

            splitContainer.Panel2.Controls.Add(rightPanel);
            Label inputLabel = new Label
            {
                Text = "Введите команду:",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black
            };
            leftPanel.Controls.Add(inputLabel);
            textBoxInputStream = new TextBox
            {
                Location = new Point(10, 40),
                Width = 380,
                Font = new Font("Arial", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            leftPanel.Controls.Add(textBoxInputStream);

            Label historyLabel = new Label
            {
                Text = "История команд:",
                Location = new Point(10, 80),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black
            };
            leftPanel.Controls.Add(historyLabel);

            listBoxCommandHistory = new ListBox
            {
                Location = new Point(10, 110),
                Width = 380,
                Height = 600,
                Font = new Font("Arial", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom
            };
            leftPanel.Controls.Add(listBoxCommandHistory);

            // Область для рисования
            pictureBoxCanvas = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            rightPanel.Controls.Add(pictureBoxCanvas);

            textBoxInputStream.KeyDown += TextBoxInputStream_KeyDown;
        }
        private void TextBoxInputStream_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string command = textBoxInputStream.Text.Trim();
                textBoxInputStream.Clear();

                try
                {
                    bool result = RPN.CalculateRPN(command, shapes, pictureBoxCanvas);
                    if (result)
                    {
                        listBoxCommandHistory.Items.Add($"[Успешно] {command}");
                    }
                    else
                    {
                        listBoxCommandHistory.Items.Add($"[Ошибка] Неверная команда: {command}");
                    }
                }
                catch (Exception ex)
                {
                    listBoxCommandHistory.Items.Add($"[Ошибка] {ex.Message}");
                }
            }
        }
    }
    public static class RPN
    {
        public static bool CalculateRPN(string expression, Dictionary<string, Square> shapes, PictureBox canvas)
        {
            string[] tokens = expression.Split(',');
            Stack<object> operands = new Stack<object>();

            foreach (string token in tokens)
            {
                if (int.TryParse(token, out int number))
                {
                    operands.Push(number);
                }
                else if (token == "S" || token == "M" || token == "D")
                {
                    return ApplyOperation(operands, token[0], shapes, canvas);
                }
                else
                {
                    operands.Push(token);
                }
            }

            return false;
        }

        private static bool ApplyOperation(Stack<object> operands, char operation, Dictionary<string, Square> shapes, PictureBox canvas)
        {
            switch (operation)
            {
                case 'S':
                    if (operands.Count == 4)
                    {
                        string name = operands.Pop().ToString();
                        int y = (int)operands.Pop();
                        int x = (int)operands.Pop();
                        int a = (int)operands.Pop();

                        if (!shapes.ContainsKey(name))
                        {
                            Square newSquare = new Square(x, y, a, name);

                            if (!newSquare.IsWithinBounds(canvas))
                            {
                                throw new Exception("Не так все легко!");
                            }

                            shapes[name] = newSquare;
                            shapes[name].Draw(canvas.CreateGraphics());
                            return true;
                        }
                        else
                        {
                            throw new Exception($"Квадрат с именем {name} уже существует.");
                        }
                    }
                    break;

                case 'M':
                    if (operands.Count == 3)
                    {
                        int dy = (int)operands.Pop();
                        int dx = (int)operands.Pop();
                        string name = operands.Pop().ToString();

                        if (shapes.ContainsKey(name))
                        {
                            bool moveSuccess = shapes[name].Move(dx, dy, canvas);
                            if (!moveSuccess)
                            {
                                throw new Exception("Александр Олегович, не сломайте же))");
                            }

                            canvas.Refresh();
                            foreach (var shape in shapes.Values)
                            {
                                shape.Draw(canvas.CreateGraphics());
                            }
                            return true;
                        }
                        else
                        {
                            throw new Exception($"Квадрат с именем {name} не найден.");
                        }
                    }
                    break;

                case 'D':
                    if (operands.Count == 1)
                    {
                        string name = operands.Pop().ToString();

                        if (shapes.ContainsKey(name))
                        {
                            shapes.Remove(name);
                            canvas.Refresh();
                            foreach (var shape in shapes.Values)
                            {
                                shape.Draw(canvas.CreateGraphics());
                            }
                            return true;
                        }
                        else
                        {
                            throw new Exception($"Квадрат с именем {name} не найден.");
                        }
                    }
                    break;
            }

            return false;
        }
    }
    public class Square
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Side { get; private set; }
        public string Name { get; private set; }

        public Square(int x, int y, int side, string name)
        {
            X = x;
            Y = y;
            Side = side;
            Name = name;
        }

        public void Draw(Graphics g)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                g.DrawRectangle(pen, X, Y, Side, Side);
            }
        }

        public bool Move(int dx, int dy, PictureBox canvas)
        {
            int newX = X + dx;
            int newY = Y + dy;

            if (newX < 0 || newY < 0 || newX + Side > canvas.Width || newY + Side > canvas.Height)
            {
                return false;
            }
            X = newX;
            Y = newY;
            return true;
        }
        public bool IsWithinBounds(PictureBox canvas)
        {
            return X >= 0 && Y >= 0 && X + Side <= canvas.Width && Y + Side <= canvas.Height;
        }
    }
    static class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}