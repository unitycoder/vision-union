using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;

public static class DebugUtils
{
    static StringBuilder s_String = new StringBuilder();

    public static void LogFlat2DMatrix<T>(T[] matrix, int width, int height)
    {
        s_String.Length = 0;
        for (int y = 0; y < height; y++)
        {
            var rowIndex = y * width;
            for (int x = 0; x < width - 1; x++)
            {
                s_String.AppendFormat("{0}, ", matrix[rowIndex + x]);
            }

            s_String.AppendLine(matrix[rowIndex + width - 1].ToString());    // end of row, no comma
        }
        
        Debug.Log(s_String);
    }
    
    public static void LogFlat2DMatrix<T>(NativeArray<T> matrix, int width, int height)
        where T: struct
    {
        s_String.Length = 0;
        for (int y = 0; y < height; y++)
        {
            var rowIndex = y * width;
            for (int x = 0; x < width - 1; x++)
            {
                s_String.AppendFormat("{0}, ", matrix[rowIndex + x]);
            }

            s_String.AppendLine(matrix[rowIndex + width - 1].ToString());    // end of row, no comma
        }
        
        Debug.Log(s_String);
    }
}
