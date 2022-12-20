using UnityEngine;

public static class ColorComparator
{
    public static bool CompareColor(Color a, Color b)
    {
        const float accdelta = 0.1f;
        bool result = false;
        if (Mathf.Abs(a.r - b.r) < accdelta)
            if (Mathf.Abs(a.g - b.g) < accdelta)
                if (Mathf.Abs(a.b - b.b) < accdelta) result = true;

        return result;
    }
}
