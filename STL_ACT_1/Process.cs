using System;

namespace STL_ACT_1
{
  class Process : IEquatable<Process>
  {
    internal int tRest;

    public int ID { get; set; }
    public int TME { get; set; }
    public string Ope { get; set; }
    public string OpeResult { get; set; }

    public int tTra { get; set; }
    public int tBlo { get; set; }
    public int tLle { get; set; }
    public int tFin { get; set; }
    public int tRet { get; set; }
    public int tResp { get; set; }
    public int tEsp { get; set; }
    public int tSer { get; set; }
    public bool Exists { get; set; }

    public Process()
    {
      ID = 0;
      TME = 0;
      Ope = "";
      OpeResult = "";
    }

    public Process(int ID, int TME, Operation Ope)
    {
      this.ID = ID;
      this.TME = TME;
      this.Ope = Ope.ToString();
      OpeResult = Ope.result;
      tSer = TME;
    }

    public bool Equals(Process other)
    {
      return ID == other.ID;
    }
  }
}
