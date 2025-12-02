using System.Drawing;
using System.Windows.Forms;

namespace UnityCompileCacheGUI_Winform;

partial class UnityCompileCacheGUI_Winform_Form
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tabControl_Main = new TabControl();
        tabPage1 = new TabPage();
        tableLayoutPanel1 = new TableLayoutPanel();
        btn_Apply_android = new Button();
        btn_Revert_android = new Button();
        btn_Refresh_android = new Button();
        lbl_notify_android = new Label();
        listView_Swap_android = new ListView();
        combo_cacheType_android = new ComboBox();
        tabPage2 = new TabPage();
        tableLayoutPanel2 = new TableLayoutPanel();
        btn_Apply_webgl = new Button();
        btn_Revert_webgl = new Button();
        btn_Refresh_webgl = new Button();
        lbl_notify_webgl = new Label();
        listView_Swap_webgl = new ListView();
        combo_cacheType_webgl = new ComboBox();
        tabPage3 = new TabPage();
        tableLayoutPanel3 = new TableLayoutPanel();
        btn_ccache_config = new Button();
        link_ccache = new LinkLabel();
        link_sccache = new LinkLabel();
        btn_sccache_config = new Button();
        tabPage4 = new TabPage();
        lbl_info = new RichTextBox();
        tabControl_Main.SuspendLayout();
        tabPage1.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        tabPage2.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        tabPage3.SuspendLayout();
        tableLayoutPanel3.SuspendLayout();
        tabPage4.SuspendLayout();
        SuspendLayout();
        // 
        // tabControl_Main
        // 
        tabControl_Main.Controls.Add(tabPage1);
        tabControl_Main.Controls.Add(tabPage2);
        tabControl_Main.Controls.Add(tabPage3);
        tabControl_Main.Controls.Add(tabPage4);
        tabControl_Main.Dock = DockStyle.Fill;
        tabControl_Main.Location = new Point(0, 0);
        tabControl_Main.Name = "tabControl_Main";
        tabControl_Main.SelectedIndex = 0;
        tabControl_Main.Size = new Size(800, 450);
        tabControl_Main.TabIndex = 1;
        tabControl_Main.SelectedIndexChanged += tabControl_Main_SelectedIndexChanged;
        // 
        // tabPage1
        // 
        tabPage1.Controls.Add(tableLayoutPanel1);
        tabPage1.Location = new Point(4, 24);
        tabPage1.Name = "tabPage1";
        tabPage1.Padding = new Padding(3);
        tabPage1.Size = new Size(792, 422);
        tabPage1.TabIndex = 0;
        tabPage1.Text = "Android(NDK)";
        tabPage1.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 4;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17.1241837F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 82.87582F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 89F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 93F));
        tableLayoutPanel1.Controls.Add(btn_Apply_android, 3, 2);
        tableLayoutPanel1.Controls.Add(btn_Revert_android, 2, 2);
        tableLayoutPanel1.Controls.Add(btn_Refresh_android, 3, 0);
        tableLayoutPanel1.Controls.Add(lbl_notify_android, 1, 2);
        tableLayoutPanel1.Controls.Add(listView_Swap_android, 0, 1);
        tableLayoutPanel1.Controls.Add(combo_cacheType_android, 0, 2);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(3, 3);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.1772156F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 85.8227844F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 71F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel1.Size = new Size(786, 416);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // btn_Apply_android
        // 
        btn_Apply_android.Dock = DockStyle.Fill;
        btn_Apply_android.Location = new Point(695, 347);
        btn_Apply_android.Name = "btn_Apply_android";
        btn_Apply_android.Size = new Size(88, 66);
        btn_Apply_android.TabIndex = 2;
        btn_Apply_android.Text = "Apply";
        btn_Apply_android.UseVisualStyleBackColor = true;
        btn_Apply_android.Click += btn_Apply_Click;
        // 
        // btn_Revert_android
        // 
        btn_Revert_android.Dock = DockStyle.Fill;
        btn_Revert_android.Location = new Point(606, 347);
        btn_Revert_android.Name = "btn_Revert_android";
        btn_Revert_android.Size = new Size(83, 66);
        btn_Revert_android.TabIndex = 1;
        btn_Revert_android.Text = "Revert";
        btn_Revert_android.UseVisualStyleBackColor = true;
        btn_Revert_android.Click += btn_Revert_Click;
        // 
        // btn_Refresh_android
        // 
        btn_Refresh_android.Dock = DockStyle.Fill;
        btn_Refresh_android.Location = new Point(695, 3);
        btn_Refresh_android.Name = "btn_Refresh_android";
        btn_Refresh_android.Size = new Size(88, 42);
        btn_Refresh_android.TabIndex = 3;
        btn_Refresh_android.Text = "Refresh";
        btn_Refresh_android.UseVisualStyleBackColor = true;
        btn_Refresh_android.Click += btn_Refresh_Click;
        // 
        // lbl_notify_android
        // 
        lbl_notify_android.Dock = DockStyle.Fill;
        lbl_notify_android.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lbl_notify_android.Location = new Point(106, 344);
        lbl_notify_android.Name = "lbl_notify_android";
        lbl_notify_android.Size = new Size(494, 72);
        lbl_notify_android.TabIndex = 4;
        lbl_notify_android.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // listView_Swap_android
        // 
        tableLayoutPanel1.SetColumnSpan(listView_Swap_android, 4);
        listView_Swap_android.Dock = DockStyle.Fill;
        listView_Swap_android.Location = new Point(3, 51);
        listView_Swap_android.Name = "listView_Swap_android";
        listView_Swap_android.Size = new Size(780, 290);
        listView_Swap_android.TabIndex = 0;
        listView_Swap_android.UseCompatibleStateImageBehavior = false;
        listView_Swap_android.ColumnClick += listView_Swap_ColumnClick;
        listView_Swap_android.DrawColumnHeader += listView_Swap_DrawColumnHeader;
        listView_Swap_android.DrawItem += listView_Swap_DrawItem;
        listView_Swap_android.DrawSubItem += listView_Swap_DrawSubItem;
        // 
        // combo_cacheType_android
        // 
        combo_cacheType_android.DropDownStyle = ComboBoxStyle.DropDownList;
        combo_cacheType_android.FormattingEnabled = true;
        combo_cacheType_android.Location = new Point(3, 347);
        combo_cacheType_android.Name = "combo_cacheType_android";
        combo_cacheType_android.Size = new Size(97, 23);
        combo_cacheType_android.TabIndex = 5;
        combo_cacheType_android.SelectedIndexChanged += combo_cacheType_SelectedIndexChanged;
        // 
        // tabPage2
        // 
        tabPage2.Controls.Add(tableLayoutPanel2);
        tabPage2.Location = new Point(4, 24);
        tabPage2.Name = "tabPage2";
        tabPage2.Padding = new Padding(3);
        tabPage2.Size = new Size(792, 422);
        tabPage2.TabIndex = 1;
        tabPage2.Text = "WebGL(Emscripten)";
        tabPage2.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.ColumnCount = 4;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17.1241837F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 82.87582F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 89F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 93F));
        tableLayoutPanel2.Controls.Add(btn_Apply_webgl, 3, 2);
        tableLayoutPanel2.Controls.Add(btn_Revert_webgl, 2, 2);
        tableLayoutPanel2.Controls.Add(btn_Refresh_webgl, 3, 0);
        tableLayoutPanel2.Controls.Add(lbl_notify_webgl, 1, 2);
        tableLayoutPanel2.Controls.Add(listView_Swap_webgl, 0, 1);
        tableLayoutPanel2.Controls.Add(combo_cacheType_webgl, 0, 2);
        tableLayoutPanel2.Dock = DockStyle.Fill;
        tableLayoutPanel2.Location = new Point(3, 3);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 3;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 14.1772156F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 85.8227844F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 71F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.Size = new Size(786, 416);
        tableLayoutPanel2.TabIndex = 1;
        // 
        // btn_Apply_webgl
        // 
        btn_Apply_webgl.Dock = DockStyle.Fill;
        btn_Apply_webgl.Location = new Point(695, 347);
        btn_Apply_webgl.Name = "btn_Apply_webgl";
        btn_Apply_webgl.Size = new Size(88, 66);
        btn_Apply_webgl.TabIndex = 2;
        btn_Apply_webgl.Text = "Apply";
        btn_Apply_webgl.UseVisualStyleBackColor = true;
        btn_Apply_webgl.Click += btn_Apply_Click;
        // 
        // btn_Revert_webgl
        // 
        btn_Revert_webgl.Dock = DockStyle.Fill;
        btn_Revert_webgl.Location = new Point(606, 347);
        btn_Revert_webgl.Name = "btn_Revert_webgl";
        btn_Revert_webgl.Size = new Size(83, 66);
        btn_Revert_webgl.TabIndex = 1;
        btn_Revert_webgl.Text = "Revert";
        btn_Revert_webgl.UseVisualStyleBackColor = true;
        btn_Revert_webgl.Click += btn_Revert_Click;
        // 
        // btn_Refresh_webgl
        // 
        btn_Refresh_webgl.Dock = DockStyle.Fill;
        btn_Refresh_webgl.Location = new Point(695, 3);
        btn_Refresh_webgl.Name = "btn_Refresh_webgl";
        btn_Refresh_webgl.Size = new Size(88, 42);
        btn_Refresh_webgl.TabIndex = 3;
        btn_Refresh_webgl.Text = "Refresh";
        btn_Refresh_webgl.UseVisualStyleBackColor = true;
        btn_Refresh_webgl.Click += btn_Refresh_Click;
        // 
        // lbl_notify_webgl
        // 
        lbl_notify_webgl.Dock = DockStyle.Fill;
        lbl_notify_webgl.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lbl_notify_webgl.Location = new Point(106, 344);
        lbl_notify_webgl.Name = "lbl_notify_webgl";
        lbl_notify_webgl.Size = new Size(494, 72);
        lbl_notify_webgl.TabIndex = 4;
        lbl_notify_webgl.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // listView_Swap_webgl
        // 
        tableLayoutPanel2.SetColumnSpan(listView_Swap_webgl, 4);
        listView_Swap_webgl.Dock = DockStyle.Fill;
        listView_Swap_webgl.Location = new Point(3, 51);
        listView_Swap_webgl.Name = "listView_Swap_webgl";
        listView_Swap_webgl.Size = new Size(780, 290);
        listView_Swap_webgl.TabIndex = 0;
        listView_Swap_webgl.UseCompatibleStateImageBehavior = false;
        listView_Swap_webgl.ColumnClick += listView_Swap_ColumnClick;
        listView_Swap_webgl.DrawColumnHeader += listView_Swap_DrawColumnHeader;
        listView_Swap_webgl.DrawItem += listView_Swap_DrawItem;
        listView_Swap_webgl.DrawSubItem += listView_Swap_DrawSubItem;
        // 
        // combo_cacheType_webgl
        // 
        combo_cacheType_webgl.DropDownStyle = ComboBoxStyle.DropDownList;
        combo_cacheType_webgl.FormattingEnabled = true;
        combo_cacheType_webgl.Location = new Point(3, 347);
        combo_cacheType_webgl.Name = "combo_cacheType_webgl";
        combo_cacheType_webgl.Size = new Size(97, 23);
        combo_cacheType_webgl.TabIndex = 6;
        combo_cacheType_webgl.SelectedIndexChanged += combo_cacheType_SelectedIndexChanged;
        // 
        // tabPage3
        // 
        tabPage3.Controls.Add(tableLayoutPanel3);
        tabPage3.Location = new Point(4, 24);
        tabPage3.Name = "tabPage3";
        tabPage3.Padding = new Padding(3);
        tabPage3.Size = new Size(792, 422);
        tabPage3.TabIndex = 2;
        tabPage3.Text = "Etc";
        tabPage3.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel3
        // 
        tableLayoutPanel3.ColumnCount = 3;
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 264F));
        tableLayoutPanel3.Controls.Add(btn_ccache_config, 1, 0);
        tableLayoutPanel3.Controls.Add(link_ccache, 0, 0);
        tableLayoutPanel3.Controls.Add(link_sccache, 0, 1);
        tableLayoutPanel3.Controls.Add(btn_sccache_config, 1, 1);
        tableLayoutPanel3.Dock = DockStyle.Fill;
        tableLayoutPanel3.Location = new Point(3, 3);
        tableLayoutPanel3.Name = "tableLayoutPanel3";
        tableLayoutPanel3.RowCount = 3;
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 307F));
        tableLayoutPanel3.Size = new Size(786, 416);
        tableLayoutPanel3.TabIndex = 0;
        // 
        // btn_ccache_config
        // 
        btn_ccache_config.Anchor = AnchorStyles.None;
        btn_ccache_config.Location = new Point(316, 15);
        btn_ccache_config.Name = "btn_ccache_config";
        btn_ccache_config.Size = new Size(151, 23);
        btn_ccache_config.TabIndex = 1;
        btn_ccache_config.Text = "Open Ccache's config";
        btn_ccache_config.UseVisualStyleBackColor = true;
        btn_ccache_config.Click += btn_ccache_config_Click;
        // 
        // link_ccache
        // 
        link_ccache.Anchor = AnchorStyles.None;
        link_ccache.AutoSize = true;
        link_ccache.Location = new Point(11, 19);
        link_ccache.Name = "link_ccache";
        link_ccache.Size = new Size(239, 15);
        link_ccache.TabIndex = 2;
        link_ccache.TabStop = true;
        link_ccache.Text = "https://github.com/ccache/ccache/releases";
        link_ccache.LinkClicked += link_ccache_sccache_LinkClicked;
        // 
        // link_sccache
        // 
        link_sccache.Anchor = AnchorStyles.None;
        link_sccache.AutoSize = true;
        link_sccache.Location = new Point(8, 73);
        link_sccache.Name = "link_sccache";
        link_sccache.Size = new Size(245, 15);
        link_sccache.TabIndex = 3;
        link_sccache.TabStop = true;
        link_sccache.Text = "https://github.com/mozilla/sccache/releases";
        link_sccache.LinkClicked += link_ccache_sccache_LinkClicked;
        // 
        // btn_sccache_config
        // 
        btn_sccache_config.Anchor = AnchorStyles.None;
        btn_sccache_config.Location = new Point(316, 69);
        btn_sccache_config.Name = "btn_sccache_config";
        btn_sccache_config.Size = new Size(151, 23);
        btn_sccache_config.TabIndex = 0;
        btn_sccache_config.Text = "Open Sccache's config";
        btn_sccache_config.UseVisualStyleBackColor = true;
        btn_sccache_config.Click += btn_sccache_config_Click;
        // 
        // tabPage4
        // 
        tabPage4.Controls.Add(lbl_info);
        tabPage4.Location = new Point(4, 24);
        tabPage4.Name = "tabPage4";
        tabPage4.Padding = new Padding(3);
        tabPage4.Size = new Size(792, 422);
        tabPage4.TabIndex = 3;
        tabPage4.Text = "Info";
        tabPage4.UseVisualStyleBackColor = true;
        // 
        // lbl_info
        // 
        lbl_info.BackColor = SystemColors.Control;
        lbl_info.Dock = DockStyle.Fill;
        lbl_info.Location = new Point(3, 3);
        lbl_info.Name = "lbl_info";
        lbl_info.Size = new Size(786, 416);
        lbl_info.TabIndex = 0;
        lbl_info.Text = "";
        lbl_info.LinkClicked += lbl_info_LinkClicked;
        // 
        // UnityCompileCacheGUI_Winform_Form
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(tabControl_Main);
        Name = "UnityCompileCacheGUI_Winform_Form";
        Text = "UnityCompileCacheGUI_Winform";
        Load += UnityCompileCacheGUI_Winform_Form_Load;
        tabControl_Main.ResumeLayout(false);
        tabPage1.ResumeLayout(false);
        tableLayoutPanel1.ResumeLayout(false);
        tabPage2.ResumeLayout(false);
        tableLayoutPanel2.ResumeLayout(false);
        tabPage3.ResumeLayout(false);
        tableLayoutPanel3.ResumeLayout(false);
        tableLayoutPanel3.PerformLayout();
        tabPage4.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TabControl tabControl_Main;
    private TabPage tabPage1;
    private TableLayoutPanel tableLayoutPanel1;
    private TabPage tabPage2;
    private ListView listView_Swap_android;
    private Button btn_Apply_android;
    private Button btn_Revert_android;
    private Button btn_Refresh_android;
    private Label lbl_notify_android;
    private TabPage tabPage3;
    private TableLayoutPanel tableLayoutPanel2;
    private Button btn_Apply_webgl;
    private Button btn_Revert_webgl;
    private Button btn_Refresh_webgl;
    private Label lbl_notify_webgl;
    private ListView listView_Swap_webgl;
    private TableLayoutPanel tableLayoutPanel3;
    private Button btn_sccache_config;
    private TabPage tabPage4;
    private ComboBox combo_cacheType_android;
    private ComboBox combo_cacheType_webgl;
    private Button btn_ccache_config;
    private RichTextBox lbl_info;
    private LinkLabel link_ccache;
    private LinkLabel link_sccache;
}
