using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace SystemDesign.Utils {
  public static class Calculations
  {
    private static char[] charSet =  "qwertyuiopasdfghjklzxcvbnm0123456789QWERTYUIOPASDFGHJKLZXCVBNM".ToCharArray() ;
    public static string HexTo62(this long num)
    {
      long rest = num;
      Stack<char> stack = new Stack<char>();
      StringBuilder result = new StringBuilder(0);
      while (rest != 0)
      {

        stack.Push(charSet[(rest - (rest / 62) * 62)]);
        rest = rest / 62;
      }
      for (; stack.Count() != 0;) {
        result.Append(stack.Pop());
      }
      return result.ToString();
    }

  }
}