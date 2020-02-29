using System;
using System.Threading.Tasks;
using System.Windows;

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

        tblTerminated.Items.Clear();
        tblBlocked.Items.Clear();
        tblReady.Items.Clear();
        tblTimes.Items.Clear();
        tblNew.Items.Clear();
        ToggleFields(false);

        // ---------- SCHEDULER ---------- //
        schedule = new Shceduler();
        schedule.CreateProcesses(totalProcesses);
        schedule.StartProcessing(this);
        // ------------------------------- //

        ToggleFields(true);
      }
    }

    internal void UpdateRunnigLabels(Process p)
    {
      lblNumPro.Content = p.ID;
      lblTME_PE.Content = p.TME;
      lblOpe_PE.Content = p.Ope;
      lblTieTra.Content = p.tTra;
      lblTieRest.Content = p.tRest;
    }

    internal void UpdateNewTable()
    {
      tblNew.Items.Clear();
      foreach (Process p in schedule.New) {
        tblNew.Items.Add(p);
      }
    }

    internal void UpdateReadyTable()
    {
      tblReady.Items.Clear();
      foreach (Process p in schedule.Ready) {
        p.tEsp++;
        tblReady.Items.Add(p);
      }
    }

    internal void UpdateBlockedTable()
    {
      if (schedule.Blocked.Count > 0) {
        tblBlocked.Items.Clear();
        bool DeInterrupt = false;
        foreach (Process p in schedule.Blocked) {
          if (++p.tBlo > 8) {
            DeInterrupt = true;
          } else {
            tblBlocked.Items.Add(p);
          }
        }
        if (DeInterrupt) {
          schedule.Deinterrupt();
          //UpdateReadyTable();
        }
      }
    }

    internal void UpdateTimesTable()
    {
      foreach (Process p in schedule.Terminated) {
        tblTimes.Items.Add(p);
      }
    }

    private void ToggleFields(bool state)
    {
      txtBoxTotalProc.IsEnabled = state;
      bttnStart.IsEnabled = state;
    }

    private void TeclaPresionada(object sender, System.Windows.Input.KeyEventArgs e)
    {
      KeyPressed = e.Key.ToString();
    }
  }
}
