using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System;
using System.Linq;

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
                string aValueConverted = row[column].ToLower();
                //Dictionary<string, string> convertedDictionary = ConvertValuesToLowerCase();
                string valueConverted = value.ToLower();
                bool checker2 = aValueConverted.Contains(valueConverted);

                if (checker2)
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
            Dictionary<string, string> convertedDictionary = new Dictionary<string, string>();
            double i = 0;
            foreach (string value in dictionary.Values)
            {
                string converted = value.ToLower();
                
                
                convertedDictionary.Add("k"+i, converted);
                i++;
            }
            return convertedDictionary;
        }

     
        //create method FindByValue i added this here
        public static List<Dictionary<string, string>> FindByValue(string value)
        {
            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();
            
                foreach (Dictionary<string, string> job in AllJobs)
                {
                    
                    Dictionary<string, string> aValue = job;

                        bool checker = aValue.ContainsValue(value);
                    
                    //Dictionary<string, string> converted = dict((k, value.ToLower()) for k, value in )
                    //double check = 0;
                    Dictionary<string, string> convertedDictionary = ConvertValuesToLowerCase(job);
                    string valueConverted = value.ToLower();
                    bool checker2 = convertedDictionary.ContainsValue(valueConverted);
                    bool checker3 = convertedDictionary.Values.Any(v => v.Contains(valueConverted));

                    if (checker3)
                    {
                        //check++;
                        if (!jobs.Contains(job))
                        {
                            //Console.WriteLine(valueConverted);
                            jobs.Add(job);
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
