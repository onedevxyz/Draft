using System.Xml.Linq;
using System.Drawing;

namespace Draft
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.rtbText = new System.Windows.Forms.RichTextBox();
            this.flowHistory = new System.Windows.Forms.FlowLayoutPanel();
            this.lHistoryTitle = new System.Windows.Forms.Label();
            this.pTop = new System.Windows.Forms.Panel();
            this.lFormTitle = new System.Windows.Forms.Label();
            this.lCharacters = new System.Windows.Forms.Label();
            this.lHistoryCount = new System.Windows.Forms.Label();
            this.pHolder = new System.Windows.Forms.Panel();
            this.pSplit = new System.Windows.Forms.Panel();
            this.lDots = new System.Windows.Forms.Label();
            this.toolTipText = new System.Windows.Forms.ToolTip(this.components);
            this.timerCopyIndicator = new System.Windows.Forms.Timer(this.components);
            this.pSeperator = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pHolder2 = new System.Windows.Forms.Panel();
            this.btnView = new Draft.ImageButton();
            this.btnEraseText = new Draft.ImageButton();
            this.btnShowEmojiPanel = new Draft.ImageButton();
            this.btnCopyText = new Draft.ImageButton();
            this.btnSettings = new Draft.ImageButton();
            this.btnClearText = new Draft.ImageButton();
            this.btnDownloadAll = new Draft.ImageButton();
            this.btnShowAllHistoryEntries = new Draft.ImageButton();
            this.btnDeleteHistory = new Draft.ImageButton();
            this.btnMinimize = new Draft.ImageButton();
            this.btnStory = new Draft.ImageButton();
            this.pTop.SuspendLayout();
            this.pHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pHolder2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbText
            // 
            this.rtbText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.rtbText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbText.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbText.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.rtbText.Location = new System.Drawing.Point(22, 15);
            this.rtbText.Margin = new System.Windows.Forms.Padding(15);
            this.rtbText.MaxLength = 50000;
            this.rtbText.Name = "rtbText";
            this.rtbText.Size = new System.Drawing.Size(333, 425);
            this.rtbText.TabIndex = 0;
            this.rtbText.Text = "";
            this.rtbText.SelectionChanged += new System.EventHandler(this.rtbText_SelectionChanged);
            this.rtbText.Click += new System.EventHandler(this.rtbText_Click);
            this.rtbText.TextChanged += new System.EventHandler(this.rtbText_TextChanged);
            this.rtbText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtbText_KeyDown);
            this.rtbText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtbText_KeyPress);
            this.rtbText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rtbText_MouseUp);
            // 
            // flowHistory
            // 
            this.flowHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.flowHistory.Location = new System.Drawing.Point(13, 26);
            this.flowHistory.Margin = new System.Windows.Forms.Padding(0);
            this.flowHistory.Name = "flowHistory";
            this.flowHistory.Size = new System.Drawing.Size(125, 414);
            this.flowHistory.TabIndex = 2;
            this.flowHistory.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.flowHistory_ControlAdded);
            this.flowHistory.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.flowHistory_ControlRemoved);
            this.flowHistory.Paint += new System.Windows.Forms.PaintEventHandler(this.flowHistory_Paint);
            this.flowHistory.MouseEnter += new System.EventHandler(this.flowHistory_MouseEnter);
            this.flowHistory.MouseLeave += new System.EventHandler(this.flowHistory_MouseLeave);
            // 
            // lHistoryTitle
            // 
            this.lHistoryTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lHistoryTitle.AutoSize = true;
            this.lHistoryTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHistoryTitle.Location = new System.Drawing.Point(9, 5);
            this.lHistoryTitle.Name = "lHistoryTitle";
            this.lHistoryTitle.Size = new System.Drawing.Size(60, 21);
            this.lHistoryTitle.TabIndex = 3;
            this.lHistoryTitle.Text = "History";
            this.lHistoryTitle.Click += new System.EventHandler(this.lHistoryTitle_Click);
            // 
            // pTop
            // 
            this.pTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.pTop.Controls.Add(this.lFormTitle);
            this.pTop.Controls.Add(this.btnMinimize);
            this.pTop.Controls.Add(this.btnStory);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(525, 42);
            this.pTop.TabIndex = 6;
            this.pTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pTop_MouseDown);
            this.pTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pTop_MouseMove);
            // 
            // lFormTitle
            // 
            this.lFormTitle.AutoSize = true;
            this.lFormTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lFormTitle.Location = new System.Drawing.Point(9, 7);
            this.lFormTitle.Name = "lFormTitle";
            this.lFormTitle.Size = new System.Drawing.Size(57, 25);
            this.lFormTitle.TabIndex = 13;
            this.lFormTitle.Text = "Draft";
            this.lFormTitle.Click += new System.EventHandler(this.lFormTitle_Click);
            this.lFormTitle.MouseEnter += new System.EventHandler(this.lFormTitle_MouseEnter);
            this.lFormTitle.MouseLeave += new System.EventHandler(this.lFormTitle_MouseLeave);
            // 
            // lCharacters
            // 
            this.lCharacters.AutoSize = true;
            this.lCharacters.BackColor = System.Drawing.Color.Transparent;
            this.lCharacters.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lCharacters.ForeColor = System.Drawing.Color.LightGray;
            this.lCharacters.Location = new System.Drawing.Point(161, 8);
            this.lCharacters.Name = "lCharacters";
            this.lCharacters.Size = new System.Drawing.Size(15, 17);
            this.lCharacters.TabIndex = 9;
            this.lCharacters.Text = "0";
            this.lCharacters.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lHistoryCount
            // 
            this.lHistoryCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lHistoryCount.AutoSize = true;
            this.lHistoryCount.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHistoryCount.Location = new System.Drawing.Point(67, 9);
            this.lHistoryCount.Name = "lHistoryCount";
            this.lHistoryCount.Size = new System.Drawing.Size(34, 17);
            this.lHistoryCount.TabIndex = 0;
            this.lHistoryCount.Text = "0/10";
            // 
            // pHolder
            // 
            this.pHolder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pHolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.pHolder.Controls.Add(this.btnView);
            this.pHolder.Controls.Add(this.btnEraseText);
            this.pHolder.Controls.Add(this.lCharacters);
            this.pHolder.Controls.Add(this.pSplit);
            this.pHolder.Controls.Add(this.btnShowEmojiPanel);
            this.pHolder.Controls.Add(this.btnCopyText);
            this.pHolder.Controls.Add(this.btnSettings);
            this.pHolder.Controls.Add(this.btnClearText);
            this.pHolder.Location = new System.Drawing.Point(22, 463);
            this.pHolder.Name = "pHolder";
            this.pHolder.Size = new System.Drawing.Size(333, 35);
            this.pHolder.TabIndex = 11;
            this.pHolder.Click += new System.EventHandler(this.pHolder_Click);
            // 
            // pSplit
            // 
            this.pSplit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pSplit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.pSplit.Location = new System.Drawing.Point(292, 2);
            this.pSplit.Name = "pSplit";
            this.pSplit.Size = new System.Drawing.Size(6, 35);
            this.pSplit.TabIndex = 16;
            // 
            // lDots
            // 
            this.lDots.AutoSize = true;
            this.lDots.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDots.ForeColor = System.Drawing.Color.DimGray;
            this.lDots.Location = new System.Drawing.Point(12, 36);
            this.lDots.Name = "lDots";
            this.lDots.Size = new System.Drawing.Size(0, 25);
            this.lDots.TabIndex = 14;
            this.lDots.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pSeperator
            // 
            this.pSeperator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(32)))), ((int)(((byte)(35)))));
            this.pSeperator.Location = new System.Drawing.Point(5, 26);
            this.pSeperator.Name = "pSeperator";
            this.pSeperator.Size = new System.Drawing.Size(6, 414);
            this.pSeperator.TabIndex = 15;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 42);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rtbText);
            this.splitContainer1.Panel1.Controls.Add(this.pHolder);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pHolder2);
            this.splitContainer1.Panel2.Controls.Add(this.pSeperator);
            this.splitContainer1.Panel2.Controls.Add(this.lHistoryTitle);
            this.splitContainer1.Panel2.Controls.Add(this.flowHistory);
            this.splitContainer1.Panel2.Controls.Add(this.lHistoryCount);
            this.splitContainer1.Size = new System.Drawing.Size(525, 520);
            this.splitContainer1.SplitterDistance = 370;
            this.splitContainer1.TabIndex = 16;
            // 
            // pHolder2
            // 
            this.pHolder2.Controls.Add(this.btnDownloadAll);
            this.pHolder2.Controls.Add(this.btnShowAllHistoryEntries);
            this.pHolder2.Controls.Add(this.btnDeleteHistory);
            this.pHolder2.Location = new System.Drawing.Point(19, 464);
            this.pHolder2.Name = "pHolder2";
            this.pHolder2.Size = new System.Drawing.Size(111, 32);
            this.pHolder2.TabIndex = 17;
            // 
            // btnView
            // 
            this.btnView.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnView.BackColor = System.Drawing.Color.Transparent;
            this.btnView.ClickImage = global::Draft.Resources.collapse_dark;
            this.btnView.FlatAppearance.BorderSize = 0;
            this.btnView.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnView.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView.HoverImage = global::Draft.Resources.collapse_dark;
            this.btnView.Image = global::Draft.Resources.collapse_light;
            this.btnView.Location = new System.Drawing.Point(306, 8);
            this.btnView.Name = "btnView";
            this.btnView.NormalImage = global::Draft.Resources.collapse_light;
            this.btnView.Size = new System.Drawing.Size(18, 18);
            this.btnView.TabIndex = 12;
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnEraseText
            // 
            this.btnEraseText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEraseText.BackColor = System.Drawing.Color.Transparent;
            this.btnEraseText.ClickImage = global::Draft.Resources.erase_dark;
            this.btnEraseText.FlatAppearance.BorderSize = 0;
            this.btnEraseText.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEraseText.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEraseText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEraseText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEraseText.HoverImage = global::Draft.Resources.erase_dark;
            this.btnEraseText.Image = global::Draft.Resources.erase_light;
            this.btnEraseText.Location = new System.Drawing.Point(46, 0);
            this.btnEraseText.Name = "btnEraseText";
            this.btnEraseText.NormalImage = global::Draft.Resources.erase_light;
            this.btnEraseText.Size = new System.Drawing.Size(32, 32);
            this.btnEraseText.TabIndex = 17;
            this.btnEraseText.UseVisualStyleBackColor = false;
            this.btnEraseText.Click += new System.EventHandler(this.btnEraseText_Click);
            // 
            // btnShowEmojiPanel
            // 
            this.btnShowEmojiPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnShowEmojiPanel.BackColor = System.Drawing.Color.Transparent;
            this.btnShowEmojiPanel.ClickImage = global::Draft.Resources.emoji2_dark;
            this.btnShowEmojiPanel.FlatAppearance.BorderSize = 0;
            this.btnShowEmojiPanel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowEmojiPanel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowEmojiPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowEmojiPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowEmojiPanel.HoverImage = global::Draft.Resources.emoji2_dark;
            this.btnShowEmojiPanel.Image = global::Draft.Resources.emoji2_light;
            this.btnShowEmojiPanel.Location = new System.Drawing.Point(82, 0);
            this.btnShowEmojiPanel.Name = "btnShowEmojiPanel";
            this.btnShowEmojiPanel.NormalImage = global::Draft.Resources.emoji2_light;
            this.btnShowEmojiPanel.Size = new System.Drawing.Size(32, 32);
            this.btnShowEmojiPanel.TabIndex = 6;
            this.btnShowEmojiPanel.UseVisualStyleBackColor = false;
            this.btnShowEmojiPanel.Click += new System.EventHandler(this.btnShowEmojiPanel_Click);
            // 
            // btnCopyText
            // 
            this.btnCopyText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCopyText.BackColor = System.Drawing.Color.Transparent;
            this.btnCopyText.ClickImage = global::Draft.Resources.paste_dark;
            this.btnCopyText.FlatAppearance.BorderSize = 0;
            this.btnCopyText.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCopyText.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCopyText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopyText.HoverImage = global::Draft.Resources.paste_dark;
            this.btnCopyText.Image = global::Draft.Resources.paste_light;
            this.btnCopyText.Location = new System.Drawing.Point(12, 0);
            this.btnCopyText.Name = "btnCopyText";
            this.btnCopyText.NormalImage = global::Draft.Resources.paste_light;
            this.btnCopyText.Size = new System.Drawing.Size(32, 32);
            this.btnCopyText.TabIndex = 5;
            this.btnCopyText.UseVisualStyleBackColor = false;
            this.btnCopyText.Click += new System.EventHandler(this.btnCopyText_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSettings.AutoSize = true;
            this.btnSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnSettings.ClickImage = global::Draft.Resources.settings_dark;
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.HoverImage = global::Draft.Resources.settings_dark;
            this.btnSettings.Image = global::Draft.Resources.settings_light;
            this.btnSettings.Location = new System.Drawing.Point(218, 2);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.NormalImage = global::Draft.Resources.settings_light;
            this.btnSettings.Size = new System.Drawing.Size(30, 30);
            this.btnSettings.TabIndex = 10;
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnClearText
            // 
            this.btnClearText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClearText.BackColor = System.Drawing.Color.Transparent;
            this.btnClearText.ClickImage = global::Draft.Resources.cleartext_dark;
            this.btnClearText.FlatAppearance.BorderSize = 0;
            this.btnClearText.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClearText.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClearText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearText.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearText.HoverImage = global::Draft.Resources.cleartext_dark;
            this.btnClearText.Image = global::Draft.Resources.cleartext_light;
            this.btnClearText.Location = new System.Drawing.Point(252, 1);
            this.btnClearText.Name = "btnClearText";
            this.btnClearText.NormalImage = global::Draft.Resources.cleartext_light;
            this.btnClearText.Size = new System.Drawing.Size(32, 32);
            this.btnClearText.TabIndex = 4;
            this.btnClearText.UseVisualStyleBackColor = false;
            this.btnClearText.Click += new System.EventHandler(this.btnClearText_Click);
            // 
            // btnDownloadAll
            // 
            this.btnDownloadAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDownloadAll.BackColor = System.Drawing.Color.Transparent;
            this.btnDownloadAll.ClickImage = global::Draft.Resources.download_dark;
            this.btnDownloadAll.FlatAppearance.BorderSize = 0;
            this.btnDownloadAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDownloadAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDownloadAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadAll.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadAll.HoverImage = global::Draft.Resources.download_dark;
            this.btnDownloadAll.Image = global::Draft.Resources.download_light;
            this.btnDownloadAll.Location = new System.Drawing.Point(38, 2);
            this.btnDownloadAll.Name = "btnDownloadAll";
            this.btnDownloadAll.NormalImage = global::Draft.Resources.download_light;
            this.btnDownloadAll.Size = new System.Drawing.Size(32, 27);
            this.btnDownloadAll.TabIndex = 16;
            this.btnDownloadAll.UseVisualStyleBackColor = false;
            this.btnDownloadAll.Click += new System.EventHandler(this.btnDownloadAll_Click);
            // 
            // btnShowAllHistoryEntries
            // 
            this.btnShowAllHistoryEntries.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnShowAllHistoryEntries.BackColor = System.Drawing.Color.Transparent;
            this.btnShowAllHistoryEntries.ClickImage = global::Draft.Resources.book_dark;
            this.btnShowAllHistoryEntries.FlatAppearance.BorderSize = 0;
            this.btnShowAllHistoryEntries.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowAllHistoryEntries.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowAllHistoryEntries.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowAllHistoryEntries.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowAllHistoryEntries.HoverImage = global::Draft.Resources.book_dark;
            this.btnShowAllHistoryEntries.Image = global::Draft.Resources.book_light;
            this.btnShowAllHistoryEntries.Location = new System.Drawing.Point(3, 1);
            this.btnShowAllHistoryEntries.Name = "btnShowAllHistoryEntries";
            this.btnShowAllHistoryEntries.NormalImage = global::Draft.Resources.book_light;
            this.btnShowAllHistoryEntries.Size = new System.Drawing.Size(32, 32);
            this.btnShowAllHistoryEntries.TabIndex = 15;
            this.btnShowAllHistoryEntries.UseVisualStyleBackColor = false;
            this.btnShowAllHistoryEntries.Click += new System.EventHandler(this.btnShowAllHistoryEntries_Click);
            // 
            // btnDeleteHistory
            // 
            this.btnDeleteHistory.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteHistory.AutoSize = true;
            this.btnDeleteHistory.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteHistory.ClickImage = global::Draft.Resources.broom_dark;
            this.btnDeleteHistory.FlatAppearance.BorderSize = 0;
            this.btnDeleteHistory.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDeleteHistory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDeleteHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteHistory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteHistory.HoverImage = global::Draft.Resources.broom_dark;
            this.btnDeleteHistory.Image = global::Draft.Resources.bloom_light;
            this.btnDeleteHistory.Location = new System.Drawing.Point(76, 0);
            this.btnDeleteHistory.Name = "btnDeleteHistory";
            this.btnDeleteHistory.NormalImage = global::Draft.Resources.bloom_light;
            this.btnDeleteHistory.Size = new System.Drawing.Size(32, 32);
            this.btnDeleteHistory.TabIndex = 7;
            this.btnDeleteHistory.UseVisualStyleBackColor = false;
            this.btnDeleteHistory.Click += new System.EventHandler(this.btnDeleteHistory_Click);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.ClickImage = global::Draft.Resources.minform_dark;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.HoverImage = global::Draft.Resources.minform_dark;
            this.btnMinimize.Image = global::Draft.Resources.minform_light;
            this.btnMinimize.Location = new System.Drawing.Point(460, 6);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.NormalImage = global::Draft.Resources.minform_light;
            this.btnMinimize.Size = new System.Drawing.Size(26, 26);
            this.btnMinimize.TabIndex = 12;
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnStory
            // 
            this.btnStory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStory.BackColor = System.Drawing.Color.Transparent;
            this.btnStory.ClickImage = global::Draft.Resources.close_dark;
            this.btnStory.FlatAppearance.BorderSize = 0;
            this.btnStory.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStory.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStory.HoverImage = global::Draft.Resources.close_dark;
            this.btnStory.Image = global::Draft.Resources.close_light;
            this.btnStory.Location = new System.Drawing.Point(487, 6);
            this.btnStory.Name = "btnStory";
            this.btnStory.NormalImage = global::Draft.Resources.close_light;
            this.btnStory.Size = new System.Drawing.Size(26, 26);
            this.btnStory.TabIndex = 11;
            this.btnStory.UseVisualStyleBackColor = false;
            this.btnStory.Click += new System.EventHandler(this.btnStory_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.ClientSize = new System.Drawing.Size(525, 562);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.lDots);
            this.Controls.Add(this.pTop);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pHolder.ResumeLayout(false);
            this.pHolder.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pHolder2.ResumeLayout(false);
            this.pHolder2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbText;
        private System.Windows.Forms.FlowLayoutPanel flowHistory;
        private System.Windows.Forms.Label lHistoryTitle;
        private Draft.ImageButton btnClearText;
        private Draft.ImageButton btnCopyText;
        private System.Windows.Forms.Panel pTop;
        private Draft.ImageButton btnDeleteHistory;
        private System.Windows.Forms.Label lHistoryCount;
        private System.Windows.Forms.Label lCharacters;
        private Draft.ImageButton btnSettings;
        private Draft.ImageButton btnMinimize;
        private Draft.ImageButton btnStory;
        private System.Windows.Forms.Panel pHolder;
        private System.Windows.Forms.ToolTip toolTipText;
        private System.Windows.Forms.Timer timerCopyIndicator;
        private Draft.ImageButton btnShowEmojiPanel;
        private System.Windows.Forms.Label lFormTitle;
        private System.Windows.Forms.Label lDots;
        private ImageButton btnShowAllHistoryEntries;
        private System.Windows.Forms.Panel pSeperator;
        private System.Windows.Forms.Panel pSplit;
        private ImageButton btnEraseText;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pHolder2;
        private ImageButton btnView;
        private ImageButton btnDownloadAll;
    }
}

