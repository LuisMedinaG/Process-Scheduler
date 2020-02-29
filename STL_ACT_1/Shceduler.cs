using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STL_ACT_1
{
  class Shceduler
  {
    public Queue<Process> New { get; set; }
    public Queue<Process> Ready { get; set; }
    public Process Running { get; set; }
    public Queue<Process> Blocked { get; set; }
    public Stack<Process> Terminated { get; set; }

    private static readonly Random r = new Random();
    private static readonly int MEMORY_SIZE = 5;
    private bool wasInterrupted;
    private bool wasBlocked;
    private int globTime;

    public Shceduler()
    {
      New = new Queue<Process>();
      Ready = new Queue<Process>();
      Running = new Process();
      Blocked = new Queue<Process>();
      Terminated = new Stack<Process>();
    }

    public async void StartProcessing(MainWindow mainWindow)
    {
      Admit();
      while (Ready.Count > 0) {
        Admit();
        Dispatch();

        // Set the variables
        wasInterrupted = false;
        wasBlocked = false;

        // --------- WINDOW ----------- //
        mainWindow.UpdateNewTable();
        mainWindow.UpdateReadyTable();
        mainWindow.UpdateRunnigLabels(Running);
        // ---------------------------- //

        // Time when is admited
        Running.tLle = globTime;
        await ExecuteRunning(mainWindow);

        if (!wasBlocked) {
          Running.tFin = globTime;
          Running.tRet = Running.tFin - Running.tLle;
          // --------- WINDOW ----------- //
          mainWindow.tblTerminated.Items.Add(Running);
          mainWindow.lblProRes.Content = New.Count.ToString();
          // ---------------------------- //
          Exit();
        }
      }
      mainWindow.UpdateRunnigLabels(new Process());
      mainWindow.UpdateTimesTable();
    }

    /********************************************************/
    private async Task ExecuteRunning(MainWindow mainWindow)
    {
      // Time is attended for the first time
      Running.tResp = globTime;
      while (Running.tTra < Running.TME) {
        // Stops for a second
        await Task.Delay(1000);
        // Checks that there's no key pressed
        await WasKeyPressed(mainWindow);
        if (wasBlocked || wasInterrupted) {
          return;
        } else {
          // Increases current time and Decrease remainig time
          Running.tRest = Running.TME - Running.tTra++;
          // Update bloked processes table
          mainWindow.UpdateBlockedTable();
          // Update runnig process labels
          mainWindow.UpdateRunnigLabels(Running);
          // Update label and increas global time
          mainWindow.lblGlobTime.Content = globTime++;
        }
      }
    }

    /********************************************************/
    private async Task WasKeyPressed(MainWindow mainWindow)
    {
      switch (mainWindow.KeyPressed) {
        case "I":
          mainWindow.tblBlocked.Items.Add(Running);
          wasBlocked = true;
          Interrupt();
          break;
        case "E":
          Running.OpeResult = "ERROR!";
          wasInterrupted = true;
          break;
        case "P":
          while (mainWindow.KeyPressed != "C") {
            await Task.Delay(1000);
          }
          break;
      }
      mainWindow.KeyPressed = "";
    }

    public void CreateProcesses(int totalProcesses)
    {
      for (int id = 1; id <= totalProcesses; id++) {
        CreateProcess(id);
      }
    }

    private void CreateProcess(int ID)
    {
      // Random values for the process
      int TME = r.Next(8, 8/*8*/);
      int num1 = r.Next(0, 100);
      int opeIdx = r.Next(0, 5);
      int num2 = r.Next(0, 100);

      if (opeIdx == 3 || opeIdx == 4) { num2++; }
      var Ope = new Operation(num1, opeIdx, num2);
      New.Enqueue(new Process(ID, TME, Ope));
    }

    public void Admit()
    {
      while (New.Count > 0 && (Ready.Count + Blocked.Count) < MEMORY_SIZE) {
        Ready.Enqueue(New.Dequeue());
      }
    }

    public void Dispatch()
    {
      if (Ready.Count > 0) {
        Running = Ready.Dequeue();
      }
    }

    public void Interrupt()
    {
      Blocked.Enqueue(Running);
      if (Ready.Count > 0) {
        Running = Ready.Dequeue();
      }
    }

    public void Deinterrupt()
    {
      if (Blocked.Count > 0) {
        Ready.Enqueue(Blocked.Dequeue());
      }
    }

    public void Exit()
    {
      Terminated.Push(Running);
    }
  }
}
