using STL_ACT_1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scheduler
{
  internal class Scheduler
  {
    public int GlobalTime { get; set; }
    private int total { get; set; }

    public Queue<Process> New { get; set; }
    public Queue<Process> Ready { get; set; }
    public Process Running { get; set; }
    public Queue<Process> Blocked { get; set; }
    public Queue<Process> Exit { get; set; }

    private static readonly Random r = new Random();
    private readonly int MEMORY_LIMIT = 5;

    private bool wasInterru;
    private bool wasBlocked;

    private MainWindow mW;

    public Scheduler(MainWindow mW)
    {
      New = new Queue<Process>();
      Ready = new Queue<Process>();
      Running = new Process();
      Blocked = new Queue<Process>();
      Exit = new Queue<Process>();

      this.mW = mW;
    }

    public async 
    Task
StartProcessing()
    {
      Admit();
      while(Ready.Count > 0 || Blocked.Count > 0) {
        Admit();
        Dispatch();

        await ExecuteRunning().ConfigureAwait(true);

        if(!wasBlocked && Running.State == 3) {
          Terminate();
        }
        wasInterru = false;
        wasBlocked = false;

        // --------- WINDOW ----------- //
        mW.UpdateLabels(new Process());
      }
    }

    public void Admit()
    {
      int processRunning = 0;
      if(Running.State == 3)
        processRunning = 1;

      while(New.Count > 0 && Ready.Count + Blocked.Count + processRunning < MEMORY_LIMIT) {
        Process p = New.Dequeue();
        p.State = 2;
        p.tEsp = 0;
        p.tLle = GlobalTime;
        Ready.Enqueue(p);
        mW.tblReady.Items.Add(p);
      }
    }

    public void Dispatch()
    {
      if(Ready.Count > 0) {
        Running = Ready.Dequeue();
        Running.State = 3;

        if(Running.tResp == -1)
          Running.tResp = GlobalTime - Running.tLle;

        mW.tblReady.Items.Remove(Running);
      } else {
        Running = new Process();
      }
    }

    public void Interrupt()
    {
      Blocked.Enqueue(Running);
      Running.State = 4;
    }

    public void Deinterrupt()
    {
      var p = Blocked.Dequeue();
      p.State = 2;
      Ready.Enqueue(p);
    }

    public void Terminate()
    {
      Running.tFin = GlobalTime;
      Running.tRet = Running.tEsp + Running.tTra;
      Exit.Enqueue(Running);
      Running.State = 5;
      // --------- WINDOW ----------- //
      mW.tblTerminated.Items.Add(Running);
    }

    private async Task ExecuteRunning()
    {
      if(Running.State == 3) {
        while(Running.tTra < Running.TME) {
          // Stops for a second
          await Task.Delay(1000).ConfigureAwait(true);
          // Increase time for running processes
          Running.tTra++;
          // Increase time for all processes
          IncreaseTime();
          // Update process remaining time
          Running.tRest = Running.TME - Running.tTra;

          // --------- WINDOW ----------- //
          mW.UpdateLabels(Running);
          mW.UpdateTable(Blocked, mW.tblBlocked);
          // ---------------------------- //

          await WasKeyPressed().ConfigureAwait(true);

          if(wasBlocked || wasInterru) { return; }
        }
      } else {
        // Stops for a second
        await Task.Delay(1000).ConfigureAwait(true);
        // Increase time for all processes
        IncreaseTime();

        await WasKeyPressed().ConfigureAwait(true);
        
        // --------- WINDOW ----------- //
        mW.UpdateTable(Blocked, mW.tblBlocked);
      }
    }

    private void IncreaseTime()
    {
      // Increase Global Time
      GlobalTime++;
      // Icrease waiting time to all ready processes 
      foreach(Process p in Ready) {
        p.tEsp++;
      }
      // Icrease blocked time to all blocked processes 
      bool DeInterrupt = false;
      foreach(Process p in Blocked) {
        p.tEsp++;
        p.tBloRes = 8 - p.tBlo++;
        if(p.tBlo >= 8) {
          DeInterrupt = true;
          p.tBlo = 0;
        }
      }
      if(DeInterrupt) {
        mW.tblBlocked.Items.Remove(Blocked.Peek());
        mW.tblReady.Items.Add(Blocked.Peek());
        Deinterrupt();
      }
    }

    public void CreateProcesses(int totalProcesses)
    {
      this.total = totalProcesses;
      for(int id = 1; id <= totalProcesses; id++) {
        CreateProcess(id);
      }
    }

    private void CreateProcess(int ID)
    {
      // Random values for the process
      int TME = r.Next(8, 18);
      int num1 = r.Next(0, 100);
      int opeIdx = r.Next(0, 5);
      int num2 = r.Next(0, 100);

      if(opeIdx == 3 || opeIdx == 4) { num2++; }
      var Ope = new Operation(num1, opeIdx, num2);
      New.Enqueue(new Process(ID, TME, Ope));
    }

    private async Task WasKeyPressed()
    {
      switch(mW.KeyPressed) {
        case "I":
          wasBlocked = true;
          Interrupt();
          break;
        case "E":
          Running.Result = "ERROR";
          wasInterru = true;
          break;
        case "P":
          while(mW.KeyPressed != "C") {
            await Task.Delay(1000).ConfigureAwait(true);
          }
          break;
        case "B":
          var myBCP = new BCP(this);
          myBCP.ShowDialog();
          break;
        case "N":
          CreateProcess(++total);
          break;
      }
      mW.KeyPressed = "";
    }
  }
}
