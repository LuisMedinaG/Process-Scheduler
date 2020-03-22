using STL_ACT_1;
using System.Windows;

namespace Scheduler
{
  public partial class BCP : Window
  {
    internal BCP(Scheduler scheduler)
    {
      InitializeComponent();
      Process pCpy;
      tblBCP.Items.Clear();
      foreach(Process p in scheduler.New) {
        pCpy = new Process(p) {
          Result = " "
        };
        tblBCP.Items.Add(pCpy);
      }
      foreach(Process p in scheduler.Ready) {
        pCpy = new Process(p) {
          Result = " "
        };
        tblBCP.Items.Add(pCpy);
      }
      if(scheduler.Running.State == 3) {
        pCpy = new Process(scheduler.Running) {
          Result = " "
        };
        tblBCP.Items.Add(pCpy);
      }          
      foreach(Process p in scheduler.Blocked) {
        pCpy = new Process(p) {
          Result = " "
        };
        tblBCP.Items.Add(pCpy);
      }
      foreach(Process p in scheduler.Exit) {
        tblBCP.Items.Add(p);
      }
    }
  }
}
