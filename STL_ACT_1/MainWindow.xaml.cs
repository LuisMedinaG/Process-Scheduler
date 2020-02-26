using System.Threading.Tasks;
using System.Windows;

namespace STL_ACT_1
{
  public partial class MainWindow : Window
  {
    private Shceduler batches;
    private string keyPressed;
    private int globTime;

    private bool isInterruption = false;
    private bool isPaused = false;
    private bool isError = false;

    public MainWindow()
    {
      globTime = 0;
      keyPressed = "";

      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      int TotalProcesses = (int)txtBoxTotalProc.Value;
      batches = new Shceduler(TotalProcesses);

      if (TotalProcesses > 0) {
        txtBoxTotalProc.Text = TotalProcesses.ToString();
        tblProFin.Items.Clear(); // Borrar los procesos finales
        ToggleFields(false);
        // -------------------- //
        batches.CreateBatches();
        StartProcessing();
        // -------------------- //
        ToggleFields(true);
      }
    }

    private async void StartProcessing()
    {
      int remainingBatchs = batches.TotalBatches;
      Process currProc;

      while (remainingBatchs > 0) {
        lblLotPen.Content = (--remainingBatchs).ToString(); // WINDOW

        /* ----- LOTE EN EJECUCION ----- */
        batches.MoveProcessToBatch();
        AddProcToCurrBacthTbl(); // WINDOW

        for (int i = 0; i < batches.BATCH_SIZE; i++) {
          if (batches.Ready.Count != 0) {
            currProc = batches.Ready.Dequeue();

            /* ----- PROCESO EN EJECUCION ----- */
            lblNumPro.Content = currProc.ID; // WINDOW
            lblTME_PE.Content = currProc.TME; // WINDOW
            lblOpe_PE.Content = currProc.Ope; // WINDOW

            tblCurrBatch.Items.Remove(currProc);

            await DoProcess(currProc);

            if (isInterruption) {
              //MessageBox.Show("Interrupcion.");
              batches.Ready.Enqueue(currProc);
              tblCurrBatch.Items.Add(currProc);
              isInterruption = false;
              i--;
            } else if (isError) {
              //MessageBox.Show("Error.");
              currProc.OpeResult = "ERROR!";
              tblProFin.Items.Add(currProc);
              isError = false;
            } else {
              /* ----- AGRGAR A PROCESOS TERMINADOS ----- */
              tblProFin.Items.Add(currProc);
            }
          }
        }
        tblProFin.Items.Add(new Process());
      }
      MessageBox.Show("Fin de procesos.");
    }

    private void AddProcToCurrBacthTbl()
    {
      tblCurrBatch.Items.Clear();
      foreach (Process p in batches.Ready) {
        tblCurrBatch.Items.Add(p);
      }
    }

    private async Task DoProcess(Process p)
    {
      bool MessageShowed = false;
      while (p.RemainigTime > 0) {
        if (isInterruption || isError) {
          break;
        }
        if (isPaused) {
          if (!MessageShowed) {
            //MessageBox.Show("Pausa");
            MessageShowed = true;
          }
        } else {
          lblTieTra.Content = $"{p.TME-p.RemainigTime} sec";
          lblTieRes.Content = $"{p.RemainigTime--} sec";
          lblGlobTime.Content = $"{globTime++} sec";
        }
        await Task.Delay(1000);
        UpdateFlags();
      }
    }

    private void UpdateFlags()
    {
      switch (keyPressed) {
        case "I": isInterruption = true; break;
        case "E": isError = true; break;
        case "P": isPaused = true; break;
        case "C": isPaused = false; break;
      }
      keyPressed = "";
    }

    private void ToggleFields(bool state)
    {
      txtBoxTotalProc.IsEnabled = state;
      bttnStart.IsEnabled = state;
    }

    private void TeclaPresionada(object sender, System.Windows.Input.KeyEventArgs e)
    {
      keyPressed = e.Key.ToString();
    }
  }
}
