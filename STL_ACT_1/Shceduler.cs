using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace STL_ACT_1
{
  class Shceduler
  {
    public int GlobalTime { get; set; }
    public Queue<Process> New { get; set; }
    public Queue<Process> Ready { get; set; }
    public Process Running { get; set; }
    public Queue<Process> Blocked { get; set; }
    public Queue<Process> Terminated { get; set; }

    private static readonly Random r = new Random();
    private static readonly int MEMORY_SIZE = 5;
    private bool wasInterru;
    private bool wasBlocked;

    public Shceduler()
    {
      GlobalTime = 0;
      New = new Queue<Process>();
      Ready = new Queue<Process>();
      Running = new Process();
      Blocked = new Queue<Process>();
      Terminated = new Queue<Process>();
    }

    public async void StartProcessing(MainWindow mainWindow)
    {
      Admit();
      while (Ready.Count > 0) {
        // Reset variables
        wasInterru = false;
        wasBlocked = false;

        Admit();
        Dispatch();

        // --------- WINDOW ----------- //
        mainWindow.UpdateLabels();
        mainWindow.UpdateTable(New, mainWindow.tblNew); // TODO
        mainWindow.UpdateTable(Ready, mainWindow.tblReady);
        mainWindow.UpdateTable(Terminated, mainWindow.tblTerminated); // TODO
        // ---------------------------- //

        await ExecuteRunning(mainWindow);

        if (!wasBlocked) {
          Exit();
        } else if (Ready.Count == 0) {
          while (Blocked.Count > 0) {
            Deinterrupt();
          }
        }
      }
      mainWindow.UpdateTable(Terminated, mainWindow.tblTerminated); // TODO
      mainWindow.UpdateTable(Terminated, mainWindow.tblTimes);
    }

    private async Task ExecuteRunning(MainWindow mainWindow)
    {
      while (Running.tTra < Running.TME) {
        // Stops for a second
        await Task.Delay(1000);
        // Increase time for all processes
        IncreaseTime();
        // Update process remaining time
        Running.tRest = Running.TME - Running.tTra;

        await WasKeyPressed(mainWindow);

        // --------- WINDOW ----------- //
        mainWindow.UpdateLabels();
        mainWindow.UpdateTable(Blocked, mainWindow.tblBlocked);
        // ---------------------------- //

        if (wasBlocked || wasInterru) { return; }
      }
    }

    private void IncreaseTime()
    {
      // Increase Global Time
      GlobalTime++;
      // Increase Running Times
      Running.tTra++;
      // Icrease waiting time to all ready processes 
      foreach (Process p in Ready) {
        p.tEsp++;
      }
      // Icrease blocked time to all blocked processes 
      bool DeInterrupt = false;
      foreach (Process p in Blocked) {
        if (p.tBlo++ == 8) DeInterrupt = true;
      }
      if (DeInterrupt) Deinterrupt();
    }

    private async Task WasKeyPressed(MainWindow mainWindow)
    {
      switch (mainWindow.KeyPressed) {
        case "I":
          wasBlocked = true;
          Interrupt();
          break;
        case "E":
          Running.OpeResult = "ERROR!";
          wasInterru = true;
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
        Running.tLle = GlobalTime;
        Ready.Enqueue(New.Dequeue());
      }
    }

    public void Dispatch()
    {
      Running.tResp = GlobalTime;
      Running = Ready.Dequeue();
    }

    public void Interrupt()
    {
      Blocked.Enqueue(Running);
    }

    public void Deinterrupt()
    {
      Ready.Enqueue(Blocked.Dequeue());
    }

    public void Exit()
    {
      Running.tFin = GlobalTime;
      Running.tRet = Running.tFin - Running.tLle;
      Terminated.Enqueue(Running);
    }
  }
}
