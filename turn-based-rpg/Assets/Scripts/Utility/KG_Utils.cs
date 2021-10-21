using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KG_Utils  {

	
    public static int BoolToInt(bool @bool)
    {
        int newVal = @bool == true ? 1 : 0;
        return newVal;
    }

    public static bool IntToBool(int @int)
    {
        bool newVal = @int == 1 ? true : false;
        return newVal;
    }

    public static int StringToInt(string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }
        return result;
    }

    // Breaks an int down into an array example: 365 = [3,6,5]
    public static int[] IntToIntArray(int num)
    {
        if (num == 0)
            return new int[1] { 0 };

        List<int> digits = new List<int>();

        for (; num != 0; num /= 10)
            digits.Add(num % 10);

        int[] array = digits.ToArray();
        System.Array.Reverse(array);

        return array;
    }

    // Removes element from array at the specified index.
    public static T[] RemoveAt<T>(this T[] oArray, int idx)
    {
        T[] nArray = new T[oArray.Length - 1];
        for (int i = 0; i < nArray.Length; ++i)
        {
            nArray[i] = (i < idx) ? oArray[i] : oArray[i + 1];
        }
        return nArray;
    }

    // Randomizes an array thats passed in. 
    public static void ShuffleArray<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }

    // Returns the specified list randomized.
    public static List<T> ShuffleList<T>(List<T> list)
    {
        List<T> randomizedList = new List<T>();
        System.Random rnd = new System.Random();
        while (list.Count > 0)
        {
            int index = rnd.Next(0, list.Count); 
            randomizedList.Add(list[index]); 
            list.RemoveAt(index);
        }
        return randomizedList;
    }

    // Returns true if 2 list contain the same data (in any order).
    public static bool CompareLists<T>(List<T> aListA, List<T> aListB)
    {
        if (aListA == null || aListB == null || aListA.Count != aListB.Count)
            return false;
        if (aListA.Count == 0)
            return true;
        Dictionary<T, int> lookUp = new Dictionary<T, int>();
        // create index for the first list
        for (int i = 0; i < aListA.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListA[i], out count))
            {
                lookUp.Add(aListA[i], 1);
                continue;
            }
            lookUp[aListA[i]] = count + 1;
        }
        for (int i = 0; i < aListB.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListB[i], out count))
            {
                // early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
                return false;
            }
            count--;
            if (count <= 0)
                lookUp.Remove(aListB[i]);
            else
                lookUp[aListB[i]] = count;
        }
        // if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
        return lookUp.Count == 0;
    }

    // 
    public static bool ArrayContains(System.Array a, object val)
    {
        return System.Array.IndexOf(a, val) != -1;
    }

    // Turns texture to teture2d.
    public static Texture2D ToTexture2D( Texture texture)
    {
        return Texture2D.CreateExternalTexture(
            texture.width,
            texture.height,
            TextureFormat.RGB24,
            false, false,
            texture.GetNativeTexturePtr());
    }

    
    public static void DrawGizmoDisk(this Transform t, float radius)
    {
        float GIZMO_DISK_THICKNESS = 0.01f;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.color = new Color(0.2f, 0.2f, 0.2f, 0.5f); //this is gray, could be anything
        Gizmos.matrix = Matrix4x4.TRS(t.position, t.rotation, new Vector3(1, GIZMO_DISK_THICKNESS, 1));
        Gizmos.DrawSphere(Vector3.zero, radius);
        Gizmos.matrix = oldMatrix;
    }

    // Caluclates the chance of something happeing based on the specified probability.
    // Example: int chanceAttackMisses = ProbabilityCheck(90). Misses 90% of the time.
    public static bool ProbabilityCheck(int itemProbability)
    {
        float rnd = Random.Range(0, 101);
        if (rnd <= itemProbability)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
