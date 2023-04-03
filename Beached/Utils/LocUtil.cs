using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Beached.Utils
{
    // works near identical to the game's Localization class, but this one supports translator notes
    internal class LocUtil
    {
        public static void GenerateStringsTemplate(string locstrings_namespace, Assembly assembly, string output_filename, Dictionary<string, object> runtimeForest)
        {
            var dictionary1 = new Dictionary<string, object>();
            foreach (var locStringTreeRoot in Localization.CollectLocStringTreeRoots(locstrings_namespace, assembly))
            {
                var dictionary2 = MakeRuntimeLocStringTree(locStringTreeRoot);
                if (dictionary2.Count > 0)
                {
                    dictionary1[locStringTreeRoot.Name] = dictionary2;
                }
            }

            if (runtimeForest != null)
            {
                dictionary1.Concat(runtimeForest);
            }

            using (StreamWriter writer = new(output_filename, false, new UTF8Encoding(false)))
            {
                writer.WriteLine("msgid \"\"");
                writer.WriteLine("msgstr \"\"");
                writer.WriteLine("\"Application: Oxygen Not Included\"");
                writer.WriteLine("\"POT Version: 2.0\"");
                writer.WriteLine("\"Modified translation file by Beached\"");
                WriteStringsTemplate(locstrings_namespace, writer, dictionary1);
            }

            Log.Info("Generated " + output_filename);
        }

        public class TextInfo
        {
            public string text;
            public string translatorsNote;
        }

        public static Dictionary<string, object> MakeRuntimeLocStringTree(Type locstring_tree_root)
        {
            Dictionary<string, object> dictionary = new();
            var fields = locstring_tree_root.GetFields();

            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.FieldType != typeof(LocString))
                {
                    continue;
                }

                if (!fieldInfo.IsStatic)
                {
                    DebugUtil.DevLogError("LocString fields must be static, skipping. " + fieldInfo.Name);
                    continue;
                }

                var locString = (LocString)fieldInfo.GetValue(null);
                if (locString == null)
                {
                    Debug.LogError("Tried to generate LocString for " + fieldInfo.Name + " but it is null so skipping");
                }
                else
                {
                    dictionary[fieldInfo.Name] = new TextInfo()
                    {
                        text = locString.text,
                        translatorsNote = GetNote(fieldInfo)
                    };
                }
            }

        var nestedTypes = locstring_tree_root.GetNestedTypes();

            foreach (Type type in nestedTypes)
            {
                var dictionary2 = MakeRuntimeLocStringTree(type);
                if (dictionary2.Count > 0)
                {
                    dictionary[type.Name] = dictionary2;
                }
            }

            return dictionary;
        }

        private static string GetNote(FieldInfo fieldInfo)
        {
            return fieldInfo.GetCustomAttribute<NoteAttribute>()?.message;
        }

        public static void GenerateStringsTemplate(Type locStringTreeRoot, string outputFolder)
        {
            outputFolder = FileSystem.Normalize(outputFolder);

            if (!FileUtil.CreateDirectory(outputFolder, 5))
            {
                return;
            }

            GenerateStringsTemplate(
                locStringTreeRoot.Namespace, 
                Assembly.GetAssembly(locStringTreeRoot), 
                FileSystem.Normalize(Path.Combine(outputFolder,
                string.Format("{0}_template.pot", 
                locStringTreeRoot.Namespace.ToLower()))), null);
        }

        private static void WriteStringsTemplate(string path, StreamWriter writer, Dictionary<string, object> runtimeTree)
        {
            if (writer == null) Log.Warning("writer is null");
            if (runtimeTree == null) Log.Warning("runtimeTree is null");
            var stringList = new List<string>(runtimeTree.Keys);
            stringList.Sort();

            foreach (string key in stringList)
            {
                var path1 = path + "." + key;
                var tree = runtimeTree[key];

                if (tree == null) Log.Warning("tree is null");
                var type = tree.GetType();

                Log.Debug("type of tree is " + type.Name);
                if (type != typeof(string) && type != typeof(TextInfo))
                {
                    WriteStringsTemplate(path1, writer, tree as Dictionary<string, object>);
                }
                else
                {
                    var info = tree as TextInfo;
                    if (info == null) Log.Warning("info is null");
                    var str = info == null ? tree as string : info.text;

                    str = str
                        .Replace("\\", "\\\\")
                        .Replace("\"", "\\\"")
                        .Replace("\n", "\\n")
                        .Replace("’", "'")
                        .Replace("“", "\\\"")
                        .Replace("”", "\\\"")
                        .Replace("…", "...");

                    var desc = info == null ? "#. " + path1 : $"# {info.translatorsNote}";

                    writer.WriteLine(desc);
                    writer.WriteLine("msgctxt \"{0}\"", path1);
                    writer.WriteLine("msgid \"" + str + "\"");
                    writer.WriteLine("msgstr \"\"");
                    writer.WriteLine("");
                }
            }
        }
    }
}
