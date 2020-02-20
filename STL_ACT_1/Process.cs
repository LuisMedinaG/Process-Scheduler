
using System;

namespace STL_ACT_1
{
  class Process : IEquatable<Process>
  {
    public string Operation { get; set; }
    public string Result { get; set; }
    public int ProcessId { get; set; }
    public int TME { get; set; }

    public Process() { }

    public Process(int ProcessId, int TME, int izq, int opeIdx, int der)
    {
      var ope = new Operation(izq, opeIdx, der);
      this.ProcessId = ProcessId;
      this.TME = TME;
      Operation = ope.ToString();
      Result = ope.Solve();
    }

    public bool Equals(Process other)
    {
      return this.ProcessId == other.ProcessId;
    }
  }
}
