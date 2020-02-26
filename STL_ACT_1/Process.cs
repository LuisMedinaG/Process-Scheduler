using System;

namespace STL_ACT_1
{
  class Process : IEquatable<Process>
  {
    public int ID { get; set; }
    public int TME { get; set; }
    public int RemainigTime { get; set; }
    public string Ope { get; set; }
    public string OpeResult { get; set; }

    public Process() { }

    public Process(int ID, int TME, Operation Ope)
    {
      this.ID = ID;
      this.TME = TME;
      RemainigTime = TME;
      this.Ope = Ope.ToString();
      OpeResult = Ope.Solve();
    }

    public bool Equals(Process other)
    {
      return this.ID == other.ID;
    }
  }
}
