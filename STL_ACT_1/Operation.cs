using System;

namespace STL_ACT_1
{
  class Operation
  {
    private int izq;
    private int der;
    private char ope;
    private char[] opes = { '+', '-', '*', '/', '%' };

    public Operation(int izq, int ope, int der)
    {
      ope = opes[ope];
      this.izq = izq;
      if (ope == '/') {
        this.der = der;
      } else {
        this.der = der;
      }
    }

    public string Solve()
    {
      double res = 0;
      switch (ope) {
        case '+': res = izq + der; break;
        case '-': res = izq - der; break;
        case '*': res = izq * der; break;
        case '/': res = izq / der; break;
        case '%': res = izq % der; break;
      }
      return res.ToString();
    }

    public override string ToString()
    {
      return $"{izq} {ope} {der}";
    }
  }
}
