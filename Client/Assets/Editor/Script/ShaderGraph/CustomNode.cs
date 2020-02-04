using System.Collections;
using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;

[Title("Custom", "Test")]
public class CustomNode : CodeFunctionNode
{
    protected override MethodInfo GetFunctionToConvert()
    {
        return GetType().GetMethod("CustomNodeFunction",
            BindingFlags.Static | BindingFlags.NonPublic);
    }

    static string CustomNodeFunction(
        [Slot(0, Binding.WorldSpaceNormal)] Vector3 A,
        [Slot(1, Binding.None)] out Vector3 Out)
    {
        Out = Vector3.zero;
        return @"
        {
            #ifdef LIGHTWEIGHT_INPUT_INCLUDED
                Light l = GetMainLight();

                Out = dot(l.direction, A);
            #else
                Out = float3(1,1,1);
            #endif
        }
        ";
    }
}
