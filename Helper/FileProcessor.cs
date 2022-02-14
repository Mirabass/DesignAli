using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Helper
{
    public static class FileProcessor
    {
        public static Dictionary<(int, int), string> LoadTableDataFromFile(string filePath, char separator)
        {
            Dictionary<(int, int), string> data = new Dictionary<(int, int), string>();
            List<string> lines = File.ReadAllLines(filePath).ToList();
            int row = 0, column = 0;
            foreach (string line in lines)
            {
                string[] cells = line.Split(separator);
                foreach (string cell in cells)
                {
                    data.Add((row, column++), cell);
                }
                row++;
                column = 0;
            }

            return data;
        }
        /// <summary>
        /// Loads file with data separated by fixed widths.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="startingRow">First row has index 0</param>
        /// <param name="widths">First item is startIndex, others are widths</param>
        /// <returns></returns>
        public static Dictionary<(int, int), string> LoadTableDataFromFileWithFixedWidths(string filePath, int startingRow, int nOmittedEndLines, List<int> widths)
        {
            Dictionary<(int, int), string> data = new Dictionary<(int, int), string>();
            List<string> lines = File.ReadAllLines(filePath).ToList();
            if (startingRow > lines.Count)
            {
                throw new Exception("Starting row is higher than lines in file");
            }
            int row = 0;
            for (int lineIndex = startingRow; lineIndex < lines.Count - nOmittedEndLines; lineIndex++)
            {
                for (int column = 0; column < widths.Count - 1; column++)
                {
                    int startIndex = widths[0];
                    int length = widths[column + 1];
                    for (int i = 1; i < column + 1; i++)
                    {
                        startIndex += widths[i];
                    }
                    string cell = lines[lineIndex].Substring(startIndex, length);
                    data.Add((row, column), cell);
                }
                row++;
            }
            return data;
        }

        public static Dictionary<(int, int), string> LoadDataFromFile_csvSemicolon(string filePath)
        {
            return LoadTableDataFromFile(filePath, ';');
        }

        public static Dictionary<(int, int), string> LoadDataFromFile_tableWithTabs(string filePath)
        {
            return LoadTableDataFromFile(filePath, '\t');
        }
    }
}
