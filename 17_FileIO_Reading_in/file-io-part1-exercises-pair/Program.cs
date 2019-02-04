using System;
using System.IO;

namespace file_io_part1_exercises_pair
{
    class Program
    {
        static void Main(string[] args)
        {
            // ask for a filesystem path for a text file ('alices_adventures_in_wonderland.txt')
            // read the contents of the file
            // display the number of words
            // display the number of sentences

            Console.WriteLine("Enter the full file path to read:");
            string directory = Console.ReadLine();

            int wordCount = 0;
            int sentenceCount = 0;
            string[] wordSeparators = {" ", ". ", "! ", "? ", ", "};
            string[] words;
            string readInLine = "";

            try
            {
                using (StreamReader sr = new StreamReader(directory))
                {
                    while (!sr.EndOfStream)
                    {
                        //read in line
                        readInLine = sr.ReadLine();

                        //word counting
                        words = readInLine.Split(wordSeparators, StringSplitOptions.None);
                        wordCount += words.Length;

                        //sentence counting
                        foreach (char letter in readInLine)
                        {
                            if (letter == '.' ||
                                letter == '!' ||
                                letter == '?')
                            {
                                sentenceCount++;
                            }
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Error reading the file");
                Console.WriteLine(e.Message);
            }

            Console.WriteLine($"Word count = {wordCount}\nSentence Count = {sentenceCount}");
            Console.ReadLine();
        }
    }
}
