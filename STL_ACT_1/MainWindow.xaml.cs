using Scheduler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace STL_ACT_1
{
  public partial class MainWindow : Window
  {
    private Scheduler.Scheduler schedule;
    internal string KeyPressed;

    public MainWindow()
    {
      InitializeComponent();
    }

    private void ButtonStart_Click(object sender, RoutedEventArgs e)
    {
      int totalProcesses = (int)txtBoxTotalProc.Value;

      if (totalProcesses > 0) {
        ClearWindow();
        EnableFields(false);
        // ---------- SCHEDULER ---------- //
        schedule = new Scheduler.Scheduler(this);
        schedule.CreateProcesses(totalProcesses);
        schedule.StartProcessing();
        // ------------------------------- //
        EnableFields(true);
      }
    }

    internal void UpdateLabels(Process p)
    {
      lblNumPro.Content = p.ID;
      lblTME_PE.Content = p.TME;
      lblOpe_PE.Content = p.Ope;
      lblTieTra.Content = p.tTra;
      lblTieRes.Content = p.tRest;

      lblProRes.Content = schedule.New.Count;
      lblGloTime.Content = schedule.GlobalTime;
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

    private void ClearWindow()
    {
      tblTerminated.Items.Clear();
      tblBlocked.Items.Clear();
      tblReady.Items.Clear();

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
