using System;

namespace STL_ACT_1
{
  class Process : IEquatable<Process>
  {
    public int ID { get; set; }
    public int TME { get; set; }
    public int RemainigTime { get; set; }
    public string Operation { get; set; }
    public string OpeResult { get; set; }

    public Process() { }

    public Process(int ID, int TME, int num1, int opeIdx, int num2)
    {
      this.ID = ID;
      this.TME = TME;
      RemainigTime = TME;
      var ope = new Operation(num1, opeIdx, num2);
      Operation = ope.ToString();
      OpeResult = ope.Solve();
    }

    public bool Equals(Process other)
    {
      return this.ID == other.ID;
    }
  }
}
