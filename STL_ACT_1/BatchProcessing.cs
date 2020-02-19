using System.Collections.Generic;

namespace STL_ACT_1
{
    class BatchProcessing
    {
        public Queue<Process> processes { get; set; }
        public Queue<Process> prosCurrBatch { get; set; }
        public Stack<Process> proCompleted { get; set; }
        public int totalProcess { get; set; }
        public int idx { get; set; }

        public BatchProcessing()
        {
            processes = new Queue<Process>();
            prosCurrBatch = new Queue<Process>();
            proCompleted = new Stack<Process>();
            totalProcess = 0;
            idx = 1;
        }

        public void MoveProsToCurrBatch()
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
