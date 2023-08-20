namespace Pigeon.Algoritms;

public sealed class ArrayComparer : IEqualityComparer<string[]>
{
    public bool Equals(string[] x, string[] y)
    {
        if (x[0] == y[0])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetHashCode(string[] obj)
    {
        if (obj.Length == 0)
            return obj.GetHashCode();

        return obj[0].GetHashCode() + obj[1].GetHashCode();
    }
}

