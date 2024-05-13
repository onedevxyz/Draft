using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.Win32; // For accessing Windows registry
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Label = System.Windows.Forms.Label;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using System.IO;
using System.Xml;

namespace Draft
{
    public partial class Form1 : Form
    {
        private List<HistoryEntry> historyEntries = new List<HistoryEntry>();
        private Point lastPoint;
        private Panel settingsPanel;
        private CheckBox chkCollapse;
        private CheckBox chkDarkMode;
        private System.Windows.Forms.Label placeholderLabel;
        private const int CornerRadius = 10; //for rounded corners
        private Timer timerTypingAnimation;

        private string textDraft;
        private Button btnResumeDraft;
        private Size originalSize;
        private System.Windows.Forms.Label tipLabel;
        private Timer tipTimer;


        public Form1()
        {

            InitializeComponent();
            InitializeDarkModeCheckBox();
            InitializeEmojiList();

            flowHistory.MouseEnter += flowHistory_MouseEnter;
            flowHistory.MouseLeave += flowHistory_MouseLeave;

            ShowInTaskbar = true;

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeDarkModeCheckBox();
            InitializeContextMenuStrip();

            //Resume Text
            string resumeText = Properties.Settings.Default.ResumeText;

            if (!string.IsNullOrEmpty(resumeText))
            {
                rtbText.Text = resumeText;
                Properties.Settings.Default.ResumeText = string.Empty; // Clear the ResumeText setting
                Properties.Settings.Default.Save();
            }

            // Timer
            timerTypingAnimation = new Timer();
            timerTypingAnimation.Interval = 1000; // 1 second interval
            timerTypingAnimation.Tick += TimerTypingAnimation_Tick;
            timerTypingAnimation.Start();

            this.TopMost = Properties.Settings.Default.TopMost;
            originalSize = this.Size;

            generatePlaceHolderLabel();

            LoadHistoryEntries();

            LoadSettingsControls();

            AddResumeDraftButton();

            CreatePaintEventHandlers();

            ApplyTabStopToButtons();

            //Other
            formCollapsed = false;
            random = new Random();

            //Focus
            rtbText.Focus();


        }



        protected override void OnLoad(EventArgs e) //for rounded corners
        {
            base.OnLoad(e);

            // Set the form's properties
            FormBorderStyle = FormBorderStyle.None;
            UpdateStyles();
            LoadDarkModeSetting();
            ApplyRoundedCorners();

        }

        private void InitializeDarkModeCheckBox()
        {
            chkDarkMode = new CheckBox
            {
                Text = "Dark Mode",
                Location = new Point(10, 70),
                Enabled = false,
                Width = 200 // Adjust the width as needed
            };

            chkDarkMode.CheckedChanged += ChkDarkMode_CheckedChanged;

        }

        private void ChkDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            SaveDarkModeSetting();
            if (chkDarkMode.Checked)
            {
                SetDarkMode(3);
                darkmode_ = true;
            }
            else
            {
                SetLightMode(3);
                darkmode_ = false;
            }
        }

        bool darkmode_;

        private void LoadDarkModeSetting()
        {
            chkDarkMode.Checked = Properties.Settings.Default.DarkMode;
            if (chkDarkMode.Checked)
            {
                SetDarkMode(3);
            }
            else
            {
                SetLightMode(3);
            }
        }

        private void SaveDarkModeSetting()
        {
            Properties.Settings.Default.DarkMode = chkDarkMode.Checked;
            Properties.Settings.Default.Save();
        }
        private void CreatePaintEventHandlers()
        {
            pHolder.Paint += (sender, e) =>
            {
                ApplyRoundedCorners(pHolder, 10); // Adjust the corner radius as desired
            };
            pHolder2.Paint += (sender, e) =>
            {
                ApplyRoundedCorners(pHolder2, 10); // Adjust the corner radius as desired
            };

            pSeperator.Paint += (sender, e) =>
            {
                ApplyRoundedCorners(pSeperator, 4); // Adjust the corner radius as desired
            };
        }
        private void ApplyRoundedCorners(Control control, int cornerRadius)
        {
            using (var path = new GraphicsPath())
            {
                path.AddArc(0, 0, cornerRadius * 2, cornerRadius * 2, 180, 90);
                path.AddLine(cornerRadius, 0, control.Width - cornerRadius, 0);
                path.AddArc(control.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2, 270, 90);
                path.AddLine(control.Width, cornerRadius, control.Width, control.Height - cornerRadius);
                path.AddArc(control.Width - cornerRadius * 2, control.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                path.AddLine(control.Width - cornerRadius, control.Height, cornerRadius, control.Height);
                path.AddArc(0, control.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                path.AddLine(0, control.Height - cornerRadius, 0, cornerRadius);

                control.Region = new Region(path);
            }
        }



        private void generatePlaceHolderLabel()
        {
            placeholderLabel = new System.Windows.Forms.Label();
            placeholderLabel.Text = "Start typing here...";
            placeholderLabel.ForeColor = Color.DarkGray;
            placeholderLabel.Font = rtbText.Font;
            placeholderLabel.Location = new Point(0, 0);
            placeholderLabel.AutoSize = true;

            placeholderLabel.Click += new EventHandler(placeholderLabel_Click);

            if (string.IsNullOrWhiteSpace(rtbText.Text))
            {
                placeholderLabel.Location = new Point(0, 0);
                rtbText.Controls.Add(placeholderLabel);
            }
        }

        private void placeholderLabel_Click(object sender, EventArgs e)
        {
            placeholderLabel.Location = new Point(0, 0);
            rtbText.Controls.Remove(placeholderLabel);
            rtbText.Focus();
        }

        private void rtbText_TextChanged(object sender, EventArgs e)
        {
            lCharacters.Text = $"{rtbText.TextLength}";

            if (string.IsNullOrWhiteSpace(rtbText.Text))
            {
                if (!rtbText.Controls.Contains(placeholderLabel))
                {
                    placeholderLabel.Location = new Point(0, 0);
                    rtbText.Controls.Add(placeholderLabel);
                }
            }
            else
            {
                rtbText.Controls.Remove(placeholderLabel);
            }

            //Limit snap text box indication
            const int characterInterval = 140;

            int dotCount = rtbText.Text.Length / characterInterval;

            if (dotCount > 0)
            {
                lDots.Text = new string('.', dotCount);
            }
            else
            {
                lDots.Text = "";
            }

        }

        private void rtbText_Click(object sender, EventArgs e)
        {
            rtbText.Controls.Remove(placeholderLabel);
            rtbText.Focus();
        }

        private void rtbText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Shift)
            {
                e.Handled = true; // Prevent the Enter key from creating a new line
                e.SuppressKeyPress = true; // Suppress the Enter key press event

                btnClearText_Click(sender, e); // Call the btnClearText_Click event handler
            }
        }

        private void btnCopyText_Click(object sender, EventArgs e)
        {
            try { Clipboard.SetText(rtbText.Text); ShowCopyIndicator("Text copied!"); } catch { }
        }

        private void rtbText_SelectionChanged(object sender, EventArgs e)
        {

        }

        public Color FindOffColor(Color color)
        {
            int darkerFactor = 30; // Adjust this value to control the darkness of the off color

            int newRed = Math.Max(0, color.R - darkerFactor);
            int newGreen = Math.Max(0, color.G - darkerFactor);
            int newBlue = Math.Max(0, color.B - darkerFactor);

            return Color.FromArgb(color.A, newRed, newGreen, newBlue);
        }

        void pTop_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        void pTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
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

        private void DisableOrEnableControlsInPanel(Panel panel, bool boolean)
        {
            foreach (Control control in panel.Controls)
            {
                if (control != btnSettings) // Skip disabling this button
                {
                    control.Enabled = boolean;
                }
            }
        }

        private bool isSettingsPanelDisplayed = false;
        bool isEmojiToggledVisible;
        private ToolTip toolTipSettings;
        private void btnSettings_Click(object sender, EventArgs e)
        {
            

            if (isSettingsPanelDisplayed)
            {
                rtbText.Parent.Controls.Remove(settingsPanel);
                settingsPanel.Dispose();
                isSettingsPanelDisplayed = false;

                if (isEmojiToggledVisible) { emojiPanel.Visible = true; }

                // Show rtbText control
                rtbText.Visible = true;


                btnSettings.FlatAppearance.BorderColor = Color.IndianRed;
                btnSettings.FlatAppearance.BorderSize = 0;


                // Remove the tip timer and event handler
                tipTimer.Stop();
                tipTimer.Tick -= TipTimer_Tick;
                tipTimer.Dispose();

                //Enable disabled controls
                DisableOrEnableControlsInPanel(pHolder, true);
            }
            else
            {


                isEmojiToggledVisible = emojiPanel.Visible;
                //Disable enabled controls
                DisableOrEnableControlsInPanel(pHolder, false);

                btnSettings.FlatAppearance.BorderColor = Color.IndianRed;
                btnSettings.FlatAppearance.BorderSize = 2;

                emojiPanel.Visible = false;

                // Create settings panel
                settingsPanel = new Panel()
                {
                    Parent = rtbText.Parent,
                    Location = rtbText.Location,
                    Size = rtbText.Size,
                    Font = new Font(this.Font.FontFamily, this.Font.Size - 2)
                };

                // Create "Topmost" checkbox
                CheckBox chkTopmost = new CheckBox()
                {
                    Text = "Topmost",
                    Location = new Point(10, 10),
                    Checked = Properties.Settings.Default.TopMost,
                    Width = 200 // Adjust the width as needed
                };
                chkTopmost.CheckedChanged += (s, args) =>
                {
                    Properties.Settings.Default.TopMost = chkTopmost.Checked;
                    Properties.Settings.Default.Save();
                    this.TopMost = chkTopmost.Checked;
                };
                settingsPanel.Controls.Add(chkTopmost);

                // Create "Collapse" checkbox
                /*
                chkCollapse = new CheckBox()
                {
                    Text = "Small form factor",
                    Location = new Point(10, 160),
                    Checked = Properties.Settings.Default.Collapse,
                    Width = 200 // Adjust the width as needed
                };
                chkCollapse.CheckedChanged += (s, args) =>
                {
                    Properties.Settings.Default.Collapse = chkCollapse.Checked;
                    Properties.Settings.Default.Save();
                    UpdateCollapseState();
                };
                settingsPanel.Controls.Add(chkCollapse);
                */

                // Create "Dark Mode" checkbox
                CheckBox chkDarkMode = new CheckBox()
                {
                    Text = "Dark Mode",
                    Location = new Point(10, 70),
                    Enabled = false,
                    Checked = Properties.Settings.Default.DarkMode,
                    Width = 200 // Adjust the width as needed
                };
                chkDarkMode.CheckedChanged += (s, args) =>
                {
                    Properties.Settings.Default.DarkMode = chkDarkMode.Checked;
                    Properties.Settings.Default.Save();
                    if (chkDarkMode.Checked)
                    {
                        SetDarkMode(3);

                    }
                    else
                    {
                        SetLightMode(3);

                    }
                };
                settingsPanel.Controls.Add(chkDarkMode);

                // Create "Start with Windows" checkbox
                CheckBox chkStartWithWindows = new CheckBox()
                {
                    Text = "Start with Windows",
                    Location = new Point(10, 100),
                    Checked = GetStartWithWindows(),
                    Width = 200 // Adjust the width as needed
                };
                chkStartWithWindows.CheckedChanged += (s, args) =>
                {
                    SetStartWithWindows(chkStartWithWindows.Checked);
                };
                settingsPanel.Controls.Add(chkStartWithWindows);

                // Create "Automatically copy text" checkbox
                CheckBox cbAutoCopy = new CheckBox()
                {
                    Text = "Automatically clipboard",
                    Location = new Point(10, 130),
                    Checked = Properties.Settings.Default.AutoCopy,
                    Width = 240 // Adjust the width as needed
                };
                cbAutoCopy.CheckedChanged += (s, args) =>
                {
                    Properties.Settings.Default.AutoCopy = cbAutoCopy.Checked;
                    Properties.Settings.Default.Save();
                };
                settingsPanel.Controls.Add(cbAutoCopy);

                // Create "Enable emoji" checkbox
                CheckBox chkEnableEmoji = new CheckBox()
                {
                    Text = "Enable emoji",
                    Location = new Point(10, 40),
                    Checked = Properties.Settings.Default.Emoji,
                    Width = 200 // Adjust the width as needed
                };
                chkEnableEmoji.CheckedChanged += (s, args) =>
                {
                    Properties.Settings.Default.Emoji = chkEnableEmoji.Checked;
                    Properties.Settings.Default.Save();

                    emojiPanel.Visible = false; 
                    isEmojiToggledVisible = emojiPanel.Visible;

                    btnShowEmojiPanel.Visible = chkEnableEmoji.Checked;
                };
                settingsPanel.Controls.Add(chkEnableEmoji);



                // Create "Close" button
                Button btnClose = new Button()
                {
                    Text = "Close",
                    Location = new Point(120, 160),
                    Width = 80, // Adjust the width as needed
                    Height = 30,
                    FlatStyle = FlatStyle.Flat
                };
                btnClose.Click += (s, args) =>
                {
                    btnSettings_Click(sender, e);
                };
                settingsPanel.Controls.Add(btnClose);

                // Create the tip label and timer
                tipTimer = new Timer();
                tipTimer.Interval = 5000; // Set the interval to 5000 milliseconds (5 seconds)
                tipTimer.Tick += TipTimer_Tick;
                tipTimer.Start();

                tipLabel = new System.Windows.Forms.Label()
                {
                    Text = "", // Initial empty text
                    Dock = DockStyle.Bottom, 
                    TextAlign = ContentAlignment.MiddleLeft, 
                    Font = new Font(this.Font.FontFamily, 10), 
                    ForeColor = Color.Gray, 
                    MaximumSize = new Size(settingsPanel.Width - 20, 0), 
                    AutoEllipsis = true, // Enable ellipsis (...) when the text is too long
                    AutoSize = true,
                    Padding = new Padding(5) 
                };
                settingsPanel.Controls.Add(tipLabel);
                TipTimer_Tick(this, EventArgs.Empty);// Manually trigger the Tick event to display the first tip instantly

                // Set tooltip messages for each checkbox
                toolTipSettings = new ToolTip();
                toolTipSettings.SetToolTip(chkTopmost, "Keep the application window on top of other windows.");
                //toolTipSettings.SetToolTip(chkCollapse, "Reduce the form size to save space.");
                toolTipSettings.SetToolTip(chkDarkMode, "Enable dark mode for the application.");
                toolTipSettings.SetToolTip(chkStartWithWindows, "Automatically start the application with Windows.");
                toolTipSettings.SetToolTip(cbAutoCopy, "Automatically copy text to clipboard after typing.");
                toolTipSettings.SetToolTip(chkEnableEmoji, "Enable emoji support for the application.");

                // Customize tooltip appearance
                toolTipSettings.OwnerDraw = true;
                toolTipSettings.Draw += ToolTip_Draw;
                toolTipSettings.BackColor = Color.FromArgb(32, 32, 32);
                toolTipSettings.ForeColor = Color.White;
                toolTipSettings.AutoPopDelay = 8000; // 8 seconds


                // Hide rtbText control
                rtbText.Visible = false;

                // Add settings panel to rtbText's parent
                rtbText.Parent.Controls.Add(settingsPanel);

                // Bring settings panel to front
                settingsPanel.BringToFront();
                isSettingsPanelDisplayed = true;
            }
        }

        private void ToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            // Customize the tooltip drawing
            e.DrawBackground();
            e.DrawBorder();

            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                // Calculate the size of the text
                SizeF textSize = e.Graphics.MeasureString(e.ToolTipText, e.Font, int.MaxValue, sf);

                // Set a maximum width for the tooltip (adjust this value as needed)
                int maxWidth = 300;
                int width = (int)Math.Min(textSize.Width + 20, maxWidth); // Add 20 for padding and ensure it doesn't exceed maxWidth
                int height = (int)textSize.Height + 20; // Add 20 for padding

                // Calculate the tooltip bounds
                RectangleF textBounds = new RectangleF(e.Bounds.Left + (e.Bounds.Width - width) / 2,
                                                       e.Bounds.Top + (e.Bounds.Height - height) / 2,
                                                       width,
                                                       height);

                // Draw the text
                e.Graphics.DrawString(e.ToolTipText, e.Font, Brushes.White, textBounds, sf);
            }
        }








        private List<string> emojiList; // Declare the emojiList as a class-level variable
        private List<Button> emojiButtons; // Declare a list to store the emoji buttons

        private void InitializeEmojiList()
        {
            // List of the 36 most popular emojis
            List<string> popularEmojis = new List<string>
    {
        "\U0001F600", "\U0001F603", "\U0001F604", "\U0001F601", "\U0001F606", "\U0001F605", "\U0001F923",
        "\U0001F602", "\U0001F642", "\U0001F643", "\U0001F609", "\U0001F60A", "\U0001F607", "\U0001F60D",
        "\U0001F929", "\U0001F618", "\U0001F617", "\U0001F61A", "\U0001F619", "\U0001F60B", "\U0001F61B",
        "\U0001F61C", "\U0001F92A", "\U0001F61D", "\U0001F911", "\U0001F917", "\U0001F92D", "\U0001F92B",
        "\U0001F914", "\U0001F910", "\U0001F60E", "\U0001F929", "\U0001F602", "\U0001F604", "\U0001F618"
    };

            // Initialize emojiList with the popular emojis
            emojiList = new List<string>(popularEmojis);
        }

        private bool isDraggingEmojiPanel = false;
        private bool isHoldingShuffleButton = false;
        private FlowLayoutPanel emojiPanel;
        private Button shuffleButton;
        private void addEmojiPanel()
        {
            // Create the flowLayoutPanel
            emojiPanel = new FlowLayoutPanel();
            emojiPanel.AutoSize = false; // Adjust the size automatically based on the contents
            emojiPanel.FlowDirection = FlowDirection.LeftToRight; // Arrange the buttons horizontally
            emojiPanel.BackColor = this.BackColor; // Set transparent background
            emojiPanel.Padding = new Padding(0, 0, 0, 0);
            emojiPanel.Margin = new Padding(0, 0, 0, 0);

            // Math the size
            emojiPanel.Height = 32;
            emojiPanel.Width = 28 * 10 + 3;
            // Add the flowLayoutPanel to the form
            this.Controls.Add(emojiPanel);

            emojiButtons = new List<Button>(); // Initialize the emojiButtons list

            // Add buttons to the flowLayoutPanel
            for (int i = 0; i < emojiList.Count; i++)
            {
                string emoji = emojiList[i];
                bool isShuffleButton = (i == 9); // Set the 10th button as the shuffle button
                AddEmojiButton(emoji, isShuffleButton);
            }

            // Adjust the position of the emoji panel below the rtbText control
            emojiPanel.Location = new Point(rtbText.Left + (rtbText.Width - emojiPanel.Width) / 2, rtbText.Bottom - 30);

            shuffleButton.Click += shuffleButton_Click;
        }

        private void UpdateEmojiPanelPosition()
        {
            emojiPanel.Location = new Point(rtbText.Left, rtbText.Bottom + 5); // 5 is a margin
        }


        private void AddEmojiButton(string emoji, bool isShuffleButton)
        {
            Button emojiButton = new Button();
            emojiButton.Size = new Size(28, 28); // Set the size to 28x28
            emojiButton.Font = new Font("Segoe UI Emoji", 12); // Increase the font size for larger emojis
            emojiButton.FlatStyle = FlatStyle.Flat; // Set the flat style
            emojiButton.FlatAppearance.BorderSize = 0; // Remove the border
            emojiButton.BackColor = this.BackColor; // Set the background color to transparent
            emojiButton.ForeColor = lFormTitle.ForeColor; // Set the text color
            emojiButton.Padding = new Padding(0, 0, 0, 0);
            emojiButton.Margin = new Padding(0, 0, 0, 0);
            emojiButton.TextAlign = ContentAlignment.TopCenter;
            emojiButton.FlatAppearance.MouseOverBackColor = btnClearText.FlatAppearance.MouseOverBackColor;
            emojiButton.FlatAppearance.MouseDownBackColor = btnClearText.FlatAppearance.MouseDownBackColor;

            if (isShuffleButton)
            {
                emojiButton.Text = "⟳"; // Set the shuffle button text to "⟳"
                shuffleButton = emojiButton; // Assign the shuffle button to the variable
            }
            else
            {
                emojiButton.Text = emoji; // Set the emoji text for other buttons
                emojiButton.Tag = emoji; // Set the emoji as the tag of the button
                emojiButton.MouseUp += emojiButton_MouseUp; // Add MouseUp event for other buttons
                emojiButtons.Add(emojiButton); // Add the emoji button to the list
            }

            emojiButton.MouseEnter += (s, args) =>
            {
                emojiButton.ForeColor = Color.FromArgb(255, 176, 46); // Set the foreground color to white on mouse enter
            };

            emojiButton.MouseLeave += (s, args) =>
            {
                emojiButton.ForeColor = lFormTitle.ForeColor; // Restore the original foreground color on mouse leave
            };

            emojiButton.MouseDown += emojiButton_MouseDown;
            emojiButton.MouseMove += emojiButton_MouseMove;

            emojiPanel.Controls.Add(emojiButton);
            emojiPanel.BringToFront();
        }


        private void ShuffleEmojiButtons()
        {
            var shuffledEmojis = emojiButtons.Select(b => b.Tag.ToString()).OrderBy(x => Guid.NewGuid()).ToList();
            for (int i = 0; i < emojiButtons.Count; i++)
            {
                emojiButtons[i].Tag = shuffledEmojis[i];
                emojiButtons[i].Text = shuffledEmojis[i];
            }
        }


        private void btnShowEmojiPanel_Click(object sender, EventArgs e)
        {
            UpdateEmojiPanelPosition();
            emojiPanel.Visible = !emojiPanel.Visible;

        }


        private Timer dragDelayTimer = new Timer();

        private void emojiButton_MouseDown(object sender, MouseEventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (clickedButton != shuffleButton && !isHoldingShuffleButton)
            {
                // Start the timer when the mouse is pressed down on an emoji button (not the shuffle button)
                dragDelayTimer.Interval = 400; // 0.4 second delay
                dragDelayTimer.Tick += DragDelayTimer_Tick;
                dragDelayTimer.Start();

                mouseOffset = e.Location;
            }
        }

        private bool isReadyForDrag = false;
        private void emojiButton_MouseMove(object sender, MouseEventArgs e)
        {
            // Check if the panel is ready to be dragged and if the mouse is moved during this time
            if (isDraggingEmojiPanel && !isHoldingShuffleButton)
            {
                Point newLocation = emojiPanel.PointToScreen(e.Location);
                newLocation.Offset(-mouseOffset.X, -mouseOffset.Y);
                emojiPanel.Location = emojiPanel.Parent.PointToClient(newLocation); // Corrected the location calculation

                hasMoved = true; // Set the flag when the panel is being dragged
            }
        }

        private void emojiButton_MouseUp(object sender, MouseEventArgs e)
        {
            dragDelayTimer.Stop(); // Stop the timer when the mouse is released, whether the delay is over or not.
            dragDelayTimer.Tick -= DragDelayTimer_Tick; // Unsubscribe from the Tick event

            if (isDraggingEmojiPanel)
            {
                // If the emoji panel was dragged and the delay is over, set the isDragging flag to false,
                // so the emoji button can be clicked to add emojis to the rtbText.
                isDraggingEmojiPanel = false;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                // If the emoji panel was not dragged, proceed to add the emoji to the rtbText.
                int selectionStart = rtbText.SelectionStart;
                string currentText = rtbText.Text;
                string selectedEmoji = ((Button)sender).Tag.ToString();

                // Insert the emoji at the current caret position
                rtbText.Text = currentText.Insert(selectionStart, selectedEmoji);

                // Move the caret to the end of the inserted emoji
                rtbText.SelectionStart = selectionStart + selectedEmoji.Length;
                rtbText.SelectionLength = 0;

                rtbText.Focus();
            }

            isHoldingShuffleButton = false; // Reset the shuffle button holding flag
        }

        private void DragDelayTimer_Tick(object sender, EventArgs e)
        {
            // When the timer tick event occurs (after 1 second), enable dragging
            isDraggingEmojiPanel = true;
            Cursor.Current = Cursors.SizeAll; // Change the cursor to the drag cursor symbol
        }

        private void shuffleButton_Click(object sender, EventArgs e)
        {
            isHoldingShuffleButton = true;
            ShuffleEmojiButtons();
            isHoldingShuffleButton = false;
        }

        private void UpdateCollapseState()
        {
            if (chkCollapse != null && chkCollapse.Checked)
            {
                // Set the collapsed size of the form
                Size = new Size(originalSize.Width - 100, originalSize.Height);
            }
            else
            {
                // Restore the original size of the form
                Size = originalSize;
            }
        }


        private bool GetStartWithWindows()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            return (rk.GetValue("MyApp") != null);
        }

        private void SetStartWithWindows(bool startWithWindows)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (startWithWindows)
            {
                rk.SetValue("MyApp", Application.ExecutablePath);
            }
            else
            {
                rk.DeleteValue("MyApp", false);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void btnStory_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }







        private void SetDarkMode(int selectedTheme)
        {
            // Dark mode color definitions
            Color[] darkColors = new Color[]
            {
        Color.FromArgb(27, 32, 40),
        Color.FromArgb(20, 24, 30),
        Color.FromArgb(33, 37, 43),
            };

            Color darkColor = darkColors[selectedTheme % darkColors.Length];
            Color lightColor = Color.WhiteSmoke;

            var controlColors = new Dictionary<Control, (Color BackColor, Color ForeColor)>
    {
        // Define the control-color mappings here
        { this, (darkColor, lightColor) },
        { lCharacters, (Color.Transparent, Color.Gray) },
        { lHistoryCount, (Color.Transparent, Color.Gray) },
        { rtbText, (darkColor, lightColor) },
        { flowHistory, (darkColor, lightColor) },
        { pTop, (darkColor, lightColor) },
        { pHolder, (Color.FromArgb(38, 41, 46), lightColor) },
        { pHolder2, (Color.FromArgb(38, 41, 46), lightColor) },
        { pSplit, (Color.FromArgb(34, 37, 43), lightColor) },
        { pSeperator, (Color.FromArgb(25, 32, 35), lightColor) },
        { placeholderLabel, (darkColor, lightColor) },
        { lFormTitle, (darkColor, lightColor) }
    };


            rtbText.ForeColor = lightColor;

            foreach (var kvp in controlColors)
            {
                kvp.Key.BackColor = kvp.Value.BackColor;
                kvp.Key.ForeColor = kvp.Value.ForeColor;
            }

            // Update the colors of dynamically added buttons
            foreach (Control control in flowHistory.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor = darkColor;
                    button.ForeColor = lightColor;
                    button.FlatAppearance.MouseOverBackColor = darkColor;
                    button.FlatAppearance.MouseDownBackColor = darkColor;
                }
            }
        }


        private void SetLightMode(int selectedTheme)
        {
            // Light mode color definitions
            Color[] lightColors = new Color[]
            {
        Color.FromArgb(166, 172, 180),
        Color.FromArgb(216, 222, 231),
        Color.FromArgb(191, 196, 204)
            };

            Color lightColor = lightColors[selectedTheme % lightColors.Length];
            Color darkColor = Color.FromArgb(30, 30, 30);
            Color lightBackcolor = Color.FromArgb(235, 235, 235); // More white backcolor
            Color fontColor = Color.FromArgb(60, 60, 60); // Darker font color

            var controlColors = new Dictionary<Control, (Color BackColor, Color ForeColor)>
    {
        // Define the control-color mappings here
        { this, (lightBackcolor, darkColor) },
        { lCharacters, (Color.Transparent, Color.Gray) },
        { lHistoryCount, (Color.Transparent, Color.Gray) },
        { rtbText, (lightBackcolor, fontColor) },
        { flowHistory, (lightBackcolor, fontColor) },
        { pTop, (lightBackcolor, fontColor) },
        { pHolder, (lightBackcolor, fontColor) },
        { pHolder2, (lightBackcolor, fontColor) },
        { placeholderLabel, (lightBackcolor, fontColor) },
        { lFormTitle, (lightBackcolor, Color.Black) } // Updated font color to black
        // Add other controls as needed
    };


            foreach (var kvp in controlColors)
            {
                kvp.Key.BackColor = kvp.Value.BackColor;
                kvp.Key.ForeColor = kvp.Value.ForeColor;
            }

        }


        private int animationTick = 0;
        private void TimerTypingAnimation_Tick(object sender, EventArgs e)
        {
            animationTick++;

            int dotsToShow = animationTick % 4; // Show 1, 2, 3, or 0 dots based on the tick count

            string placeholderText = "Start typing here";

            // Add the dots to the placeholder text
            placeholderText += new string('.', dotsToShow);

            // Update the placeholder label text
            placeholderLabel.Text = placeholderText;
        }

        private void ShowCopyIndicator(string text)
        {
            ToolTip copyToolTip = new ToolTip();
            // Customize tooltip appearance
            copyToolTip.OwnerDraw = true;
            copyToolTip.Draw += ToolTip_Draw;
            copyToolTip.BackColor = Color.FromArgb(32, 32, 32);
            copyToolTip.ForeColor = Color.White;
            copyToolTip.AutoPopDelay = 8000; // 8 seconds
            copyToolTip.Show(text, btnClearText, 0, -btnClearText.Height - 10, 1500);
        }



        ContextMenuStrip contextMenuStrip1;
        ToolStripMenuItem pinUnpinMenuItem;
        ToolStripMenuItem deleteMenuItem;
        private void historyButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            HistoryEntry entry = (HistoryEntry)button.Tag;

            // Load the RTF content into rtbText
            rtbText.Rtf = entry.RtfContent;

            rtbText.Focus();
        }




        private void btnClearText_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.AutoCopy == true)
            {
                try
                {
                    Clipboard.SetText(rtbText.Text);
                    ShowCopyIndicator("Text copied!");
                }
                catch { }
            }

            if (rtbText.TextLength >= 1)
            {
                string text = rtbText.Text.Replace("|", "-"); // Replace "|" with "-"

                // Check if the text already exists in the history
                if (HistoryEntries.Any(entry => entry.Text == text))
                {
                    // Don't create a new entry if the text is the same
                    ShowCopyIndicator("That has already been added!");
                    rtbText.Focus();
                    return;
                }

                HistoryEntry newItem = new HistoryEntry()
                {
                    RtfContent = rtbText.Rtf,
                    Text = text,
                    IsPinned = false,
                    Timestamp = DateTime.Now  // Set the current time as the timestamp
                };


                // Insert the new item at the beginning of the list
                HistoryEntries.Insert(0, newItem);

                CreateButton(newItem);
                SaveHistoryEntries();
                rtbText.Clear();
            }

            lHistoryCount.Text = HistoryEntries.Count.ToString();
            rtbText.Focus();
        }



        private void ApplyTabStopToButtons() 
        {
            // This will ensure that the TabStop property is set to false for all the buttons on the form, including custom ImageButton instances.
            foreach (Control control in Controls)
            {
                if (control is Button || control is ImageButton)
                {
                    control.TabStop = false;
                }
            }
        }

        private void AddResumeDraftButton()
        {
            // Create "Resume Draft" button
            btnResumeDraft = new Button()
            {
                Parent = pHolder,
                Text = "Resume Draft ✖",
                Visible = false,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Padding = new Padding(0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.IndianRed,
                AutoSize = true,
                Margin = new Padding(0)
            };
            btnResumeDraft.FlatAppearance.BorderSize = 0;
            btnResumeDraft.FlatAppearance.MouseOverBackColor = btnClearText.FlatAppearance.MouseOverBackColor;
            btnResumeDraft.FlatAppearance.MouseDownBackColor = btnClearText.FlatAppearance.MouseDownBackColor;

            // Set the click event to load the draft text into the rtbText control
            btnResumeDraft.Click += (s, args) =>
            {
                rtbText.Text = textDraft;
                textDraft = string.Empty;
                btnResumeDraft.Visible = false;
            };

            // Add MouseEnter event handler
            btnResumeDraft.MouseEnter += (sender, e) =>
            {
                btnResumeDraft.ForeColor = Color.LightCoral; // Change the text color when mouse enters
            };

            // Add MouseLeave event handler
            btnResumeDraft.MouseLeave += (sender, e) =>
            {
                btnResumeDraft.ForeColor = Color.IndianRed; // Change the text color back when mouse leaves
            };

            // Update the button position to the right of btnShowEmojiPanel
            int buttonX = btnShowEmojiPanel.Right + 2; // Adjust the horizontal position
            int buttonY = btnShowEmojiPanel.Top + (btnShowEmojiPanel.Height - btnResumeDraft.Height) / 2; // Center vertically
            btnResumeDraft.Location = new Point(buttonX, buttonY);

            // Make sure the button is displayed on top of other controls
            btnResumeDraft.BringToFront();
        }

        private void LoadSettingsControls()
        {
            ///Emoji Panel  
            addEmojiPanel(); // create the emojiPanel but keep it hidden
            emojiPanel.Visible = false;
            emojiPanel.Region = new Region(CreateRoundRectangle(
                emojiPanel.ClientRectangle.X,
                emojiPanel.ClientRectangle.Y,
                emojiPanel.Width,
                emojiPanel.Height,
                CornerRadius));
            btnShowEmojiPanel.Visible = Properties.Settings.Default.Emoji;

        }


        public class HistoryEntry
        {
            public string RtfContent { get; set; }
            public string Text { get; set; }
            public bool IsPinned { get; set; }
            public DateTime Timestamp { get; set; }  
        }


        List<HistoryEntry> HistoryEntries = new List<HistoryEntry>();

        private string GetButtonLabelText(int index, bool isPinned)
        {
            string buttonText = isPinned ? "📌 " : ""; // Add the pin symbol if the button is pinned

            if (index < HistoryEntries.Count)
            {
                string historyText = HistoryEntries[index].Text;

                // Remove the new line character
                historyText = historyText.Replace("\u000A", "..");

                int maxLength = 10;

                if (historyText.Length > maxLength)
                {
                    buttonText += historyText.Substring(0, maxLength) + "...";
                }
                else
                {
                    buttonText += historyText;
                }
            }
            else
            {
                buttonText += $"Text {index + 1}";
            }

            return buttonText;
        }




        private Button CreateButton(HistoryEntry historyEntry)
        {

            DrawingControl.SuspendDrawing(flowHistory);

            Button newButton = new Button();

            newButton.TabStop = true;
            newButton.ContextMenuStrip = contextMenuStrip1;

            // Set the button as the tag for the context menu items
            contextMenuStrip1.Items[0].Tag = newButton; // Pin/Unpin menu item
            contextMenuStrip1.Items[1].Tag = newButton; // Delete menu item

            // Update the button the context menu is associated with when it's right-clicked
            newButton.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Tag = s;
                    contextMenuStrip1.Items[0].Tag = s; // Update the Pin/Unpin menu item tag
                    contextMenuStrip1.Items[1].Tag = s; // Update the Delete menu item tag

                    // Update the Pin/Unpin menu item text based on the pin state of the button
                    HistoryEntry entry = (HistoryEntry)((Button)s).Tag;
                    ToolStripMenuItem pinUnpinMenuItem = (ToolStripMenuItem)contextMenuStrip1.Items[0];
                    pinUnpinMenuItem.Text = entry.IsPinned ? "Unpin" : "Pin";
                }
            };

            newButton.Click += historyButton_Click;
            newButton.Text = GetButtonLabelText(HistoryEntries.IndexOf(historyEntry), historyEntry.IsPinned);

            newButton.Tag = historyEntry;

            newButton.AutoSize = true;
            newButton.FlatAppearance.MouseOverBackColor = btnClearText.FlatAppearance.MouseOverBackColor;
            newButton.FlatAppearance.MouseDownBackColor = btnClearText.FlatAppearance.MouseDownBackColor;
            newButton.FlatStyle = FlatStyle.Flat;
            newButton.FlatAppearance.BorderSize = 0;
            newButton.BackColor = flowHistory.BackColor;
            newButton.ForeColor = btnClearText.ForeColor;
            newButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            newButton.Anchor = AnchorStyles.Left | AnchorStyles.Right; // Set the anchor property
            newButton.TextAlign = ContentAlignment.MiddleLeft;
            newButton.Width = flowHistory.Width;




            int totalMargin = newButton.Margin.Top + newButton.Margin.Bottom;
            newButton.Height = (flowHistory.Height - totalMargin) / 10;

            newButton.Margin = new Padding(0, 0, 0, 2);

            // Adjust the button index based on the pin state
            int insertIndex = 0; // Index to insert the button

            if (historyEntry.IsPinned)
            {
                // Find the index of the first non-pinned button
                for (int i = 0; i < flowHistory.Controls.Count; i++)
                {
                    Button button = flowHistory.Controls[i] as Button;
                    if (button != null && !((HistoryEntry)button.Tag).IsPinned)
                    {
                        insertIndex = i;
                        break;
                    }
                }
            }
            else
            {
                // Find the index of the last pinned button
                for (int i = 0; i < flowHistory.Controls.Count; i++)
                {
                    Button button = flowHistory.Controls[i] as Button;
                    if (button != null && ((HistoryEntry)button.Tag).IsPinned)
                    {
                        insertIndex = i + 1; // Insert below the pinned button
                    }
                }
            }



                flowHistory.Controls.Add(newButton);
                flowHistory.Controls.SetChildIndex(newButton, insertIndex);


            // Limit the visible buttons to 10
            int delFromIndex = 10;
            for (int i = flowHistory.Controls.Count - 1; i >= delFromIndex; i--)
            {
                Control control = flowHistory.Controls[i];
                flowHistory.Controls.Remove(control);
                control.Dispose();
            }


            // Adjust the context menu text based on the pin state
            if (historyEntry.IsPinned)
            {
                pinUnpinMenuItem.Text = "Unpin";
            }
            else
            {
                pinUnpinMenuItem.Text = "Pin";
            }

            DrawingControl.ResumeDrawing(flowHistory);

            return newButton;

            
        }

        class DrawingControl //https://stackoverflow.com/questions/487661/how-do-i-suspend-painting-for-a-control-and-its-children
        {
            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

            private const int WM_SETREDRAW = 11;

            public static void SuspendDrawing(Control parent)
            {
                SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
            }

            public static void ResumeDrawing(Control parent)
            {
                SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
                parent.Refresh();
            }
        }

        private void TogglePinState(Button button)
        {

            HistoryEntry historyEntry = (HistoryEntry)button.Tag;

            // Check if the maximum limit of 9 pinned buttons has been reached
            int pinnedCount = HistoryEntries.Count(entry => entry.IsPinned);
            if (!historyEntry.IsPinned && pinnedCount >= 9)
            {
                CustomMessageBoxForm messageBox = new CustomMessageBoxForm();
                messageBox.Title = "Maximum Pins Reached";
                messageBox.MessageText = "You can only pin up to 9 items...";
                messageBox.ShowYesNoButtons = false; // Set to true for Yes/No buttons
                messageBox.ShowDialog();

                if (messageBox.DialogResult == DialogResult.OK)
                {
                    return;
                } else
                {
                    return;
                }
            }

            // Toggle the pin state
            historyEntry.IsPinned = !historyEntry.IsPinned;

            // Clear controls and re-add all buttons in the correct order
            flowHistory.Controls.Clear();
            foreach (HistoryEntry entry in HistoryEntries)
            {
                CreateButton(entry); // Recreate the button for the entry
            }
            SaveHistoryEntries(); // Save the change

        }

        private void pinUnpinMenuItem_Click(object sender, EventArgs e)
        {
            // Assume the Tag of the context menu is the Button it's associated with
            Button button = (Button)contextMenuStrip1.Tag;
            TogglePinState(button);

            // Update the context menu text based on the current pin state
            HistoryEntry historyEntry = (HistoryEntry)button.Tag;
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            menuItem.Text = historyEntry.IsPinned ? "Unpin" : "Pin";
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            // Get the Button from the Tag of the context menu
            Button button = (Button)contextMenuStrip1.Tag;

            // Check if the button belongs to the flowHistory panel
            if (button.Parent == flowHistory)
            {
                HistoryEntry entry = (HistoryEntry)button.Tag;
                HistoryEntries.Remove(entry);
                flowHistory.Controls.Remove(button);
            }
            else
            {
                Panel entryPanel = (Panel)button.Parent;
                HistoryEntry entry = (HistoryEntry)button.Tag;
                HistoryEntries.Remove(entry);
                entryPanel.Parent.Controls.Remove(entryPanel);
            }

            SaveHistoryEntries();  // Save the history entries
        }


        private void DeleteAllButtonsExceptPinned()
        {
            CustomMessageBoxForm messageBox = new CustomMessageBoxForm();
            messageBox.Title = "Are you sure?";
            messageBox.MessageText = "Are you sure you want to delete all non-pinned entries?";
            messageBox.ShowYesNoButtons = true; // Set to true for Yes/No buttons
            messageBox.ShowDialog();

            if (messageBox.DialogResult == DialogResult.Yes)
            {
                HistoryEntries.RemoveAll(item => !item.IsPinned);
                flowHistory.Controls.Clear();
                foreach (HistoryEntry item in HistoryEntries)
                {
                    CreateButton(item);
                }
                SaveHistoryEntries();  // Save the history entries
            }

            
        }

        private void SaveHistoryEntries()
        {
            // Save all items in HistoryEntries to settings
            Properties.Settings.Default.HistoryEntries = new System.Collections.Specialized.StringCollection();

            foreach (HistoryEntry item in HistoryEntries)
            {
                Properties.Settings.Default.HistoryEntries.Add(JsonConvert.SerializeObject(item));
            }
            lHistoryCount.Text = HistoryEntries.Count.ToString();
            Properties.Settings.Default.Save();
        }

        private void ListAllHistoryEntriesWithTimestamp()
        {
            foreach (HistoryEntry entry in HistoryEntries)
            {
                Console.WriteLine($"Text: {entry.Text}, Pinned: {entry.IsPinned}, Timestamp: {entry.Timestamp}, rtf: {entry.RtfContent}");
            }
        }

        private void LoadHistoryEntries()
        {
            HistoryEntries.Clear();

            // Check if HistoryEntries in settings is not null and has items
            if (Properties.Settings.Default.HistoryEntries != null)
            {
                foreach (string serializedItem in Properties.Settings.Default.HistoryEntries)
                {
                    HistoryEntry item = JsonConvert.DeserializeObject<HistoryEntry>(serializedItem);
                    if (item != null)
                    {
                        HistoryEntries.Add(item);
                    }
                }

                // Sort HistoryEntries by IsPinned and Timestamp
                HistoryEntries.Sort((x, y) =>
                {
                    int pinnedComparison = y.IsPinned.CompareTo(x.IsPinned);
                    if (pinnedComparison != 0) return pinnedComparison;
                    return x.Timestamp.CompareTo(y.Timestamp);
                });

                // After sorting, create buttons for them
                foreach (HistoryEntry item in HistoryEntries)
                {
                    Button button = CreateButton(item);
                    if (item.IsPinned)
                    {
                        flowHistory.Controls.Add(button);
                    }
                    else
                    {
                        // Add to a temporary list if you want to limit the number of unpinned items shown
                    }
                }
            }

            // Limit the visible buttons to 10
            while (flowHistory.Controls.Count > 10)
            {
                Control control = flowHistory.Controls[flowHistory.Controls.Count - 1];
                flowHistory.Controls.Remove(control);
                control.Dispose();
            }

            ListAllHistoryEntriesWithTimestamp();
            lHistoryCount.Text = HistoryEntries.Count.ToString();
        }



        private ToolStripMenuItem saveToFileMenuItem; // Declare it as a class-level variable
        private ToolStripMenuItem editMenuItem;

        private Button btnSaveChanges;
        private Button btnCancelChanges;

        private void InitializeContextMenuStrip()
        {
            // Create a context menu strip
            contextMenuStrip1 = new ContextMenuStrip();

            // Create a "Pin/Unpin" menu item
            pinUnpinMenuItem = new ToolStripMenuItem();
            pinUnpinMenuItem.Click += pinUnpinMenuItem_Click;

            // Create a "Delete" menu item
            deleteMenuItem = new ToolStripMenuItem("Delete");
            deleteMenuItem.Click += deleteMenuItem_Click;

            // Create a "Save to File" menu item
            saveToFileMenuItem = new ToolStripMenuItem("Save to File");
            saveToFileMenuItem.Click += saveToFileMenuItem_Click;

            // Add the items to the context menu strip
            contextMenuStrip1.Items.Add(pinUnpinMenuItem);
            contextMenuStrip1.Items.Add(deleteMenuItem);
            contextMenuStrip1.Items.Add(saveToFileMenuItem);


            // Add a separator
            contextMenuStrip1.Items.Add(new ToolStripSeparator());
            // Create a "Edit" menu item
            editMenuItem = new ToolStripMenuItem("Edit");
            editMenuItem.Click += editMenuItem_Click;

            // Add the "Edit" menu item to the context menu strip
            contextMenuStrip1.Items.Add(editMenuItem);

            // Create an instance of your custom renderer
            CustomContextMenuRenderer renderer = new CustomContextMenuRenderer();

            // Set the renderer for the context menu strip
            contextMenuStrip1.Renderer = renderer;
        }

        private void saveToFileMenuItem_Click(object sender, EventArgs e)
        {
            // Get the Button from the Tag of the context menu
            Button button = (Button)contextMenuStrip1.Tag;
            HistoryEntry entry = (HistoryEntry)button.Tag;

            // Create a save file dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            saveFileDialog.FileName = GenerateFileName(entry.Text); // Set the default file name

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                // Save the history entry text to the file
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.Write(entry.Text);
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exception that occurred during the file saving process
                    MessageBox.Show($"Error saving the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GenerateFileName(string entryText)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmm"); // Generate timestamp
            string sanitizedText = RemoveInvalidFileNameChars(entryText); // Sanitize the entry text

            // Limit the file name length to a maximum of 18 characters
            if (sanitizedText.Length > 18)
            {
                sanitizedText = sanitizedText.Substring(0, 18);
            }

            return $"{sanitizedText}_{timestamp}.txt";
        }


        private string RemoveInvalidFileNameChars(string fileName)
        {
            // Remove characters that are not allowed in file names
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (char invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar.ToString(), "");
            }
            return fileName;
        }

        private void btnDeleteHistory_Click(object sender, EventArgs e)
        {
            DeleteAllButtonsExceptPinned();

            
        }

        private void editMenuItem_Click(object sender, EventArgs e)
        {
            // Enable edit mode
            EditEntryMode(false);

            // Get the Button from the Tag of the context menu
            Button button = (Button)contextMenuStrip1.Tag;
            HistoryEntry entry = (HistoryEntry)button.Tag;

            // Set the entry text to the rtbText control
            rtbText.Rtf = entry.RtfContent;

            // Create the "Save Changes" button
            btnSaveChanges = new Button();
            btnSaveChanges.TextAlign = ContentAlignment.MiddleCenter;
            btnSaveChanges.FlatStyle = FlatStyle.Flat; // Set flat style
            btnSaveChanges.FlatAppearance.BorderSize = 0; // Remove border
            btnSaveChanges.Font = new Font("Segoe UI", 9.75F);
            btnSaveChanges.BackColor = Color.FromArgb(33, 37, 43);
            btnSaveChanges.ForeColor = Color.White; // Set text color
            btnSaveChanges.Text = "Save Changes";
            // Apply rounded corners to the btnDeleteOldEntries button
            btnSaveChanges.Region = new Region(CreateRoundRectangle(
                btnSaveChanges.ClientRectangle.X,
                btnSaveChanges.ClientRectangle.Y,
                btnSaveChanges.Width,
                btnSaveChanges.Height,
                CornerRadius));
            btnSaveChanges.Click += btnSaveChanges_Click;

            // Create the "Cancel" button
            btnCancelChanges = new Button();
            btnCancelChanges.TextAlign = ContentAlignment.MiddleCenter;
            btnCancelChanges.FlatStyle = FlatStyle.Flat; // Set flat style
            btnCancelChanges.FlatAppearance.BorderSize = 0; // Remove border
            btnCancelChanges.Font = new Font("Segoe UI", 9.75F);
            btnCancelChanges.BackColor = Color.FromArgb(33, 37, 43);
            btnCancelChanges.ForeColor = Color.White; // Set text color
            btnCancelChanges.Text = "Cancel";
            // Apply rounded corners to the btnDeleteOldEntries button
            btnCancelChanges.Region = new Region(CreateRoundRectangle(
                btnCancelChanges.ClientRectangle.X,
                btnCancelChanges.ClientRectangle.Y,
                btnCancelChanges.Width,
                btnCancelChanges.Height,
                CornerRadius));
            btnCancelChanges.Click += btnCancelChanges_Click;

            // Set the position and size of the buttons
            ///btnSaveChanges.Location = new Point(pHolder.Right - btnSaveChanges.Width - 20, pHolder.Bottom - 50);
            ///btnCancelChanges.Location = new Point(btnSaveChanges.Left, btnSaveChanges.Bottom + 10);
            ///

            // Set the anchor and location for btnSaveChanges
            btnSaveChanges.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveChanges.Location = new Point(this.ClientSize.Width - btnSaveChanges.Width - 20, this.ClientSize.Height - btnSaveChanges.Height - 20);

            // Set the anchor and location for btnCancelChanges
            btnCancelChanges.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancelChanges.Location = new Point(this.ClientSize.Width - btnCancelChanges.Width - 20, this.ClientSize.Height - btnCancelChanges.Height - 50);

            // Add the buttons to the form's controls
            this.Controls.Add(btnSaveChanges);
            this.Controls.Add(btnCancelChanges);

            btnSaveChanges.BringToFront();
            btnCancelChanges.BringToFront();
        }

        private void EditEntryMode(bool _enabled)
        { 

            if (!_enabled) //Go into edit mode
            {
                lHistoryCount.Visible = _enabled;
                lHistoryTitle.Visible = _enabled;
                splitContainer1.Panel2Collapsed = true;
                rtbText.Width = 479;
                pSeperator.Visible = _enabled;
                flowHistory.Visible = _enabled;
                pHolder.Visible = _enabled;
                pHolder2.Visible = _enabled;

                if (emojiPanel.Visible == true)
                {
                    emojiPanel.Location = new Point(25,407);
                }
            } 
            else //Go back to normal mode
            {
                lHistoryCount.Visible = _enabled;
                lHistoryTitle.Visible = _enabled;
                splitContainer1.Panel2Collapsed = false;
                rtbText.Width = 333;
                pSeperator.Visible = _enabled;
                flowHistory.Visible = _enabled;
                pHolder.Visible = _enabled;
                pHolder2.Visible = _enabled;

                if (emojiPanel.Visible == true)
                {
                    UpdateEmojiPanelPosition();
                }
            }
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            // Get the Button from the Tag of the context menu
            Button button = (Button)contextMenuStrip1.Tag;
            HistoryEntry entry = (HistoryEntry)button.Tag;

            string newRtfContent = rtbText.Rtf;


            // Check if an identical entry already exists
            if (HistoryEntries.Any(h => h.RtfContent == newRtfContent && h != entry))
            {
                // Display a message or handle the duplicate entry case as needed
                ///MessageBox.Show("An identical entry already exists.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            entry.RtfContent = newRtfContent;

            // Update the button text with the updated entry text
            button.Text = GetButtonLabelText(HistoryEntries.IndexOf(entry), entry.IsPinned);


            // Save the updated HistoryEntries
            SaveHistoryEntries();

            btnCancelChanges.Dispose();
            btnSaveChanges.Dispose();
            EditEntryMode(true);

        }

        private void btnCancelChanges_Click(object sender, EventArgs e)
        {
            btnCancelChanges.Dispose();
            btnSaveChanges.Dispose();
            // Disable edit mode
            EditEntryMode(true);
        }


        private void StoreDraft() //Not in prod yet
        {
            // Loop over all the buttons in the flowHistory panel
            foreach (Control c in flowHistory.Controls)
            {
                if (c is Button)
                {
                    Button button = (Button)c;

                    // If the text in rtbText matches the text of a button in flowHistory
                    if (rtbText.Text == button.Text)
                    {
                        // Do not store this text as a draft
                        return;
                    }
                }
            }

            // If no matching text was found in flowHistory, store the text as a draft
            // Only store the text as a draft if there isn't a draft already
            if (string.IsNullOrEmpty(textDraft))
            {
                textDraft = rtbText.Text;
            }

            // Only show the button if there is a draft to resume
            btnResumeDraft.Visible = !string.IsNullOrEmpty(textDraft);
        }

        bool formCollapsed;
        private Point mouseOffset;
        private bool isDragging = false;
        private bool hasMoved = false;

        private void lFormTitle_Click(object sender, EventArgs e)
        {
            if (!hasMoved)
            {
                if (!formCollapsed)
                {
                    originalSize = this.Size;
                    this.Size = new Size(100, 40);
                    formCollapsed = true;
                    pHolder.Visible = false;

                    lFormTitle.MouseDown += lFormTitle_MouseDown;
                    lFormTitle.MouseMove += lFormTitle_MouseMove;
                    lFormTitle.MouseUp += lFormTitle_MouseUp;
                }
                else
                {
                    this.Size = originalSize;
                    formCollapsed = false;
                    pHolder.Visible = true;

                    lFormTitle.MouseDown -= lFormTitle_MouseDown;
                    lFormTitle.MouseMove -= lFormTitle_MouseMove;
                    lFormTitle.MouseUp -= lFormTitle_MouseUp;
                }
            }

            lFormTitle.BackColor = Color.FromArgb(25, 33, 36);
            hasMoved = false; // Reset the flag after each click
        }

        private void lFormTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (formCollapsed)
            {
                isDragging = true;
                mouseOffset = e.Location;
            }
        }

        private void lFormTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newLocation = this.PointToScreen(e.Location);
                newLocation.Offset(-mouseOffset.X, -mouseOffset.Y);
                this.Location = newLocation;

                hasMoved = true; // Set the flag when the form is being dragged
            }
        }

        private void lFormTitle_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
            }
        }

        private Random random;
        private int previousTipIndex = -1;
        private string[] tips = { "Made by OneDevXyz (https://onedev.xyz)", "Tip: You can collapse the form by clicking the logo", "Tip: Use Shift + Enter to start a new page", "Tip: You can move the emojis by click-holding any of the emoji buttons for 0.4 seconds", "Tip: The application stores the 10 last texts saved in the history section. To see older ones, click the History Library button (book icon)", "Tip: You can pin history items by right clicking them to prevent them from being removed" }; // Add more tips as needed

        private void TipTimer_Tick(object sender, EventArgs e)
        {

            int tipIndex;
            do
            {
                tipIndex = random.Next(tips.Length);
            } while (tipIndex == previousTipIndex); // Generate a new index if it's the same as the previous one

            tipLabel.Text = tips[tipIndex]; // Set the text to the current tip

            previousTipIndex = tipIndex; // Update the previous tip index
        }

        private void lFormTitle_MouseEnter(object sender, EventArgs e)
        {
            lFormTitle.ForeColor = Color.FromArgb(60, 60, 60);
        }

        private void lFormTitle_MouseLeave(object sender, EventArgs e)
        {
            lFormTitle.ForeColor = Color.WhiteSmoke;
        }

        private void flowHistory_ControlAdded(object sender, ControlEventArgs e)
        {

            ApplyButtonColorFade();
        }

        private void flowHistory_ControlRemoved(object sender, ControlEventArgs e)
        {
            ApplyButtonColorFade();
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackColor = button.ForeColor;
            button.ForeColor = Color.White;
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackColor = Color.Transparent;
            button.ForeColor = button.BackColor;
        }


        private bool isHistoryEntriesDisplayed = false;
        private Panel pHistoryEntries;
        private ToolTip tooltip;



        private void DisposeShowAllHistoryEntriesMenu()
        {
            btnShowAllHistoryEntries.FlatAppearance.BorderSize = 0;
            isHistoryEntriesDisplayed = false;
            pHistoryEntries.Dispose();
            pHolder.BackColor = Color.FromArgb(38, 41, 46); //temp, needs to change considering light/dark mode
            pHolder2.BackColor = Color.FromArgb(38, 41, 46); //temp, needs to change considering light/dark mode
            return; // Panel is already displayed, no need to proceed
        }

        private void btnShowAllHistoryEntries_Click(object sender, EventArgs e)
        {
            if (isHistoryEntriesDisplayed)
            {
                DisposeShowAllHistoryEntriesMenu();
                return;
            }

            DrawingControl.SuspendDrawing(this);

            isHistoryEntriesDisplayed = true;
            pHolder.BackColor = this.BackColor; //temp, needs to change considering light/dark mode
            //pHolder2.BackColor = this.BackColor;

            Panel panel = new Panel();
            pHistoryEntries = panel;
            panel.Size = rtbText.Size;
            panel.Height = rtbText.Height + pHolder.Height + pHolder.Height;
            panel.Location = rtbText.Location;
            panel.BackColor = rtbText.BackColor;
            panel.BorderStyle = BorderStyle.None; // Add border style
            panel.BringToFront();

            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.AutoScroll = true;
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight; // Change flow direction to LeftToRight
            flowLayoutPanel.WrapContents = true; // Wrap the contents to the next line
            flowLayoutPanel.Padding = new Padding(4); // Add padding for spacing


            // Search textbox
            RichTextBox searchTextBox = new RichTextBox()
            {
                Width = 232,
                Location = new Point(10, 2), // Set the location of the searchTextBox
                Font = new Font("Segoe UI", 11.75F),
                BackColor = Color.FromArgb(38, 41, 46),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None, // Remove the border
                Height = 28,
                MaxLength = 30,
                Multiline = false,
                SelectionIndent = 8,
            };

            // Apply rounded corners to the searchTextBox
            searchTextBox.Region = new Region(CreateRoundRectangle(
                searchTextBox.ClientRectangle.X,
                searchTextBox.ClientRectangle.Y,
                searchTextBox.Width,
                searchTextBox.Height,
                CornerRadius));

            // Handle the searchTextBox's Enter event to change its background color
            searchTextBox.Enter += (searchSender, searchEventArgs) =>
            {
                searchTextBox.BackColor = Color.FromArgb(50, 53, 58);
            };

            // Handle the searchTextBox's Leave event to restore its background color
            searchTextBox.Leave += (searchSender, searchEventArgs) =>
            {
                searchTextBox.BackColor = Color.FromArgb(38, 41, 46);
            };

            // Create a timer for delayed search
            System.Timers.Timer searchTimer = new System.Timers.Timer();

            // Set the timer properties
            searchTimer.Interval = 500; // Set the delay in milliseconds
            searchTimer.AutoReset = false; // Set the timer to trigger only once

            // Event handler for the timer's Elapsed event
            searchTimer.Elapsed += (timerSender, timerEventArgs) =>
            {
                string searchText = string.Empty;
                Invoke(new Action(() =>
                {
                    // Get the search text from the text box
                    searchText = searchTextBox.Text.ToLower();
                }));

                foreach (Control control in flowLayoutPanel.Controls)
                {
                    if (control is Panel entryPanel && control.Controls.Count > 1)
                    {
                        Button button = control.Controls[1] as Button;
                        if (button != null)
                        {
                            string buttonText = button.Text.ToLower();
                            bool entryMatchesSearch = buttonText.Contains(searchText);

                            Invoke(new Action(() =>
                            {
                                // Update the visibility of the entryPanel on the UI thread
                                entryPanel.Visible = entryMatchesSearch;
                            }));
                        }
                    }
                }

                // Hide the "Load More" button panel when there is text in the search textbox
                foreach (Control control in flowLayoutPanel.Controls)
                {
                    if (control is Panel entryPanel && control.Controls.Count == 1)
                    {
                        // The panel is the "Load More" button panel
                        Button loadMoreButton = control.Controls[0] as Button;
                        if (loadMoreButton != null && loadMoreButton.Text.Equals("Load More"))
                        {
                            Invoke(new Action(() =>
                            {
                                // Hide the "Load More" button panel
                                entryPanel.Visible = !searchText.Equals("");
                            }));
                        }
                    }
                }

                // Search through all HistoryEntries and update the visibility
                foreach (Panel entryPanel in flowLayoutPanel.Controls.OfType<Panel>())
                {
                    bool entryMatchesSearch = false;
                    Invoke(new Action(() =>
                    {
                        // Get the button control from the entryPanel
                        Button button = entryPanel.Controls.OfType<Button>().FirstOrDefault();
                        if (button != null)
                        {
                            // Get the button text and check if it contains the search text
                            string buttonText = button.Text.ToLower();
                            entryMatchesSearch = buttonText.Contains(searchText);
                        }
                    }));

                    // Update the visibility of the entryPanel on the UI thread
                    Invoke(new Action(() =>
                    {
                        entryPanel.Visible = entryMatchesSearch;

                        // Check if the entryPanel is the "Load More" button panel
                        if (!entryMatchesSearch && entryPanel.Controls.Count == 1)
                        {
                            Button loadMoreButton = entryPanel.Controls.OfType<Button>().FirstOrDefault();
                            if (loadMoreButton != null && loadMoreButton.Text.Equals("Load More"))
                            {
                                entryPanel.Visible = false;
                            }
                        }
                    }));
                }
            };
            int buttonSize = 28;
            int spacing = 5;

            // Handle the searchTextBox's TextChanged event to start the timer for delayed search
            searchTextBox.TextChanged += (searchSender, searchEventArgs) =>
            {
                // Reset the timer on each text change
                //searchTimer.Stop();
                //searchTimer.Start();
            };

            // Create the Search button
            Button btnSearch = new Button()
            {
                Text = "s",
                Size = new Size(buttonSize, buttonSize),
                Location = new Point(searchTextBox.Right + spacing, searchTextBox.Top),
                Font = searchTextBox.Font, // Match font
                BackColor = searchTextBox.BackColor, // Match background color
                ForeColor = searchTextBox.ForeColor, // Match foreground color
                FlatStyle = FlatStyle.Flat // Similar flat style
            };
            btnSearch.FlatAppearance.BorderSize = 0; // No border for flat style
            btnSearch.Click += (searchSender, searchArgs) =>
            {
                PerformSearch(searchTextBox.Text, flowLayoutPanel);
            };

            btnSearch.Click += (searchSender, searchArgs) =>  // Unique names for lambda parameters
            {
                PerformSearch(searchTextBox.Text, flowLayoutPanel);
            };

            // Create the Clear Search button
            // Create the Clear Search button
            Button btnClearSearch = new Button()
            {
                Text = "c",
                Size = new Size(buttonSize, buttonSize),
                Location = new Point(btnSearch.Right + spacing, searchTextBox.Top),
                Font = searchTextBox.Font, // Match font
                BackColor = searchTextBox.BackColor, // Match background color
                ForeColor = searchTextBox.ForeColor, // Match foreground color
                FlatStyle = FlatStyle.Flat // Similar flat style
            };
            btnClearSearch.FlatAppearance.BorderSize = 0; // No border for flat style
            btnClearSearch.Click += (clearSender, clearArgs) =>
            {
                searchTextBox.Clear();
                PerformSearch("", flowLayoutPanel); // Reset the search
            };

            btnSearch.Region = new Region(CreateRoundRectangle(
                btnSearch.ClientRectangle.X,
                btnSearch.ClientRectangle.Y,
                btnSearch.Width,
                btnSearch.Height,
                CornerRadius));

            btnClearSearch.Region = new Region(CreateRoundRectangle(
                btnClearSearch.ClientRectangle.X,
                btnClearSearch.ClientRectangle.Y,
                btnClearSearch.Width,
                btnClearSearch.Height,
                CornerRadius));

            // Create the entry panels
            int startIndex = 0; // Starting index of the visible entries
            int visibleCount = 25; // Number of visible entries
            LoadHistoryEntries(flowLayoutPanel, startIndex, visibleCount);

            // Handle the Scroll event to load more entries when scrolling reaches the bottom
            flowLayoutPanel.Scroll += (scrollSender, scrollEventArgs) =>
            {
                if (flowLayoutPanel.VerticalScroll.Value + flowLayoutPanel.Height >= flowLayoutPanel.VerticalScroll.Maximum - 10)
                {
                    // Load the next batch of entries when scrolling reaches the bottom
                    startIndex += visibleCount;
                    LoadHistoryEntries(flowLayoutPanel, startIndex, visibleCount);
                }
            };

            // Create the button to delete old entries
            Button btnDeleteOldEntries = new Button();
            btnDeleteOldEntries.Text = "Delete Old Entries";
            btnDeleteOldEntries.Height = 40;
            btnDeleteOldEntries.Width = 210;
            btnDeleteOldEntries.Font = new Font("Segoe UI", 9.75F);
            btnDeleteOldEntries.FlatStyle = FlatStyle.Flat; // Set flat style
            btnDeleteOldEntries.FlatAppearance.BorderSize = 0; // Remove border
            btnDeleteOldEntries.BackColor = Color.IndianRed; // Set background color
            btnDeleteOldEntries.ForeColor = Color.WhiteSmoke; // Set text color
            btnDeleteOldEntries.Margin = new Padding(15, 10, 0, 10); // Add top margin for spacing
            btnDeleteOldEntries.Click += (s, ev) =>
            {
                // Delete all history entries except the first 10
                HistoryEntries.RemoveRange(10, HistoryEntries.Count - 10);
                flowLayoutPanel.Controls.Clear(); // Clear the panels
                lHistoryCount.Text = HistoryEntries.Count.ToString(); // Update the history count label
            };

            // Apply rounded corners to the btnDeleteOldEntries button
            btnDeleteOldEntries.Region = new Region(CreateRoundRectangle(
                btnDeleteOldEntries.ClientRectangle.X,
                btnDeleteOldEntries.ClientRectangle.Y,
                btnDeleteOldEntries.Width,
                btnDeleteOldEntries.Height,
                CornerRadius));

            Panel contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = this.BackColor; //temp, needs to change considering light/dark mode

            Panel searchDeletePanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 35,
                Location = new Point(5, 2),
                Padding = new Padding(0),
                BackColor = this.BackColor, // Set the background color to match the form
            };

            contentPanel.Controls.Add(flowLayoutPanel); // Add the flowLayoutPanel first
            contentPanel.Controls.Add(searchDeletePanel); // Add the searchDeletePanel second
            searchDeletePanel.Controls.Add(searchTextBox);
            searchDeletePanel.Controls.Add(btnSearch);
            searchDeletePanel.Controls.Add(btnClearSearch);

            panel.Controls.Add(contentPanel);
            splitContainer1.Panel1.Controls.Add(panel);
            panel.BringToFront();

            isHistoryEntriesDisplayed = true; // Set the flag to indicate that the panel is displayed

            DrawingControl.ResumeDrawing(this);
        }

        private void PerformSearch(string searchText, FlowLayoutPanel flowLayoutPanel)
        {
            searchText = searchText.ToLower();

            foreach (Control control in flowLayoutPanel.Controls)
            {
                if (control is Panel entryPanel && control.Controls.Count > 1)
                {
                    Button button = control.Controls[1] as Button;
                    if (button != null)
                    {
                        string buttonText = button.Text.ToLower();
                        entryPanel.Visible = buttonText.Contains(searchText);
                    }
                }
            }

            // Update the visibility of the "Load More" button
            UpdateLoadMoreButtonVisibility(flowLayoutPanel, searchText);
        }

        private void UpdateLoadMoreButtonVisibility(FlowLayoutPanel flowLayoutPanel, string searchText)
        {
            foreach (Control control in flowLayoutPanel.Controls)
            {
                if (control is Panel entryPanel && control.Controls.Count == 1)
                {
                    Button loadMoreButton = entryPanel.Controls[0] as Button;
                    if (loadMoreButton != null && loadMoreButton.Text.Equals("Load More"))
                    {
                        entryPanel.Visible = string.IsNullOrEmpty(searchText);
                    }
                }
            }
        }


        private void LoadHistoryEntries(FlowLayoutPanel flowLayoutPanel, int startIndex, int visibleCount)
        {
            int endIndex = Math.Min(startIndex + visibleCount, HistoryEntries.Count); // Calculate the end index

            for (int i = startIndex; i < endIndex; i++)
            {
                Panel entryPanel = new Panel();
                entryPanel.Dock = DockStyle.Top;
                entryPanel.Height = 30;
                entryPanel.Width = 298;
                entryPanel.BackColor = Color.FromArgb(25, 32, 35); // Set panel background color
                entryPanel.BorderStyle = BorderStyle.None; // Add border style
                entryPanel.Padding = new Padding(0, 2, 0, 0); // Use padding variable

                // Apply rounded corners to the entryPanel
                entryPanel.Region = new Region(CreateRoundRectangle(
                    entryPanel.ClientRectangle.X,
                    entryPanel.ClientRectangle.Y,
                    entryPanel.Width,
                    entryPanel.Height,
                    CornerRadius));

                // Create label for the entry number
                Label label = new Label();
                label.Text = "" + (i + 1);
                label.Dock = DockStyle.Left;
                label.Width = 30;
                label.Font = new Font("Segoe UI", 7.75F);
                label.BackColor = Color.FromArgb(31, 35, 41);
                label.ForeColor = Color.White;
                label.Padding = new Padding(2);

                // Create button with entry text
                Button button = new Button();
                button.Text = GetButtonLabelText(i, HistoryEntries[i].IsPinned);
                button.FlatStyle = FlatStyle.Flat; // Set flat style
                button.FlatAppearance.BorderSize = 0; // Remove border
                button.Font = new Font("Segoe UI", 9.75F);
                button.BackColor = Color.FromArgb(33, 37, 43);
                button.ForeColor = Color.White; // Set text color
                button.Dock = DockStyle.Fill; // Fill the button within the entry panel
                button.Margin = new Padding(2, 0, 2, 0); // Add margin for spacing

                // Set the Tag property of the button to the corresponding HistoryEntry
                button.Tag = HistoryEntries[i];

                //ToolTip
                tooltip = new ToolTip();
                tooltip.AutoPopDelay = 5000; // Set the tooltip display duration (in milliseconds)
                tooltip.OwnerDraw = true;
                tooltip.Draw += ToolTip_Draw;
                tooltip.BackColor = Color.FromArgb(32, 32, 32);
                tooltip.ForeColor = Color.White;
                tooltip.AutoPopDelay = 8000; // 8 seconds
                                                     // Set the tooltip text
                string tooltipText = HistoryEntries[i].Text.Length <= 50 ? HistoryEntries[i].Text : HistoryEntries[i].Text.Substring(0, 50) + "...";
                tooltip.SetToolTip(button, tooltipText);

                button.Click += (s, ev) =>
                {
                    Button popOutButton = (Button)s;
                    HistoryEntry historyEntry = (HistoryEntry)popOutButton.Tag;
                    string entryText = historyEntry.Text;

                    // Add the HistoryEntry text to rtbText
                    rtbText.Clear();
                    //rtbText.AppendText(entryText);
                    rtbText.Rtf = historyEntry.RtfContent;

                    // Dispose the pHistoryEntries panel
                    DisposeShowAllHistoryEntriesMenu();
                };

                button.MouseDown += (s, ev) =>
                {
                    // Right-click event to show the context menu
                    if (ev.Button == MouseButtons.Right)
                    {
                        contextMenuStrip1.Tag = s;
                        contextMenuStrip1.Items[0].Tag = s; // Update the Pin/Unpin menu item tag
                        contextMenuStrip1.Items[1].Tag = s; // Update the Delete menu item tag
                        contextMenuStrip1.Show(MousePosition);
                    }
                };

                // Create the Pop Out button
                Button btnPopOut = new Button();
                btnPopOut.Text = "❒";
                btnPopOut.Width = 32;
                btnPopOut.FlatStyle = FlatStyle.Flat;
                btnPopOut.FlatAppearance.BorderSize = 0;
                btnPopOut.BackColor = Color.FromArgb(33, 37, 43);
                btnPopOut.ForeColor = Color.White;
                btnPopOut.Dock = DockStyle.Right;
                btnPopOut.Margin = new Padding(0);

                // Set the Tag property of the button to the corresponding HistoryEntry
                btnPopOut.Tag = HistoryEntries[i];

                btnPopOut.Click += (s, ev) =>
                {
                    Button clickedButton = (Button)s;
                    HistoryEntry historyEntry = (HistoryEntry)clickedButton.Tag;

                    // Create and show CustomTextForm with the RTF content of the history entry
                    CustomTextForm customForm = new CustomTextForm(historyEntry.RtfContent, Color.FromArgb(39, 42, 49), SaveHistoryEntries, HistoryEntries);
                    customForm.Show();
                };


                entryPanel.Controls.Add(label);
                entryPanel.Controls.Add(button);
                entryPanel.Controls.Add(btnPopOut);
                flowLayoutPanel.Controls.Add(entryPanel);
            }

            // Check if there are more entries to load
            if (endIndex < HistoryEntries.Count)
            {
                // Add a panel with "Load More" button
                Panel loadMorePanel = new Panel();
                loadMorePanel.Dock = DockStyle.Top;
                loadMorePanel.Height = 30;
                loadMorePanel.Width = 240;
                loadMorePanel.BackColor = Color.FromArgb(25, 32, 35); // Set panel background color
                loadMorePanel.BorderStyle = BorderStyle.None; // Add border style
                loadMorePanel.Padding = new Padding(0, 2, 0, 0); // Use padding variable

                // Apply rounded corners to the loadMorePanel
                loadMorePanel.Region = new Region(CreateRoundRectangle(
                    loadMorePanel.ClientRectangle.X,
                    loadMorePanel.ClientRectangle.Y,
                    loadMorePanel.Width,
                    loadMorePanel.Height,
                    CornerRadius));

                // Create the "Load More" button
                Button btnLoadMore = new Button();
                btnLoadMore.Text = "Load More";
                btnLoadMore.FlatStyle = FlatStyle.Flat; // Set flat style
                btnLoadMore.FlatAppearance.BorderSize = 0; // Remove border
                btnLoadMore.Font = new Font("Segoe UI", 9.75F);
                btnLoadMore.BackColor = Color.FromArgb(33, 37, 43);
                btnLoadMore.ForeColor = Color.White; // Set text color
                btnLoadMore.Dock = DockStyle.Fill; // Fill the button within the loadMorePanel
                btnLoadMore.Margin = new Padding(2, 0, 2, 0); // Add margin for spacing

                // Handle the "Load More" button click event
                btnLoadMore.Click += (s, ev) =>
                {
                    flowLayoutPanel.Controls.Remove(loadMorePanel); // Remove the "Load More" button panel
                    startIndex += visibleCount; // Update the starting index
                    LoadHistoryEntries(flowLayoutPanel, startIndex, visibleCount); // Load the next batch of entries
                };

                loadMorePanel.Controls.Add(btnLoadMore); // Add the "Load More" button to the panel
                flowLayoutPanel.Controls.Add(loadMorePanel); // Add the "Load More" button panel to the flowLayoutPanel
            }
        }




        public class CustomTextForm : Form
        {
            private string entryText;

            private Panel leftPanel;
            private Button closeButton;
            private Button copyButton;
            private CheckBox topMostCheckBox;
            private Label topMostLabel;
            private RichTextBox tbEntryText;
            private Button btnSaveChanges;
            private Color backgroundColor;

            private Action saveHistoryEntries;
            private List<HistoryEntry> historyEntries;

            private Point lastMousePosition;
            private bool isResizing;

            private Size initialSize;
            private Point initialMousePosition;

            public CustomTextForm(string entryRtf, Color backgroundColor, Action saveHistoryEntries, List<HistoryEntry> historyEntries)
            {
                this.entryText = entryRtf; // Store the RTF content
                this.backgroundColor = backgroundColor;
                this.saveHistoryEntries = saveHistoryEntries;
                this.historyEntries = historyEntries;

                InitializeComponents();
            }

            private void InitializeComponents()
            {
                // Form settings
                FormBorderStyle = FormBorderStyle.None;
                BackColor = backgroundColor;
                AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Size = new Size(350, 410);
                Region = new Region(CreateRoundRectangle(0, 0, Width, Height, 10)); // Rounded corners

                // Left panel
                leftPanel = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 60,
                    BackColor = Color.FromArgb(33, 37, 43),
                    Cursor = Cursors.SizeAll
                };
                leftPanel.MouseDown += LeftPanel_MouseDown;
                leftPanel.MouseMove += LeftPanel_MouseMove;
                leftPanel.MouseUp += LeftPanel_MouseUp;

                // Close button
                closeButton = new Button()
                {
                    Text = "✖",
                    ForeColor = Color.White,
                    BackColor = backgroundColor,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(40, 40),
                    Location = new Point(10, 10)
                };
                closeButton.FlatAppearance.BorderSize = 0;
                closeButton.Click += CloseButton_Click;

                // Copy button
                copyButton = new Button()
                {
                    Text = "📝",
                    ForeColor = Color.White,
                    BackColor = backgroundColor,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(40, 40),
                    Location = new Point(10, closeButton.Bottom + 10) // Place the button below the closeButton
                };
                copyButton.FlatAppearance.BorderSize = 0;
                copyButton.Click += CopyButton_Click;

                // TopMost checkbox
                topMostCheckBox = new CheckBox()
                {
                    BackColor = leftPanel.BackColor,
                    Text = "TopMost",
                    ForeColor = Color.White,
                    Size = new Size(50, 25),
                    Dock = DockStyle.Bottom,
                };
                topMostCheckBox.CheckedChanged += TopMostCheckBox_CheckedChanged;

                // Content panel
                Panel contentPanel = new Panel()
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(10),
                    Margin = new Padding(10),
                    BackColor = backgroundColor
                };

                // Entry text RichTextBox
                tbEntryText = new RichTextBox()
                {
                    Multiline = true,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(39, 42, 49),
                    BorderStyle = BorderStyle.None,
                    Dock = DockStyle.Fill,
                    Rtf = entryText // Set the RTF content
                };

                // Save changes button
                btnSaveChanges = new Button()
                {
                    Text = "Save Changes",
                    ForeColor = Color.White,
                    BackColor = backgroundColor,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(120, 30),
                    Location = new Point(10, topMostCheckBox.Bottom + 10) // Place the button below the topMostCheckBox
                };
                btnSaveChanges.FlatAppearance.BorderSize = 0;
                btnSaveChanges.Click += SaveChanges_Click;


                contentPanel.Controls.Add(tbEntryText);
                // Add controls to the form
                Controls.Add(contentPanel);
                Controls.Add(leftPanel);
                leftPanel.Controls.Add(closeButton);
                leftPanel.Controls.Add(copyButton);
                contentPanel.Controls.Add(topMostCheckBox);
                Controls.Add(btnSaveChanges);
            }

            private void TopMostCheckBox_CheckedChanged(object sender, EventArgs e)
            {
                TopMost = topMostCheckBox.Checked;
            }

            private void SaveChanges_Click(object sender, EventArgs e)
            {
                string modifiedRtfContent = tbEntryText.Rtf;

                // Optional: Check if an identical RTF entry already exists
                if (historyEntries.Any(entry => entry.RtfContent == modifiedRtfContent && entry.RtfContent != entryText))
                {
                    // Handle duplicate RTF content case
                    return;
                }

                // Find and update the corresponding HistoryEntry
                foreach (HistoryEntry entry in historyEntries)
                {
                    if (entry.RtfContent == entryText) // Assuming entryText is the original RTF content
                    {
                        entry.RtfContent = modifiedRtfContent;
                        break;
                    }
                }

                // Save the updated HistoryEntries
                saveHistoryEntries();

                // Close the form
                Close();
            }


            private void LeftPanel_MouseDown(object sender, MouseEventArgs e)
            {
                lastMousePosition = e.Location;
                leftPanel.Cursor = Cursors.SizeAll;
            }

            private void LeftPanel_MouseMove(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left && !isResizing)
                {
                    Location = new Point(Location.X + e.X - lastMousePosition.X, Location.Y + e.Y - lastMousePosition.Y);
                }
            }

            private void LeftPanel_MouseUp(object sender, MouseEventArgs e)
            {
                isResizing = false;
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);

                if (e.Button == MouseButtons.Left)
                {
                    isResizing = true;
                    initialSize = Size;
                    initialMousePosition = e.Location;
                }
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);

                if (isResizing)
                {
                    int deltaX = e.X - initialMousePosition.X;
                    int deltaY = e.Y - initialMousePosition.Y;

                    int newWidth = initialSize.Width + deltaX;
                    int newHeight = initialSize.Height + deltaY;

                    if (newWidth > MinimumSize.Width)
                    {
                        Width = newWidth;
                    }

                    if (newHeight > MinimumSize.Height)
                    {
                        Height = newHeight;
                    }
                }
                else
                {
                    const int resizeMargin = 8; // Adjust the margin as needed

                    if (e.X >= Width - resizeMargin && e.Y >= Height - resizeMargin)
                    {
                        Cursor = Cursors.SizeNWSE;
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                base.OnMouseUp(e);
                isResizing = false;
            }

            private void CloseButton_Click(object sender, EventArgs e)
            {
                Close();
            }

            private void CopyButton_Click(object sender, EventArgs e)
            {
                Clipboard.SetText(entryText);
            }

            private GraphicsPath CreateRoundRectangle(int x, int y, int width, int height, int radius)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddLine(x + radius, y, x + width - radius, y);
                path.AddArc(x + width - radius, y, radius, radius, 270, 90);
                path.AddLine(x + width, y + radius, x + width, y + height - radius);
                path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
                path.AddLine(x + width - radius, y + height, x + radius, y + height);
                path.AddArc(x, y + height - radius, radius, radius, 90, 90);
                path.AddLine(x, y + height - radius, x, y + radius);
                path.AddArc(x, y, radius, radius, 180, 90);
                path.CloseFigure();
                return path;
            }
        }


        private void ApplyButtonColorFade()
        {
            int buttonCount = flowHistory.Controls.Count;
            int maxButtonCount = 10; // Maximum number of buttons to display
            Color startColor = Color.FromArgb(254, 254, 254);
            Color endColor = Color.FromArgb(40, 47, 53);

            for (int i = 0; i < maxButtonCount; i++)
            {
                double factor = (double)i / (maxButtonCount - 1); // Calculate the fading factor

                int r = (int)(startColor.R * (1 - factor) + endColor.R * factor); // Interpolate the red component
                int g = (int)(startColor.G * (1 - factor) + endColor.G * factor); // Interpolate the green component
                int b = (int)(startColor.B * (1 - factor) + endColor.B * factor); // Interpolate the blue component

                // Clamp the color values to the valid range of 0-255
                r = Clamp(r, 0, 255);
                g = Clamp(g, 0, 255);
                b = Clamp(b, 0, 255);

                if (i < buttonCount)
                {
                    Button button = flowHistory.Controls[i] as Button;
                    button.ForeColor = Color.FromArgb(r, g, b); // Set the calculated color as the button's foreground color

                    button.MouseEnter += Button_MouseEnter;
                    button.MouseLeave += Button_MouseLeave;
                }
            }
        }


        private int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }


        private void pHolder_Click(object sender, EventArgs e)
        {
            pHolder.Focus();
        }

        private void flowHistory_Paint(object sender, PaintEventArgs e)
        {
            ApplyButtonColorFade();
        }

        private void flowHistory_MouseEnter(object sender, EventArgs e)
        {
            ApplyButtonColorFade();
        }

        private void flowHistory_MouseLeave(object sender, EventArgs e)
        {
            ApplyButtonColorFade();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.ResumeText = rtbText.Text;
            Properties.Settings.Default.Save();
        }

        private void btnEraseText_Click(object sender, EventArgs e)
        {
            rtbText.Clear();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbText.Text))
            {
                rtbText.Controls.Add(placeholderLabel);
            }
        }

        private void rtbText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '|')
            {
                e.Handled = true; // Ignore the character
            }
        }

        public class CustomContextMenuRenderer : ToolStripRenderer
        {
            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                // Set the background color of the entire context menu strip
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(25, 32, 35)))
                {
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
                }
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                // Set the text color
                e.Item.ForeColor = Color.WhiteSmoke;
                base.OnRenderItemText(e);
            }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.Enabled)
                {
                    if (e.Item.Selected)
                    {
                        // Set the mouseover color
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(29, 36, 39)))
                        {
                            e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
                        }
                    }
                    else
                    {
                        // Set the default color
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(25, 32, 35)))
                        {
                            e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
                        }
                    }
                }
                else
                {
                    // Set the disabled color
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(25, 32, 35)))
                    {
                        e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
                    }
                }
            }
        }

        private void rtbText_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentSelectionStart = rtbText.SelectionStart;
                int currentSelectionLength = rtbText.SelectionLength;

                if (currentSelectionLength > 0)
                {
                    // Create the context menu strip
                    ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

                    // Create an instance of your custom renderer
                    CustomContextMenuRenderer renderer = new CustomContextMenuRenderer();

                    // Set the renderer for the context menu strip
                    contextMenuStrip.Renderer = renderer;



                    // Create the "Undo" menu item
                    ToolStripMenuItem undoMenuItem = new ToolStripMenuItem("Undo");
                    undoMenuItem.Click += (undoSender, undoEventArgs) =>
                    {
                        // Undo the last text change
                        rtbText.Undo();
                    };
                    contextMenuStrip.Items.Add(undoMenuItem);

                    // Create the "Redo" menu item
                    ToolStripMenuItem redoMenuItem = new ToolStripMenuItem("Redo");
                    redoMenuItem.Click += (redoSender, redoEventArgs) =>
                    {
                        // Redo the last undone text change
                        rtbText.Redo();
                    };
                    contextMenuStrip.Items.Add(redoMenuItem);

                    // Add a separator
                    contextMenuStrip.Items.Add(new ToolStripSeparator());

                    // Create the "Cut" menu item
                    ToolStripMenuItem cutMenuItem = new ToolStripMenuItem("Cut");
                    cutMenuItem.Click += (cutSender, cutEventArgs) =>
                    {
                        // Cut the selected text to the clipboard
                        rtbText.Cut();
                    };
                    contextMenuStrip.Items.Add(cutMenuItem);

                    // Create the "Copy" menu item
                    ToolStripMenuItem copyMenuItem = new ToolStripMenuItem("Copy");
                    copyMenuItem.Click += (copySender, copyEventArgs) =>
                    {
                        // Copy the selected text to the clipboard
                        rtbText.Copy();
                    };
                    contextMenuStrip.Items.Add(copyMenuItem);

                    // Create the "Paste" menu item
                    ToolStripMenuItem pasteMenuItem = new ToolStripMenuItem("Paste");
                    pasteMenuItem.Click += (pasteSender, pasteEventArgs) =>
                    {
                        // Paste the text from the clipboard at the caret position
                        rtbText.Paste();
                    };
                    contextMenuStrip.Items.Add(pasteMenuItem);

                    // Add a separator
                    contextMenuStrip.Items.Add(new ToolStripSeparator());

                    // Create the "Select All" menu item
                    ToolStripMenuItem selectAllMenuItem = new ToolStripMenuItem("Select All");
                    selectAllMenuItem.Click += (selectAllSender, selectAllEventArgs) =>
                    {
                        // Select all text in the RichTextBox
                        rtbText.SelectAll();
                    };
                    contextMenuStrip.Items.Add(selectAllMenuItem);

                    // Create the "Clear" menu item
                    ToolStripMenuItem clearMenuItem = new ToolStripMenuItem("Clear");
                    clearMenuItem.Click += (clearSender, clearEventArgs) =>
                    {
                        // Clear the content of the RichTextBox
                        rtbText.Clear();
                    };
                    contextMenuStrip.Items.Add(clearMenuItem);

                    // Create the "Bold" menu item
                    ToolStripMenuItem boldMenuItem = new ToolStripMenuItem("Bold");
                    boldMenuItem.Click += (boldSender, boldEventArgs) =>
                    {
                        // Apply bold font style to the selected text
                        rtbText.SelectionFont = new Font(rtbText.SelectionFont, rtbText.SelectionFont.Style ^ FontStyle.Bold);
                    };
                    contextMenuStrip.Items.Add(boldMenuItem);

                    // Create the "Italic" menu item
                    ToolStripMenuItem italicMenuItem = new ToolStripMenuItem("Italic");
                    italicMenuItem.Click += (italicSender, italicEventArgs) =>
                    {
                        // Apply italic font style to the selected text
                        rtbText.SelectionFont = new Font(rtbText.SelectionFont, rtbText.SelectionFont.Style ^ FontStyle.Italic);
                    };
                    contextMenuStrip.Items.Add(italicMenuItem);

                    // Create the "Strikethrough" menu item
                    ToolStripMenuItem strikethroughMenuItem = new ToolStripMenuItem("Strikethrough");
                    strikethroughMenuItem.Click += (strikethroughSender, strikethroughEventArgs) =>
                    {
                        // Toggle the strikethrough effect on the selected text
                        rtbText.SelectionFont = new Font(rtbText.SelectionFont, rtbText.SelectionFont.Style ^ FontStyle.Strikeout);
                    };
                    contextMenuStrip.Items.Add(strikethroughMenuItem);

                    // Add a separator
                    contextMenuStrip.Items.Add(new ToolStripSeparator());

                    // Create the "Insert Date/Time" menu item
                    ToolStripMenuItem insertDateTimeMenuItem = new ToolStripMenuItem("Insert Date/Time");
                    insertDateTimeMenuItem.Click += (insertDateTimeSender, insertDateTimeEventArgs) =>
                    {
                        // Insert the current date and time at the caret position
                        rtbText.SelectedText = DateTime.Now.ToString();
                    };
                    contextMenuStrip.Items.Add(insertDateTimeMenuItem);

                    // Add a separator
                    contextMenuStrip.Items.Add(new ToolStripSeparator());

                    // Create the "Left Align" menu item
                    ToolStripMenuItem leftAlignMenuItem = new ToolStripMenuItem("Left Align");
                    leftAlignMenuItem.Click += (leftAlignSender, leftAlignEventArgs) =>
                    {
                        // Align the selected text to the left
                        rtbText.SelectionAlignment = HorizontalAlignment.Left;
                    };
                    contextMenuStrip.Items.Add(leftAlignMenuItem);

                    // Create the "Center Align" menu item
                    ToolStripMenuItem centerAlignMenuItem = new ToolStripMenuItem("Center Align");
                    centerAlignMenuItem.Click += (centerAlignSender, centerAlignEventArgs) =>
                    {
                        // Align the selected text to the center
                        rtbText.SelectionAlignment = HorizontalAlignment.Center;
                    };
                    contextMenuStrip.Items.Add(centerAlignMenuItem);

                    // Create the "Right Align" menu item
                    ToolStripMenuItem rightAlignMenuItem = new ToolStripMenuItem("Right Align");
                    rightAlignMenuItem.Click += (rightAlignSender, rightAlignEventArgs) =>
                    {
                        // Align the selected text to the right
                        rtbText.SelectionAlignment = HorizontalAlignment.Right;
                    };
                    contextMenuStrip.Items.Add(rightAlignMenuItem);

                    // Add a separator
                    contextMenuStrip.Items.Add(new ToolStripSeparator());

                    // Create the "Bullets" menu item
                    ToolStripMenuItem bulletsMenuItem = new ToolStripMenuItem("Bullets");
                    bulletsMenuItem.Click += (bulletsSender, bulletsEventArgs) =>
                    {
                        // Toggle the bullet style for the selected text
                        rtbText.SelectionBullet = !rtbText.SelectionBullet;
                    };
                    contextMenuStrip.Items.Add(bulletsMenuItem);
                    // Create the "Set Color" menu item
                        ToolStripMenuItem setColorMenuItem = new ToolStripMenuItem("Set Color");

                        // Create sub-menu items for default colors
                        ToolStripMenuItem colorRedMenuItem = new ToolStripMenuItem("Red");
                        colorRedMenuItem.Click += (colorRedSender, colorRedEventArgs) =>
                        {
                            // Set the selected text color to red
                            rtbText.SelectionColor = Color.Red;
                        };
                        setColorMenuItem.DropDownItems.Add(colorRedMenuItem);

                        ToolStripMenuItem colorBlueMenuItem = new ToolStripMenuItem("Blue");
                        colorBlueMenuItem.Click += (colorBlueSender, colorBlueEventArgs) =>
                        {
                            // Set the selected text color to blue
                            rtbText.SelectionColor = Color.Blue;
                        };
                        setColorMenuItem.DropDownItems.Add(colorBlueMenuItem);

                        ToolStripMenuItem colorGreenMenuItem = new ToolStripMenuItem("Green");
                        colorGreenMenuItem.Click += (colorGreenSender, colorGreenEventArgs) =>
                        {
                            // Set the selected text color to green
                            rtbText.SelectionColor = Color.Green;
                        };
                        setColorMenuItem.DropDownItems.Add(colorGreenMenuItem);

                        // Create the "Reset" menu item
                        ToolStripMenuItem resetMenuItem = new ToolStripMenuItem("Reset");
                        resetMenuItem.Click += (resetSender, resetEventArgs) =>
                        {
                            // Reset the selected text color to the default forecolor
                            rtbText.SelectionColor = rtbText.ForeColor;
                        };
                        setColorMenuItem.DropDownItems.Add(resetMenuItem);

                        // Add the "Set Color" menu item to the context menu strip
                        contextMenuStrip.Items.Add(setColorMenuItem);


                    // Add a separator
                    contextMenuStrip.Items.Add(new ToolStripSeparator());

                    // Create the "Increase Indentation" menu item
                    ToolStripMenuItem increaseIndentMenuItem = new ToolStripMenuItem("Increase Indentation");
                    increaseIndentMenuItem.Click += (increaseIndentSender, increaseIndentEventArgs) =>
                    {
                        // Increase the indentation level of the selected text
                        rtbText.SelectionIndent += 20;
                    };
                    contextMenuStrip.Items.Add(increaseIndentMenuItem);

                    // Create the "Decrease Indentation" menu item
                    ToolStripMenuItem decreaseIndentMenuItem = new ToolStripMenuItem("Decrease Indentation");
                    decreaseIndentMenuItem.Click += (decreaseIndentSender, decreaseIndentEventArgs) =>
                    {
                        // Decrease the indentation level of the selected text
                        rtbText.SelectionIndent -= 20;
                    };
                    contextMenuStrip.Items.Add(decreaseIndentMenuItem);

                    // Add a separator
                    contextMenuStrip.Items.Add(new ToolStripSeparator());

                    // Attach the context menu strip to the RichTextBox
                    rtbText.ContextMenuStrip = contextMenuStrip;

                    // Show the context menu strip at the mouse position
                    contextMenuStrip.Show(rtbText, e.Location);
                }
                else
                {
                    // If no text is selected, clear the context menu strip
                    rtbText.ContextMenuStrip = null;
                }
            }
        }

        private void lHistoryTitle_Click(object sender, EventArgs e)
        {
            MessageBox.Show(flowHistory.Controls.Count.ToString());
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel2Collapsed == false)
            {
                splitContainer1.Panel2Collapsed = true;
            } else
            {
                splitContainer1.Panel2Collapsed = false;
            }
        }

        private void btnDownloadAll_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "XML Files|*.xml",
                    Title = "Save Notes as XML"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlElement root = xmlDoc.CreateElement("Notes");
                    xmlDoc.AppendChild(root);

                    // Add history entries to the XML.
                    foreach (string entry in Properties.Settings.Default.HistoryEntries)
                    {
                        XmlElement entryElement = xmlDoc.CreateElement("HistoryEntry");
                        entryElement.InnerText = entry;
                        root.AppendChild(entryElement);
                    }

                    // Assuming PinnedButtons is a delimited string of pinned notes.
                    var pinnedNotes = Properties.Settings.Default.PinnedButtons.Split('|');
                    foreach (string pinnedNote in pinnedNotes)
                    {
                        XmlElement pinnedElement = xmlDoc.CreateElement("PinnedNote");
                        pinnedElement.InnerText = pinnedNote;
                        root.AppendChild(pinnedElement);
                    }

                    xmlDoc.Save(saveFileDialog.FileName);
                    MessageBox.Show("Notes have been saved to XML successfully.", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving notes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
