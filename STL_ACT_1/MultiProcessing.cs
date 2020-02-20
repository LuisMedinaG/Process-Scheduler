using System.Collections.Generic;

namespace STL_ACT_1
{
  class MultiProcessing
  {
    public Queue<Process> processes { get; set; }
    public Queue<Process> prosCurrBatch { get; set; }
    public Stack<Process> proCompleted { get; set; }
    public int total { get; set; }
    public int idx { get; set; }

    public MultiProcessing(int t)
    {
      processes = new Queue<Process>();
      prosCurrBatch = new Queue<Process>();
      proCompleted = new Stack<Process>();
      // -------------------------------- //
      idx = 1;
      total = t;
    }

    public void MoveProssToCurrBatch()
    {
      int TAM_LOTE = 0;
      while (TAM_LOTE < 5 && processes.Count != 0) {
        Process p = processes.Dequeue();
        prosCurrBatch.Enqueue(p);
        TAM_LOTE++;
      }
    }
  }
}
