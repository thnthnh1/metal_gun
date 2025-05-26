using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.ETool.ETAssetNameModifierSP
{
    public static class ERegex
    {
        public static string RemovePrefix(this string input, string prefix)
        {
            string pattern = "^" + Regex.Escape(prefix);
            return Regex.Replace(input, pattern, "");
        }
        public static string RemoveSuffix(this string input, string suffix)
        {
            string pattern = Regex.Escape(suffix) + "$";
            return Regex.Replace(input, pattern, "");
        }
        /// <summary>
        /// Use a regular expression to match "g card game" and replace it with "gCardGame"
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToCamelStyle(this string input)
        {
            return Regex.Replace(input, @"(?:_| )(\w)", match => match.Groups[1].Value.ToUpper());
        }
        public static string ToSnakeStyle(this string input)
        {
            // Use regex to insert underscores before capital letters and convert to lowercase
            return Regex.Replace(input, "([a-z0-9])([A-Z])|\\s", "$1_$2").ToLower();
        }
    }
    public static class EFile
    {
        /// <summary>
        /// Count file type in direction
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileTypes"></param>
        /// <returns></returns>
        public static int FilesCount(DirectoryInfo directory, List<string> fileTypes)
        {
            int ret = 0;
            foreach (string fileType in fileTypes)
            {
                FileInfo[] imageFiles = directory.GetFiles("*." + fileType);
                ret += imageFiles.Length;
            }
            return ret;
        }
        /// <summary>
        /// Rename file to newFileName = name.filetype ex: object1.obj
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="newFileName">newFileName = name.filetype ex: object1.obj</param>
        public static void Rename(FileInfo sourceFile, string newFileName)
        {
            // Update name
            File.Move(sourceFile.FullName, newFileName);
            // Update the corresponding .meta file
            string oldMetaFilePath = sourceFile.FullName + ".meta";
            if (File.Exists(oldMetaFilePath))
            {
                string newMetaFilePath = newFileName + ".meta";
                File.Move(oldMetaFilePath, newMetaFilePath);
            }
        }
    }
}