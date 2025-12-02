using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnityCompileCacheGUI_Winform;

sealed partial class UnityCompileCacheGUI_Winform_Form : Form
{
    const string VERSION = "v0.0.1";

    private readonly Dictionary<E_TAB, List<UnityUpdateItem>> _dic = new Dictionary<E_TAB, List<UnityUpdateItem>>(capacity: 2);
    private CComboItem[] _comboItems = new CComboItem[2] {
        new CComboItem { Name = "ccache", Id = 1 , WrapperType = E_UNITYCOMPILECACHE_TYPE.UnityCompileCache_ccache},
        new CComboItem { Name = "sccache", Id = 2 , WrapperType = E_UNITYCOMPILECACHE_TYPE.UnityCompileCache_sccache},
    };

    private List<UnityUpdateItem> _currentItems;
    private E_TAB _currentTab;
    private ListView _currentListView;
    private CComboItem _currSelectedCombo;


    public UnityCompileCacheGUI_Winform_Form()
    {
        InitializeComponent();
        lbl_info.Text = @$"
NF.Tool.UnityCompileCache {VERSION}

github: https://github.com/netpyoung/NF.Tool.UnityCompileCache/

- ccache  wrapper version: {EmbeddedInfo.EmbeddedFile.ProductVersion_ccache}
- sccache wrapper version: {EmbeddedInfo.EmbeddedFile.ProductVersion_sccache}

";
        _dic[E_TAB.ANDROID] = new List<UnityUpdateItem>();
        _dic[E_TAB.WEBGL] = new List<UnityUpdateItem>();
        _currentItems = _dic[E_TAB.ANDROID];
        _currentTab = E_TAB.ANDROID;
        _currentListView = listView_Swap_android;
        _currSelectedCombo = _comboItems[0];
    }

    private void UnityCompileCacheGUI_Winform_Form_Load(object sender, EventArgs e)
    {
        listView_Swap_android.View = View.Details;
        listView_Swap_android.CheckBoxes = true;
        listView_Swap_android.OwnerDraw = true;
        listView_Swap_android.FullRowSelect = true;
        listView_Swap_android.EnableCopyOnCtrlC();

        listView_Swap_android.Columns.Add(new ColumnHeader { Text = string.Empty, Width = 25 });
        listView_Swap_android.Columns.Add(new ColumnHeader { Text = nameof(UnityUpdateItem.UnityVersion), Width = 100 });
        listView_Swap_android.Columns.Add(new ColumnHeader { Text = "mode", Width = 100 });
        listView_Swap_android.Columns.Add(new ColumnHeader { Text = nameof(UnityUpdateItem.Dir), Width = 100 });
        listView_Swap_android.EnableLastColumnFill();

        combo_cacheType_android.DataSource = _comboItems;
        combo_cacheType_android.DisplayMember = nameof(CComboItem.Name);
        combo_cacheType_android.ValueMember = nameof(CComboItem.Id);

        listView_Swap_webgl.View = View.Details;
        listView_Swap_webgl.CheckBoxes = true;
        listView_Swap_webgl.OwnerDraw = true;
        listView_Swap_webgl.FullRowSelect = true;
        listView_Swap_webgl.EnableCopyOnCtrlC();

        listView_Swap_webgl.Columns.Add(new ColumnHeader { Text = string.Empty, Width = 25 });
        listView_Swap_webgl.Columns.Add(new ColumnHeader { Text = nameof(UnityUpdateItem.UnityVersion), Width = 100 });
        listView_Swap_webgl.Columns.Add(new ColumnHeader { Text = "mode", Width = 100 });
        listView_Swap_webgl.Columns.Add(new ColumnHeader { Text = nameof(UnityUpdateItem.Dir), Width = 100 });
        listView_Swap_webgl.EnableLastColumnFill();

        combo_cacheType_webgl.DataSource = _comboItems;
        combo_cacheType_webgl.DisplayMember = nameof(CComboItem.Name);
        combo_cacheType_webgl.ValueMember = nameof(CComboItem.Id);
    }

    private static void UpdateLabel(Label label)
    {
        if (Utils.IsRunningAsAdministrator())
        {
            label.Visible = false;
        }
        else
        {
            label.Visible = true;
            label.Text = "Need to run as administrator";
            label.BackColor = Color.Red;
            label.ForeColor = Color.White;
        }
    }

    private void tabControl_Main_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectedIndexChanged();
    }

    #region Tab Android & WebGL
    private void listView_Swap_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
    {
        if (e.ColumnIndex != 0)
        {
            e.DrawDefault = true;
            return;
        }

        e.DrawBackground();

        bool isChecked = Convert.ToBoolean(e.Header!.Tag);
        if (isChecked)
        {
            CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(e.Bounds.Left + 4, e.Bounds.Top + 4), System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal);
        }
        else
        {
            CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(e.Bounds.Left + 4, e.Bounds.Top + 4), System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
        }
    }

    private void listView_Swap_DrawItem(object sender, DrawListViewItemEventArgs e)
    {
        e.DrawDefault = true;
    }

    private void listView_Swap_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
    {
        e.DrawDefault = true;
    }

    private void listView_Swap_ColumnClick(object sender, ColumnClickEventArgs e)
    {
        if (e.Column != 0)
        {
            return;
        }

        ListView listView = (ListView)sender;

        bool isChecked = Convert.ToBoolean(listView.Columns[e.Column].Tag);
        listView.Columns[e.Column].Tag = !isChecked;

        foreach (ListViewItem item in listView.Items)
        {
            item.Checked = !isChecked;
        }

        listView.Invalidate();
    }

    private async void btn_Refresh_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        button.Enabled = false;

        try
        {
            await RefreshList();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        finally
        {
            button.Enabled = true;
        }
    }

    private async void btn_Apply_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        button.Enabled = false;

        try
        {
            if (TryApply())
            {
                await RefreshList();
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            UpdateLabel(lbl_notify_android);
            UpdateLabel(lbl_notify_webgl);
            MessageBox.Show(ex.ToString());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        finally
        {
            button.Enabled = true;
        }
    }


    private async void btn_Revert_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        button.Enabled = false;

        try
        {
            bool isSuccess = await TryRevert();
            if (isSuccess)
            {
                await RefreshList();
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            UpdateLabel(lbl_notify_android);
            UpdateLabel(lbl_notify_webgl);
            MessageBox.Show(ex.ToString());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        finally
        {
            button.Enabled = true;
        }
    }

    private async void combo_cacheType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ComboBox x = (ComboBox)sender;
        _currSelectedCombo = (CComboItem)x.SelectedItem!;
    }
    #endregion // Tab Android & WebGL


    #region Tab Etc
    private void btn_ccache_config_Click(object sender, EventArgs e)
    {
        string defaultConfigPath = "%LOCALAPPDATA%\\ccache\\ccache.conf";
        string path = Environment.ExpandEnvironmentVariables(defaultConfigPath);
        DialogResult x = MessageBox.Show($"Edit?\n{path}", defaultConfigPath, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (x != DialogResult.Yes)
        {
            return;
        }

        if (!File.Exists(path))
        {
            string dir = Path.GetDirectoryName(path)!;
            Directory.CreateDirectory(dir);
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(@"
# ref: https://ccache.dev/manual/latest.html#_location_of_the_configuration_file
# ref: https://ccache.dev/manual/latest.html#_configuration_options
#
#
# depend_mode = true
# direct_mode = true
# hard_link = true
# run_second_cpp = true
# sloppiness = file_stat_matches,pch_defines,file_macro,time_macros,include_file_mtime,include_file_ctime,ivfsoverlay
#
# ref: https://www.distcc.org/
# prefix_command = ""distcc"" 
#
#
");
            }
        }

        Process.Start("notepad.exe", path);
    }

    private void btn_sccache_config_Click(object sender, EventArgs e)
    {
        string defaultConfigPath = "%APPDATA%\\Mozilla\\sccache\\config\\config";
        string path = Environment.ExpandEnvironmentVariables(defaultConfigPath);
        DialogResult x = MessageBox.Show($"Edit?\n{path}", defaultConfigPath, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (x != DialogResult.Yes)
        {
            return;
        }

        if (!File.Exists(path))
        {
            string dir = Path.GetDirectoryName(path)!;
            Directory.CreateDirectory(dir);
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(@"
# ref: https://github.com/mozilla/sccache/blob/main/docs/Configuration.md
#
#
# [cache.redis]
# endpoint = ""redis://127.0.0.1:6379""
");
            }
        }

        Process.Start("notepad.exe", path);
    }
    #endregion // Tab Etc

    public static async Task<List<UnityUpdateItem>> GetItems(E_UNITYCOMPILECACHE_TYPE unityCompileCacheType, Func<string, string?> funcFindClang)
    {
        // TODO(pyoung): updatable logic


        string[] editorDirs = Directory.GetDirectories("C:\\Program Files\\Unity\\Hub\\Editor");
        List<UnityUpdateItem> ret = new List<UnityUpdateItem>(editorDirs.Length);
        foreach (string editorDir in editorDirs)
        {
            string unityVersion = Path.GetFileName(editorDir);
            string exeDir = string.Empty;
            string unityCompileCache_Mode = "None";
            string unityCompileCache_Name = string.Empty;
            bool hasUnityCompileCache = false;

            string? clangBinPath = await Task.Run(() => funcFindClang(editorDir));
            if (clangBinPath == null)
            {
                ret.Add(new UnityUpdateItem
                {
                    UnityVersion = unityVersion,
                    UnityCompileCache_Mode = unityCompileCache_Mode,
                    UnityCompileCache_Name = unityCompileCache_Name,
                    Dir = exeDir,
                    HasUnityCompileCache = hasUnityCompileCache
                });
                continue;
            }

            exeDir = Path.GetDirectoryName(clangBinPath)!;

            FileVersionInfo? binFvi = FileVersionInfo.GetVersionInfo(clangBinPath)!;

            if (binFvi != null)
            {
                if (!string.IsNullOrEmpty(binFvi.ProductName))
                {
                    unityCompileCache_Name = binFvi.ProductName;

                    if (unityCompileCache_Name.StartsWith("UnityCompileCache_", StringComparison.OrdinalIgnoreCase))
                    {
                        hasUnityCompileCache = true;
                        unityCompileCache_Mode = unityCompileCache_Name.Substring("UnityCompileCache_".Length);
                    }
                }
            }

            ret.Add(new UnityUpdateItem
            {
                UnityVersion = unityVersion,
                UnityCompileCache_Mode = unityCompileCache_Mode,
                UnityCompileCache_Name = unityCompileCache_Name,
                Dir = exeDir,
                HasUnityCompileCache = hasUnityCompileCache
            });
        }
        return ret;
    }

    private async Task RefreshList()
    {
        ListView listView = _currentListView;
        listView.Items.Clear();

        List<UnityUpdateItem> items;
        switch (_currentTab)
        {
            case E_TAB.ANDROID:
                items = await GetItems(_currSelectedCombo.WrapperType, ClangFinder.FindClangFromNDK);
                break;
            case E_TAB.WEBGL:
                items = await GetItems(_currSelectedCombo.WrapperType, ClangFinder.FindClangFromEmscripten);
                break;
            default:
                return;
        }

        _currentItems.Clear();
        _currentItems.AddRange(items);

        foreach (UnityUpdateItem x in items)
        {
            ListViewItem item = new ListViewItem();
            item.Text = string.Empty;
            item.SubItems.Add(x.UnityVersion);
            item.SubItems.Add(x.UnityCompileCache_Mode);
            item.SubItems.Add(x.Dir);

            listView.Items.Add(item);
        }
    }

    private bool TryApply()
    {
        // TODO: confirm
        //if (!Utils.IsRunningAsAdministrator())
        //{
        //    MessageBox.Show("You need to run this program as administrator", "Need to run as administrator", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    return false;
        //}

        List<UnityUpdateItem> currentItems = _currentItems;
        ListView currentListView = _currentListView;

        bool hasUpdate = false;
        foreach (ListViewItem item in currentListView.Items)
        {
            if (item.Checked)
            {
                string version = item.SubItems[1].Text;
                UnityUpdateItem x = currentItems.First(x => x.UnityVersion == version);
                if (string.IsNullOrEmpty(x.Dir))
                {
                    continue;
                }

                hasUpdate = true;
                break;
            }
        }

        if (!hasUpdate)
        {
            MessageBox.Show("There is nothing updatable.");
            return false;
        }

        string exeName = $"{_currSelectedCombo.WrapperType.ToString()}.exe";
        string tempFullPath = Utils.ExtractResourceToTempFilePath(exeName);

        foreach (ListViewItem item in currentListView.Items)
        {
            if (!item.Checked)
            {
                continue;
            }

            string version = item.SubItems[1].Text;
            UnityUpdateItem x = currentItems.First(x => x.UnityVersion == version);
            if (string.IsNullOrEmpty(x.Dir))
            {
                continue;
            }

            Utils.BackupAndUpdate(tempFullPath, Path.Combine(x.Dir, "clang.exe"));
            Utils.BackupAndUpdate(tempFullPath, Path.Combine(x.Dir, "clang++.exe"));
            Utils.BackupAndUpdate(tempFullPath, Path.Combine(x.Dir, "clang-cl.exe"));
        }

        return true;
    }

    async Task<bool> TryRevert()
    {
        //if (!Utils.IsRunningAsAdministrator())
        //{
        //    MessageBox.Show("You need to run this program as administrator", "Need to run as administrator", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    return false;
        //}

        List<UnityUpdateItem> items = _currentItems;
        ListView listView = _currentListView;

        List<UnityUpdateItem> willChangeItems = new List<UnityUpdateItem>(capacity: listView.Items.Count);
        foreach (ListViewItem item in listView.Items)
        {
            if (!item.Checked)
            {
                continue;
            }

            string version = item.SubItems[1].Text;
            UnityUpdateItem x = items.First(x => x.UnityVersion == version);
            if (string.IsNullOrEmpty(x.Dir))
            {
                continue;
            }

            if (!x.HasUnityCompileCache)
            {
                continue;
            }
            willChangeItems.Add(x);
        }

        if (willChangeItems.Count == 0)
        {
            MessageBox.Show("There is nothing to revert");
            return false;
        }

        foreach (UnityUpdateItem x in willChangeItems)
        {
            Utils.Revert(x.Dir, "clang.exe");
            Utils.Revert(x.Dir, "clang++.exe");
            Utils.Revert(x.Dir, "clang-cl.exe");
        }
        return true;
    }

    void SelectedIndexChanged()
    {
        E_TAB tab = (E_TAB)tabControl_Main.SelectedIndex;
        _currentTab = tab;
        switch (tab)
        {
            case E_TAB.ANDROID:
                _currentListView = listView_Swap_android;
                _currentItems = _dic[E_TAB.ANDROID];
                break;
            case E_TAB.WEBGL:
                _currentListView = listView_Swap_webgl;
                _currentItems = _dic[E_TAB.WEBGL];
                break;
            default:
                break;
        }
    }

    private void lbl_info_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = e.LinkText,
            UseShellExecute = true
        });
    }

    private void link_ccache_sccache_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        e.Link!.Visited = true;
        var x = (LinkLabel)sender;

        Process.Start(new ProcessStartInfo
        {
            FileName = x.Text,
            UseShellExecute = true
        });
    }
}