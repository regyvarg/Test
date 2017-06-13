using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Console
{
    public class FileParser
    {
        const string END_OF_RECORD = "===================END OF RESULT===================";
        /// <summary>
        /// Parse the input file and provide processed result.
        /// </summary>
        /// <param name="filePath">input file path</param>
        /// <param name="sortField">sort field</param>
        /// <param name="searchText">search text</param>
        /// <returns>result records</returns>
        public static string ParseFile(string filePath,string sortField,string searchText)
        {
            Dictionary<string,string> result = new Dictionary<string,string>();

            var resultString = new StringBuilder();
            var recordDictionary = new Dictionary<string, bool>();
            SortedDictionary<string, Dictionary<string,bool>> sortedResult = new SortedDictionary<string, Dictionary<string,bool>>(); 

            //key of sorteddictiony is search field, value of sorted dictionary is dictionary (in case same sortedfield has multiple record). 
            //value of SortedDictionary is dictionary of (record string, flag to indicate Is Match Search Text Found).

            var recordString = new StringBuilder();
            var sortFieldValueString = string.Empty;
            
            using (var reader = new StreamReader(filePath))
            {
                var currentLine = string.Empty;
                var isMatchRecord = false;
                string[] sortFieldValue;
                while ((currentLine = reader.ReadLine()) != null)
                {
                    //append to record string.
                    recordString.AppendLine(currentLine);
                    //sort field
                    if (currentLine.Contains(sortField))
                    {
                        sortFieldValue= currentLine.Split(':');
                        if (sortFieldValue != null && sortFieldValue.Length > 1)
                        {
                            sortFieldValueString = sortFieldValue[1];   
                        }
                    }
                    //search text - is match found
                    if (currentLine.Contains(searchText))
                    {
                        isMatchRecord = true;
                    }
                    //one record is processed. so, store the processed result and reset the fields
                    if (currentLine.Contains(END_OF_RECORD))
                    {
                        if (!sortedResult.ContainsKey(sortFieldValueString.Trim()))
                        { 
                            var recordStringDictionary = new Dictionary<string,bool>();
                            recordStringDictionary.Add(recordString.ToString(), isMatchRecord);
                            sortedResult.Add(sortFieldValueString.Trim(), recordStringDictionary);
                        }
                        else
                        {
                            sortedResult[sortFieldValueString.Trim()].Add(recordString.ToString(), isMatchRecord);
                        }
                     
                     isMatchRecord = false;
                     sortFieldValueString = string.Empty;
                        recordString.Clear();
                    }
                }
            }
            if (searchText == string.Empty)
            {                
                return processResultDictionary(sortedResult, false);
            }
            else
            {
                return processResultDictionary(sortedResult, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordSet">
        /// //key of sorteddictiony is search field, value of sorted dictionary is dictionary (in case same sortedfield has multiple record). value of SortedDictionary is dictionary of (record string, flag to indicate Is Match Search Text Found).
        /// </param>
        /// <param name="isOnlyMatch">
        /// to Indicate only search match text record to result
        /// </param>
        /// <returns>result string</returns>
        private static string processResultDictionary(SortedDictionary<string,Dictionary<string,bool>> recordSet,bool isOnlyMatch)
        {
            StringBuilder resultString = new StringBuilder();;
            foreach (var record in recordSet)
            {
                foreach (var recordDictionary in record.Value)
                {
                    if (isOnlyMatch && recordDictionary.Value)
                    {
                        resultString.Append(recordDictionary.Key);
                    }
                    else if (!isOnlyMatch)
                    {
                        resultString.Append(recordDictionary.Key);
                    }
                }
            }

            if (resultString.ToString() == string.Empty)
            {
            return "No Match Record Found.";
            }
            return resultString.ToString();
        }
    }
}
