using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System;

namespace TechJobsConsole
{
    class JobData
    {
        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();
        static bool IsDataLoaded = false;

        public static List<Dictionary<string, string>> FindAll()
        {
            LoadData();
            return AllJobs;
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        public static List<string> FindAll(string column)
        {
            LoadData();

            List<string> values = new List<string>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                string aValue = job[column];

                if (!values.Contains(aValue))
                {
                    values.Add(aValue);
                }
            }
            return values;
        }

        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> row in AllJobs)
            {
                string aValue = row[column];
                bool checker = aValue.Contains(value);
          

                if (checker)
                {
                    jobs.Add(row);
                    //return jobs;
                    //checker = false;

                }
            }

            return jobs;
        }

        //Convert the values to lowercase method:

        public static Dictionary<string, string> ConvertValuesToLowerCase(Dictionary<string, string> dictionary)
        {
            foreach (string value in dictionary.Values)
            {
                string converted = value.ToLower();

            }
            return dictionary;
        }

        /*
        private static Dictionary<string, string> ConvertKeysToLowerCase(Dictionary<string, string>> dictionaries)
        {
            Dictionary<string, string> resultingConvertedDictionaries
                = new Dictionary<string, string>();
            //foreach (ILanguage keyLanguage in dictionaries.Keys)
           // {
                Dictionary<string, string> convertedDictionary = new Dictionary<string, string>();
                foreach (string value in dictionaries.Values)
                {
 
                    convertedDictionary.Add("k" , value.ToLower());
                }

                //resultingConvertedDictionaries.Add(convertedDictionary);
            }
           // return convertedDictionary;
            //return resultingConvertedDictionaries;
        }
        /*
        public static Dictionary<string, string> ConvertValuesToLowerCase(Dictionary<string, string> dictionaries)
        {
            // < Dictionary<string, string> > convertedDictionary;
            IDictionary<ILanguage, IDictionary<string, string>> resultingConvertedDictionaries
         = new Dictionary<ILanguage, IDictionary<string, string>>();
            foreach (ILanguage keyLanguage in dictionaries.Keys)
            {
                IDictionary<string, string> convertedDictionatry = new Dictionary<string, string>();
                foreach (string key in dictionaries[keyLanguage].Keys)
                {
                    convertedDictionatry.Add(key.ToLower(), dictionaries[keyLanguage][key]);
                }
                resultingConvertedDictionaries.Add(keyLanguage, convertedDictionatry);
            }
            return resultingConvertedDictionaries;
            /*
            List<Dictionary<string, string>> resultingConvertedDictionaries = new List<Dictionary<string, string>>;
            foreach(Dictionary<string, string> dictionaree in dictionaries)
            {
                Dictionary<string, string> convertedDictionary = new Dictionary<string, string>();
                foreach (string value in dictionaree.Values)
                {
                    convertedDictionary.Add(dictionaree, value.ToLower());
                }
                resultingConvertedDictionaries.Add(convertedDictionary);
            }
            return resultingConvertedDictionaries;*/


        //create method FindByValue i added this here
        public static List<Dictionary<string, string>> FindByValue(string value)
        {
            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();
            //KeyValuePair<string, string> allJobs = AllJobs;
            
            foreach (Dictionary<string, string> column in AllJobs)
            {
                foreach (Dictionary<string, string> row in AllJobs)
                {
                    //Dictionary<string, string> column1 = column;
                    Dictionary<string, string> aValue = row;
                    
                    //seemed to work better when column was removed behind row

                    bool checker = aValue.ContainsValue(value);
                    //Dictionary<string, string> converted = dict((k, value.ToLower()) for k, value in )
                    //double check = 0;
                    Dictionary<string, string> convertedDictionary = ConvertValuesToLowerCase(row);
                    string valueConverted = value.ToLower();
                    bool checker2 = convertedDictionary.ContainsValue(valueConverted);

                    if (checker2)
                    {
                        //check++;
                        if (!jobs.Contains(row))
                        {
                            //Console.WriteLine(valueConverted);
                            jobs.Add(row);
                        }
                    }

                   }
            }

            return jobs;
        }

        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
        {

            if (IsDataLoaded)
            {
                return;
            }

            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] rowArrray = CSVRowToStringArray(line);
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            string[] headers = rows[0];
            rows.Remove(headers);

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                AllJobs.Add(rowDict);
            }

            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }
    }
}
