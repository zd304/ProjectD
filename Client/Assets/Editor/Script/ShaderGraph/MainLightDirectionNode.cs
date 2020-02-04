using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;

[Title("Custom", "Main Light Direction")]
public class MainLightDirectionNode : CodeFunctionNode
{
    protected override MethodInfo GetFunctionToConvert()
    {
        return GetType().GetMethod("CustomNodeFunction",
            BindingFlags.Static | BindingFlags.NonPublic);
    }

    static string CustomNodeFunction(
        [Slot(1, Binding.None)] out Vector3 Out)
    {
        Out = Vector3.zero;
        return @"
        {
            #ifdef LIGHTWEIGHT_INPUT_INCLUDED
                Light l = GetMainLight();

                Out = dl.direction;
            #else
                Out = float3(1,1,1);
            #endif
        }
        ";
    }
}