using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UnityCompileCacheGUI_Winform;

static class ListViewExtensions
{
    public static void EnableCopyOnCtrlC(this ListView lv)
    {
        lv.KeyDown -= Lv_KeyDown;
        lv.KeyDown += Lv_KeyDown;
    }

    private static void Lv_KeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not ListView lv)
        {
            return;
        }

        if (e.Control && e.KeyCode == Keys.C)
        {
            CopySelectedItems(lv);
            e.SuppressKeyPress = true;
        }
    }

    private static void CopySelectedItems(ListView lv)
    {
        if (lv.SelectedItems.Count == 0)
        {
            return;
        }

        StringBuilder sb = new StringBuilder();
        foreach (ListViewItem item in lv.SelectedItems)
        {
            List<string> parts = new List<string>(item.SubItems.Count);
            foreach (ListViewItem.ListViewSubItem sub in item.SubItems)
            {
                parts.Add(sub.Text);
            }

            sb.AppendLine(string.Join("\t", parts));
        }

        Clipboard.SetText(sb.ToString());
    }

    public static void EnableLastColumnFill(this ListView listView)
    {
        listView.SizeChanged += (_, __) => listView.AdjustLastColumn();
        listView.AdjustLastColumn();
    }

    public static void AdjustLastColumn(this ListView listView)
    {
        if (listView.Columns.Count == 0)
        {
            return;
        }

        int total = listView.ClientSize.Width;
        for (int i = 0; i < listView.Columns.Count - 1; i++)
        {
            total -= listView.Columns[i].Width;
        }

        total = Math.Max(50, total);
        listView.Columns[listView.Columns.Count - 1].Width = total;
    }
}
