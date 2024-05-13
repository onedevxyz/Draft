
namespace Draft
{
    partial class CustomMessageBoxForm
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
            this.timerCopyIndicator = new System.Windows.Forms.Timer(this.components);
            this.lDots = new System.Windows.Forms.Label();
            this.lTitle = new System.Windows.Forms.Label();
            this.pTop = new System.Windows.Forms.Panel();
            this.btnCloseMessagebox = new Draft.ImageButton();
            this.toolTipText = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lMessage = new System.Windows.Forms.Label();
            this.labelbuttonNo = new System.Windows.Forms.Label();
            this.labelbuttonYes = new System.Windows.Forms.Label();
            this.labelbuttonOK = new System.Windows.Forms.Label();
            this.pTop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lDots
            // 
            this.lDots.AutoSize = true;
            this.lDots.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDots.ForeColor = System.Drawing.Color.DimGray;
            this.lDots.Location = new System.Drawing.Point(12, 40);
            this.lDots.Name = "lDots";
            this.lDots.Size = new System.Drawing.Size(0, 25);
            this.lDots.TabIndex = 22;
            this.lDots.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lTitle
            // 
            this.lTitle.AutoSize = true;
            this.lTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTitle.Location = new System.Drawing.Point(12, 14);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(49, 25);
            this.lTitle.TabIndex = 13;
            this.lTitle.Text = "Title";
            this.lTitle.TextChanged += new System.EventHandler(this.lTitle_TextChanged);
            // 
            // pTop
            // 
            this.pTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.pTop.Controls.Add(this.lTitle);
            this.pTop.Controls.Add(this.btnCloseMessagebox);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(378, 42);
            this.pTop.TabIndex = 20;
            this.pTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pTop_MouseDown);
            this.pTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pTop_MouseMove);
            this.pTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pTop_MouseUp_1);
            // 
            // btnCloseMessagebox
            // 
            this.btnCloseMessagebox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseMessagebox.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseMessagebox.ClickImage = global::Draft.Resources.close_dark;
            this.btnCloseMessagebox.FlatAppearance.BorderSize = 0;
            this.btnCloseMessagebox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCloseMessagebox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCloseMessagebox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseMessagebox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseMessagebox.HoverImage = global::Draft.Resources.close_dark;
            this.btnCloseMessagebox.Image = global::Draft.Resources.close_light;
            this.btnCloseMessagebox.Location = new System.Drawing.Point(337, 14);
            this.btnCloseMessagebox.Name = "btnCloseMessagebox";
            this.btnCloseMessagebox.NormalImage = global::Draft.Resources.close_light;
            this.btnCloseMessagebox.Size = new System.Drawing.Size(26, 26);
            this.btnCloseMessagebox.TabIndex = 11;
            this.btnCloseMessagebox.UseVisualStyleBackColor = false;
            this.btnCloseMessagebox.Click += new System.EventHandler(this.btnCloseMessagebox_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(43)))));
            this.panel1.Controls.Add(this.labelbuttonOK);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.labelbuttonNo);
            this.panel1.Controls.Add(this.labelbuttonYes);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(378, 310);
            this.panel1.TabIndex = 23;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lMessage);
            this.panel2.Location = new System.Drawing.Point(18, 18);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(337, 238);
            this.panel2.TabIndex = 17;
            // 
            // lMessage
            // 
            this.lMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lMessage.Font = new System.Drawing.Font("Segoe UI Emoji", 12F);
            this.lMessage.Location = new System.Drawing.Point(0, 0);
            this.lMessage.Name = "lMessage";
            this.lMessage.Size = new System.Drawing.Size(337, 238);
            this.lMessage.TabIndex = 16;
            this.lMessage.Text = "Message";
            // 
            // labelbuttonNo
            // 
            this.labelbuttonNo.AutoSize = true;
            this.labelbuttonNo.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelbuttonNo.Location = new System.Drawing.Point(141, 267);
            this.labelbuttonNo.Name = "labelbuttonNo";
            this.labelbuttonNo.Size = new System.Drawing.Size(41, 25);
            this.labelbuttonNo.TabIndex = 15;
            this.labelbuttonNo.Text = "NO";
            this.labelbuttonNo.Click += new System.EventHandler(this.labelbuttonNo_Click);
            this.labelbuttonNo.MouseEnter += new System.EventHandler(this.labelbuttonNo_MouseEnter);
            this.labelbuttonNo.MouseLeave += new System.EventHandler(this.labelbuttonNo_MouseLeave);
            // 
            // labelbuttonYes
            // 
            this.labelbuttonYes.AutoSize = true;
            this.labelbuttonYes.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelbuttonYes.Location = new System.Drawing.Point(197, 267);
            this.labelbuttonYes.Name = "labelbuttonYes";
            this.labelbuttonYes.Size = new System.Drawing.Size(43, 25);
            this.labelbuttonYes.TabIndex = 14;
            this.labelbuttonYes.Text = "YES";
            this.labelbuttonYes.Click += new System.EventHandler(this.labelbuttonYes_Click);
            this.labelbuttonYes.MouseEnter += new System.EventHandler(this.labelbuttonYes_MouseEnter);
            this.labelbuttonYes.MouseLeave += new System.EventHandler(this.labelbuttonYes_MouseLeave);
            // 
            // labelbuttonOK
            // 
            this.labelbuttonOK.AutoSize = true;
            this.labelbuttonOK.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelbuttonOK.Location = new System.Drawing.Point(171, 267);
            this.labelbuttonOK.Name = "labelbuttonOK";
            this.labelbuttonOK.Size = new System.Drawing.Size(38, 25);
            this.labelbuttonOK.TabIndex = 17;
            this.labelbuttonOK.Text = "OK";
            this.labelbuttonOK.Click += new System.EventHandler(this.labelbuttonOK_Click);
            this.labelbuttonOK.MouseEnter += new System.EventHandler(this.labelbuttonOK_MouseEnter);
            this.labelbuttonOK.MouseLeave += new System.EventHandler(this.labelbuttonOK_MouseLeave);
            // 
            // CustomMessageBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 352);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lDots);
            this.Controls.Add(this.pTop);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomMessageBoxForm";
            this.Text = "CustomMessageBoxForm";
            this.Load += new System.EventHandler(this.CustomMessageBoxForm_Load);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageButton btnCloseMessagebox;
        private System.Windows.Forms.Timer timerCopyIndicator;
        private System.Windows.Forms.Label lDots;
        private System.Windows.Forms.Label lTitle;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.ToolTip toolTipText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelbuttonNo;
        private System.Windows.Forms.Label labelbuttonYes;
        private System.Windows.Forms.Label lMessage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelbuttonOK;
    }
}