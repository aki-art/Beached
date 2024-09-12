using HarmonyLib;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Beached.Content.Defs.StarmapEntities
{
    /// <summary>
    /// TODO: move that to whatever location you want
    /// </summary>
    public class TranspilerHelper
    {
        public static int FindIndexOfNextLocalIndex(List<CodeInstruction> codeInstructions, int insertionIndex, bool goingBackwards = true) => FindIndicesOfLocalsByIndex(codeInstructions,insertionIndex,1,goingBackwards)[0];


        public static int[] FindIndicesOfLocalsByIndex(List<CodeInstruction> codeInstructions, int insertionIndex, int numberOfVarsToFind = 1, bool goingBackwards = true)
        {
            var indices = new List<int>();

            if (insertionIndex != -1)
            {
                int direction = goingBackwards ? -1 : 1;
                for (int i = insertionIndex - 1; i >= 0 && i<codeInstructions.Count && indices.Count < numberOfVarsToFind; i +=direction)
                {
                    if (CodeInstructionExtensions.IsLdloc(codeInstructions[i]))
                    {
                        int locIndex = GiveOpCodeIndexFromLocalBuilder(codeInstructions[i]);
                        if (!indices.Contains(locIndex))
                            indices.Insert(0, locIndex);
                        break;
                    };
                }


                //Debug.Log(codeInstructions[i].operand.GetType().ToString(), "transpilertest");
                //Debug.Log(codeInstructions[i].operand.ToString(), "transpilertest");
                //Debug.Log(((LocalBuilder)codeInstructions[i].operand).LocalIndex.ToString(), "transpilertest");
            }
            else
            {
                indices.Add(-1);
            }
            return indices.ToArray();
        }

        /// <summary>
        /// first entry in the tuple is the local index, second entry is the code index
        /// </summary>
        /// <param name="codeInstructions">CIs</param>
        /// <param name="insertionIndex">start index</param>
        /// <param name="goingBackwards">backwards = true -> descending index from insertionIndex </param>
        /// <returns></returns>
        public static Tuple<int, int> FindIndexOfNextLocalIndexWithPosition(List<CodeInstruction> codeInstructions, int insertionIndex, bool goingBackwards = true)
        {
            var array = FindIndicesOfLocalsByIndexWithPositions(codeInstructions, insertionIndex, 1, goingBackwards);
            return new Tuple<int, int>(array.first[0], array.second[0]);
        }
        public static Tuple<int[],int[]> FindIndicesOfLocalsByIndexWithPositions(List<CodeInstruction> codeInstructions, int insertionIndex, int numberOfVarsToFind = 1, bool goingBackwards = true)
        {
            var indices = new List<int>();
            var positions = new List<int>();

            if (insertionIndex != -1)
            {
                int direction = goingBackwards ? -1 : 1;
                for (int i = insertionIndex - 1; i >= 0 && i < codeInstructions.Count && indices.Count < numberOfVarsToFind; i += direction)
                {
                    if (CodeInstructionExtensions.IsLdloc(codeInstructions[i]))
                    {
                        int locIndex = GiveOpCodeIndexFromLocalBuilder(codeInstructions[i]);
                        if (!indices.Contains(locIndex))
                        {
                            indices.Insert(0, locIndex);
                            positions.Insert(0, i); break;
                        }
                        break;
                    };
                }


                //Debug.Log(codeInstructions[i].operand.GetType().ToString(), "transpilertest");
                //Debug.Log(codeInstructions[i].operand.ToString(), "transpilertest");
                //Debug.Log(((LocalBuilder)codeInstructions[i].operand).LocalIndex.ToString(), "transpilertest");
            }
            else
            {
                indices.Add(-1);
            }
            return new Tuple<int[], int[]> (indices.ToArray(),positions.ToArray());
        }



        static int GiveOpCodeIndexFromLocalBuilder(CodeInstruction codeInstruction)
        {
            var CheckCode = codeInstruction.opcode;
            if (CheckCode == OpCodes.Ldloc_0)
            {
                return 0;
            }
            else if (CheckCode == OpCodes.Ldloc_1)
            {
                return 1;
            }
            else if (CheckCode == OpCodes.Ldloc_2)
            {
                return 2;
            }
            else if (CheckCode == OpCodes.Ldloc_3)
            {
                return 3;
            }
            else 
            {
                if (codeInstruction.operand == null)
                    return -1;
                return ((LocalBuilder)codeInstruction.operand).LocalIndex;
            }
        }


        //public static int GetFirstIndexOfType(List<CodeInstruction> codeInstructions, Type ObjectType)
        //{
        //    Debug.Log(ObjectType.ToString(), "transpilertest");

        //    for (int i = 0; i < codeInstructions.Count; ++i)
        //    {
        //        if (CodeInstructionExtensions.IsLdloc(codeInstructions[i],))
        //        {
        //            //var locBuilder = codeInstructions[i].operand;
        //            Debug.Log(((LocalBuilder)codeInstructions[i].operand).LocalIndex.ToString(), "transpilertest");
        //            if (((LocalBuilder)locBuilder).LocalType == ObjectType)
        //            {
        //                return ((LocalBuilder)locBuilder).LocalIndex;
        //            }
        //        }
        //    }
        //    return -1;
        //}



        public static void PrintInstructions(List<HarmonyLib.CodeInstruction> codes, bool extendedInfo = false)
        {
            Debug.Log("\"IL-Dump Start:\n");
            for (int i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                //Debug.Log(code);
                //Debug.Log(code.opcode);
                //Debug.Log(code.operand);

                if (extendedInfo)
                {
                    if (code.operand != null)
                        Debug.Log(i + "=> OpCode: " + code.opcode + "::" + code.operand + "<> typeof (" + (code.operand.GetType()) + ")");
                    else
                        Debug.Log(i + "=> OpCode: " + code.opcode);
                }
                else
                    Debug.Log(i + ": " + code);
            }
            Debug.Log("\nIL-Dump Finished");
        }
    }
}
