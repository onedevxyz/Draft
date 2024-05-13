using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Draft
{
    public partial class CustomMessageBoxForm : Form
    {
        private bool isDragging = false;
        private Point dragStartPoint;

        public string MessageText { get; set; }
        public string Title { get; set; }
        public DialogResult DialogResult { get; set; }
        public bool ShowYesNoButtons { get; set; } // Flag to determine button visibility

        public CustomMessageBoxForm()
        {
            InitializeComponent();

            ApplyRoundedCorners();

            // Set the form properties
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;

            // Calculate the position based on the cursor
            int formWidth = Width;
            int formHeight = Height;
            int cursorX = Cursor.Position.X;
            int cursorY = Cursor.Position.Y;

            // Adjust the position to center the form around the cursor
            int formX = cursorX - formWidth / 2;
            int formY = cursorY - formHeight / 2;

            Location = new Point(formX, formY);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Set the form properties
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;

            // Calculate the position based on the cursor
            int formWidth = Width;
            int formHeight = Height;
            int cursorX = Cursor.Position.X;
            int cursorY = Cursor.Position.Y;

            // Adjust the position to center the form around the cursor
            int formX = cursorX - formWidth / 2;
            int formY = cursorY - formHeight / 2;

            Location = new Point(formX, formY);


        }

        private void ApplyRoundedCorners()
        {
            const int CornerRadius = 10;

            Region = new Region(CreateRoundRectangle(0, 0, Width, Height, CornerRadius));
        }

        private GraphicsPath CreateRoundRectangle(int x, int y, int width, int height, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(x, y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            path.AddLine(x + cornerRadius, y, x + width - cornerRadius, y);
            path.AddArc(x + width - cornerRadius * 2, y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            path.AddLine(x + width, y + cornerRadius, x + width, y + height - cornerRadius);
            path.AddArc(x + width - cornerRadius * 2, y + height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            path.AddLine(x + width - cornerRadius, y + height, x + cornerRadius, y + height);
            path.AddArc(x, y + height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            path.AddLine(x, y + height - cornerRadius, x, y + cornerRadius);

            return path;
        }


        private void CustomMessageBoxForm_Load(object sender, EventArgs e)
        {
            lTitle.Text = Title;
            lMessage.Text = MessageText;

            // Set button visibility based on ShowYesNoButtons flag
            if (ShowYesNoButtons)
            {
                labelbuttonOK.Visible = false;
                labelbuttonYes.Visible = true;
                labelbuttonNo.Visible = true;
            }
            else
            {
                labelbuttonOK.Visible = true;
                labelbuttonYes.Visible = false;
                labelbuttonNo.Visible = false;
            }
        }

        private void pTop_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            dragStartPoint = new Point(e.X, e.Y);
        }

        private void pTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - dragStartPoint.X, p.Y - dragStartPoint.Y);
            }
        }

        private void pTop_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void labelbuttonNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void labelbuttonYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void labelbuttonYes_MouseEnter(object sender, EventArgs e)
        {
            labelbuttonYes.BackColor = Color.FromArgb(50, 50, 50);
        }

        private void labelbuttonNo_MouseEnter(object sender, EventArgs e)
        {
            labelbuttonNo.BackColor = Color.FromArgb(50, 50, 50);
        }

        private void labelbuttonNo_MouseLeave(object sender, EventArgs e)
        {
            labelbuttonNo.BackColor = Color.Transparent;
        }

        private void lTitle_TextChanged(object sender, EventArgs e)
        {
            labelbuttonNo.BackColor = Color.Transparent;
        }

        private void labelbuttonYes_MouseLeave(object sender, EventArgs e)
        {
            labelbuttonYes.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw rounded borders
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                int cornerRadius = 10;
                int width = ClientSize.Width;
                int height = ClientSize.Height;

                path.AddArc(0, 0, cornerRadius * 2, cornerRadius * 2, 180, 90);
                path.AddLine(cornerRadius, 0, width - cornerRadius, 0);
                path.AddArc(width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2, 270, 90);
                path.AddLine(width, cornerRadius, width, height - cornerRadius);
                path.AddArc(width - cornerRadius * 2, height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                path.AddLine(width - cornerRadius, height, cornerRadius, height);
                path.AddArc(0, height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                path.AddLine(0, height - cornerRadius, 0, cornerRadius);

                Region = new Region(path);
            }
        }

        private void pTop_MouseUp_1(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void btnCloseMessagebox_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void labelbuttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void labelbuttonOK_MouseEnter(object sender, EventArgs e)
        {
            labelbuttonOK.BackColor = Color.FromArgb(50, 50, 50);
        }

        private void labelbuttonOK_MouseLeave(object sender, EventArgs e)
        {
            labelbuttonOK.BackColor = Color.Transparent;
        }
    }
}
