using System;
using System.IO;
using System.Collections.Generic;

namespace FindAndReplace
{
    class Program
    {
        static void Main(string[] args)
        {
            //Write a program that can be used to open a file and find and replace all occurrences of one word with another word.

            //The program should ask the user for 4 inputs

            //* The search phrase
            //* The replace phrase
            //* The source file path.  
            //  * This must be an existing file.  If the user enters an invalid source file path, the program will indicate this to the user and go again.*
            //* The destination file path.  
            //* The program will * *create a copy * *of the source file with the requested replacements at this location.
            //  If a file or directory already exists at this location, the program will print an error and exit.*

            //In addition to writing out the changed file, the program will display to the console the number of occurrences of the search phrase that was found and replaced

            //Use `alices_adventures_in_wonderland.txt` for test input.

            Console.WriteLine("Enter the search phrase:");
            string searchPhrase = Console.ReadLine();

            Console.WriteLine("Enter the replace phrase:");
            string replacePhrase = Console.ReadLine();

            Console.WriteLine("Enter the source file path:");
            string sourcePath = Console.ReadLine();

            while (!File.Exists(sourcePath))
            {
                Console.WriteLine("I couldn't find that file.  Try again.\nEnter the source file path:");
                sourcePath = Console.ReadLine();
            }

            Console.WriteLine("Enter the destination file path:");
            string destinationPath = Console.ReadLine();

            if (File.Exists(destinationPath))
            {
                Console.WriteLine("That file already exists.  Exiting!");
            }
            else
            {
                int findCounter = 0;
                using (StreamReader sr = new StreamReader(sourcePath))
                {
                    using (StreamWriter sw = new StreamWriter(destinationPath))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();

                            findCounter += PhraseCount(line, searchPhrase);
                            line = line.Replace(searchPhrase, replacePhrase);

                            sw.WriteLine(line);
                        }
                    }
                }
                Console.WriteLine($"The phrase \'{searchPhrase}\'was found and replaced {findCounter} times");
            }
            Console.ReadLine();
        }

        static public int PhraseCount(string input, string phraseToFind)
        {
            // continually find firstOccurence of phrase starting at an accruing index
            HashSet<int> indicesFound = new HashSet<int>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input.Substring(i).Contains(phraseToFind))
                {
                    indicesFound.Add(input.IndexOf(phraseToFind, i));
                }
            }

            return indicesFound.Count;
        }
    }
}
