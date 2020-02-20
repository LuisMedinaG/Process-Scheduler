using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace STL_ACT_1
{
  public partial class MainWindow : Window
  {
    private MultiProcessing batchs;
    private string keyPressed;
    private int globTime;
    private bool isInterr = false;
    private bool isPaused = false;
    private bool isError = false;
    private static Random r = new Random();

    public MainWindow()
    {
      globTime = 0;
      keyPressed = "";

      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      batchs = new MultiProcessing((int)txtBoxTotalProc.Value);

      if (batchs.total > 0) {
        txtBoxTotalProc.Text = batchs.total.ToString();
        tblProFin.Items.Clear();
        ToggleFields(true);
        CreateBatchs();
      }
    }

    private void CreateBatchs()
    {
      int batchSze = 5;
      /******* ADD BATCH *******/
      for (int i = 1, j = 1; i <= batchs.total; i++, j++) {
        if (j == batchSze) {
          batchs.idx++;
          j = 1;
        }
        /******* ADD PROCESS TO BATCH *******/
        CreateProcess(i, batchs.processes);
        
      }
      StartProcessing();
    }

    private static void CreateProcess(int i, Queue<Process> processes)
    {
      int tme = r.Next(5, 6);
      int num1 = r.Next(0, 100);
      int opeIdx = r.Next(0, 5);
      int num2 = r.Next(0, 100);
      processes.Enqueue(new Process(i, tme, num1, opeIdx, num2));
    }

    private async void StartProcessing()
    {
      int remainingBatchs = batchs.idx;
      Process currProc;

      ToggleFields(false);
      while (remainingBatchs > 0) {
        lblLotPen.Content = remainingBatchs--.ToString();

        /* ----- LOTE EN EJECUCION ----- */
        batchs.MoveProssToCurrBatch();
        AddToCurrBacthTbl();

        for (int i = 0; i < 5; i++) {
          if (batchs.prosCurrBatch.Count != 0) {
            currProc = batchs.prosCurrBatch.Dequeue();

            /* ----- PROCESO EN EJECUCION ----- */
            lblNumPro.Content = currProc.ProcessId;
            lblTME_PE.Content = currProc.TME;
            lblOpe_PE.Content = currProc.Operation;

            await DoProcess(currProc);

            if (isInterr) {
              batchs.prosCurrBatch.Enqueue(currProc);
              //AddToCurrBacthTbl();
              tblLotEje.Items.Remove(currProc);
              tblLotEje.Items.Add(currProc);
              isInterr = false;
              i--;
            } else {
              /* ----- PROCESOS TERMINADOS ----- */
              tblLotEje.Items.Remove(currProc);
              tblProFin.Items.Add(currProc);
            }
          }
        }
        tblProFin.Items.Add(new Process());
      }
      ToggleFields(false);
      lblLotPen.Content = "0";
      MessageBox.Show("Fin de procesos.", "Info");
    }

    private void AddToCurrBacthTbl()
    {
      tblLotEje.Items.Clear();
      foreach (Process p in batchs.prosCurrBatch) {
        tblLotEje.Items.Add(p);
      }
    }

    private async Task DoProcess(Process p)
    {
      bool MessageShowed = true;
      int remainingTime = p.TME;
      int elapsedTime = 0;
      while (remainingTime > 0) {
        await Task.Delay(1000);
        UpdateFlags();
        if (isPaused) {
          if (MessageShowed) {
            MessageBox.Show("Pausa");
            MessageShowed = false;
          }
          keyPressed = "";
        } else if (isInterr) {
          MessageBox.Show("Interrupcion.");
          keyPressed = "";
          break;
        } else if (isError) {
          MessageBox.Show("Error.");
          isError = false;
          keyPressed = "";
          break;
        } else {
          lblTieTra.Content = (elapsedTime++).ToString() + " sec";
          lblTieRes.Content = (remainingTime--).ToString() + " sec";
          lblContGlo.Content = (globTime++).ToString();
        }
      }
    }

    private void UpdateFlags()
    {
      switch (keyPressed) {
        case "I": isInterr = true; break;
        case "E": isError = true; break;
        case "P": isPaused = true; break;
        case "C": isPaused = false; break;
      }
    }

    private void ToggleFields(bool state)
    {
      txtBoxTotalProc.IsEnabled = !state;
      bttnStart.IsEnabled = !state;
    }

    private void TeclaPresionada(object sender, System.Windows.Input.KeyEventArgs e)
    {
      keyPressed = e.Key.ToString();
    }
  }
}
