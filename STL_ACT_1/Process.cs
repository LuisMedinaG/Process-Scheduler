using System;

namespace STL_ACT_1
{
  internal class Process : IEquatable<Process>
  {
    public int Id { get; set; }
    public int State { get; set; }
    public int TME { get; set; }

    public string Ope { get; set; }
    public string Result { get; set; }

    public int tTra { get; set; }
    public int tBlo { get; set; }
    public int tBloRes { get; set; }
    public int tLle { get; set; }
    public int tFin { get; set; }
    public int tRet { get; set; }
    public int tResp { get; set; }
    public int tEsp { get; set; }
    public int tSer { get; set; }
    public int tRest { get; set; }

    public Process()
    {
      Id = -1;
      TME = -1;
      Ope = "";
      Result = "";
      tTra = -1;
      tEsp = -1;
      tFin = -1;
      tLle = -1;
      tSer = -1;
      tFin = -1;
      tRet = -1;
      tBlo = -1;
      tResp = -1;
      tRest = -1;
      tBloRes = -1;
    }

    public Process(int Id, int TME, Operation Ope)
    {
      this.Id = Id;
      this.TME = TME;
      this.Ope = Ope.ToString();
      Result = Ope.result;
      
      State = 1;

      tRest = TME;
      tSer = TME;
      tTra = -1;
      tEsp = -1;
      tFin = -1;
      tLle = -1;
      tFin = -1;
      tRet = -1;
      tResp = -1;
      tBlo = -1;
      tBloRes = -1;
    }

    public Process(Process p)
    {
      State = p.State;
      Id = p.Id;
      TME = p.TME;
      Ope = p.Ope;
      Result = p.Result;

      tTra = p.tTra;
      tBlo = p.tBlo;
      tBloRes = p.tBloRes;
      tEsp = p.tEsp;
      tFin = p.tFin;
      tLle = p.tLle;
      tResp = p.tResp;
      tRest = p.tRest;
      tSer = p.tSer;
      tFin = p.tFin;
      tRet = p.tRet;
    }

    public bool Equals(Process other)
    {
      return Id == other.Id;
    }
  }
}
