using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace STL_ACT_1
{
  public partial class MainWindow : Window
  {
    private Shceduler schedule;
    public string KeyPressed;

    public MainWindow()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      int totalProcesses = (int)txtBoxTotalProc.Value;

      if (totalProcesses > 0) {
        txtBoxTotalProc.Text = totalProcesses.ToString();
        ClearAll();
        EnableFields(false);
        // ---------- SCHEDULER ---------- //
        schedule = new Shceduler();
        schedule.CreateProcesses(totalProcesses);
        schedule.StartProcessing(this);
        // ------------------------------- //
        EnableFields(true);
      }
    }

    internal void UpdateLabels()
    {
      var p = schedule.Running;

      lblNumPro.Content = p.ID;
      lblTME_PE.Content = p.TME;
      lblOpe_PE.Content = p.Ope;
      lblTieTra.Content = p.tTra;
      lblTieRes.Content = p.tRest;

      lblProRes.Content = (schedule.New.Count + schedule.Ready.Count + schedule.Blocked.Count).ToString();
      lblGloTime.Content = schedule.GlobalTime.ToString();
    }

    internal void UpdateTableNew()
    {
      tblNew.Items.RemoveAt(tblNew.Items.Count);
    }

    internal void UpdateTable(Queue<Process> collection, DataGrid table)
    {
      table.Items.Clear();
      foreach (Process p in collection) {
        table.Items.Add(p);
      }
    }

    private void EnableFields(bool state)
    {
      txtBoxTotalProc.IsEnabled = state;
      bttnStart.IsEnabled = state;
    }

    internal void ClearAll()
    {
      tblTerminated.Items.Clear();
      tblBlocked.Items.Clear();
      tblReady.Items.Clear();
      tblTimes.Items.Clear();
      tblNew.Items.Clear();

      lblNumPro.Content = "";
      lblTME_PE.Content = "";
      lblOpe_PE.Content = "";
      lblTieTra.Content = "";
      lblTieRes.Content = "";
    }

    private void TeclaPresionada(object sender, System.Windows.Input.KeyEventArgs e)
    {
      KeyPressed = e.Key.ToString();
    }
  }
}
