using System;
using System.Collections.Generic;

namespace STL_ACT_1
{
  class Shceduler
  {
    public Queue<Process> New { get; set; }
    public Queue<Process> Ready { get; set; }
    public Process Running { get; set; }
    public Stack<Process> Terminated { get; set; }

    public int TotalProcesses { get; set; }
    public int TotalBatches { get; set; }

    private static readonly Random r = new Random();
    public static readonly int BATCH_SIZE = 5;

    public Shceduler(int totalProcesses)
    {
      New = new Queue<Process>();
      Ready = new Queue<Process>();
      Terminated = new Stack<Process>();

      TotalProcesses = totalProcesses;
      TotalBatches = 1;
    }

    public void CreateBatches()
    {
      /******* ADD BATCH *******/
      for (int i = 1, id = 1; id <= TotalProcesses; i++, id++) {
        if (i == BATCH_SIZE) {
          TotalBatches++;
          i = 1;
        }
        /******* ADD PROCESS TO BATCH *******/
        CreateProcess(id);
      }
    }

    private void CreateProcess(int ID)
    {
      int TME = r.Next(8, 18);
      int num1 = r.Next(0, 100);
      int opeIdx = r.Next(0, 5);
      int num2 = r.Next(0, 100);
      if (opeIdx == 3) { num2++; }
      var Ope = new Operation(num1, opeIdx, num2);
      New.Enqueue(new Process(ID, TME, Ope));
    }

    public void MoveProcessToBatch()
    {
      int cont = 0;
      while (cont < BATCH_SIZE && New.Count != 0) {
        Process p = New.Dequeue();
        Ready.Enqueue(p);
        cont++;
      }
    }
  }
}
