using System.Collections;
using System.Collections.Generic;

public class MathUtils
{
    public static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
