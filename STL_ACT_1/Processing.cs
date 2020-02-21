using System;
using System.Collections.Generic;

namespace STL_ACT_1
{
  class Processing
  {
    public Queue<Process> Processes { get; set; }
    public Queue<Process> CurrBatch { get; set; }
    public Stack<Process> ProcCompleted { get; set; }
    public int TotalProcesses { get; set; }
    public int TotalBatches { get; set; }
    
    private static readonly Random r = new Random();
    private static readonly int BATCH_SIZE = 5;

    public Processing(int totalProcesses)
    {
      Processes = new Queue<Process>();
      CurrBatch = new Queue<Process>();
      ProcCompleted = new Stack<Process>();

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

    private void CreateProcess(int id)
    {
      // Random values
      int TME = r.Next(3, 10); // (8, 18)
      int num1 = r.Next(0, 100);
      int opeIdx = r.Next(0, 5);
      int num2 = r.Next(0, 100);
      Processes.Enqueue(new Process(id, TME, num1, opeIdx, num2));
    }

    public void MoveProcessToBatch()
    {
      int cont = 0;
      while (cont < BATCH_SIZE && Processes.Count != 0) {
        Process p = Processes.Dequeue();
        CurrBatch.Enqueue(p);
        cont++;
      }
    }
  }
}
