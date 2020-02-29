using System;

namespace STL_ACT_1
{
  class Operation
  {
    private int num1;
    private int num2;
    private char ope;
    private static char[] opes = { '+', '-', '*', '/', '%' };

    public string result { get; set; }

    public Operation(int num1, int ope, int num2)
    {
      this.num1 = num1;
      this.ope = opes[ope];
      this.num2 = num2;
      result = Solve();
    }

    public string Solve()
    {
      double res = 0;
      switch (ope) {
        case '+': res = num1 + num2; break;
        case '-': res = num1 - num2; break;
        case '*': res = num1 * num2; break;
        case '/': res = num1 / num2; break;
        case '%': res = num1 % num2; break;
      }
      return res.ToString();
    }

    public override string ToString()
    {
      return $"{num1} {ope} {num2}";
    }
  }
}
