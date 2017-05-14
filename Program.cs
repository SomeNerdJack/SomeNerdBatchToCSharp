using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/***
 * ==========OVERALL NOTES==========
 * BatchToCSharp2.0 has a few pre-prepared commands that are not found currently prepared here.
 * -These commands are likely not working... But the concepts are there.
***/

namespace BatchToCSharp2._0
{
    class ReadFromFile
    {
        static string pathfinder(string args)
        {
            string dir = "Environment.GetEnvironmentVariable(" + args + ");" ;
            System.Console.WriteLine("\t\tEnviroment variable detected: " + args);
            System.Console.WriteLine("\t\tCurrent contents of variable: " + args);
            return dir;
        }

        static string angleBracketHandler(string line) //Takes entire line as argument
        {
            if (line.Contains(">>"))    //This appends a new line to a text file, rather than entirley ovewriting it.
            {
                int i = line.IndexOf(">>") + 2;         //Position of characters to split string by (Plus 2 to excempt them from the string
                line = line.Substring(i);               //Cuts the string based on position from int i
                line = line.Trim();                     //Removes whitespace (spacea, if they exist)
                System.IO.File.AppendAllText(@"C:\Temp\REPLACETHISPATH.txt", Environment.NewLine);
            }
            string arg = line.TrimStart('>');
            if(arg.Contains('%'))
            {
                //variable hander //NOT IMPLEMENTED //Use "var" instead of int, string, etc.
                bool localVariable = false;

                if (!localVariable)
                {
                    if (arg.Equals("%CD%", StringComparison.InvariantCultureIgnoreCase))             //If the path we are looking for is the dirrectory in whitch the program is located.
                    {
                        string dir = ("\"System.IO.Directory.GetCurrentDirectory();");    //Set dir to the command to get the current directory.
                        return dir;                                                                 //The end result of the method is returned.
                    }
                }
                return "Error";
            }

            return "Error";
        }




        static void Main()
        {

            ///VARIABLES\\\
            string filepath = "C:\\Users\\TRUTECH\\Desktop\\TestFile2.txt";             //Double slashes = one slash
            string filename = System.IO.Path.GetFileNameWithoutExtension(@filepath);    //Get the filename
            List<string> fileContents = new List<string>();                                //The contents to be written to the C# file
            fileContents.Add("using System;" + Environment.NewLine + "using System.Collections.Generic;" + Environment.NewLine + "using System.Linq;" + Environment.NewLine + "using System.Text;" + Environment.NewLine + "using System.Threading;" + Environment.NewLine + Environment.NewLine);    //Default "using" statments and the empty line thereafter      //Future improvment: figure out what is needed rather than guessing.
            string classname = "class (" + filename + ")" + Environment.NewLine + "{";    //Get the filename (without extension)
            int[] tabulation = { 3 }; //Amount of tabs to insert before new lines to line up with brackets //NOT IMPLEMENTED YET
            int lineNumber = 1; //Line of batch file being processed. Purely for troubleshooting reasons.
            string[] lines = System.IO.File.ReadAllLines(@filepath);    //Take contents of file and make array of each line
            int totalLines = lines.Length;

            ///MAIN PROGRAM\\\
            foreach (string line in lines)          //Look at one line at a time
            {
                string[] words = line.Split(' ');       //Split line into words that are seperated by spaces (and don't keep the spaces in the array)
                System.Console.WriteLine("Processing line:\t" + line);  //Say what line is bring processed


                //Process rest of file\\\
                string command = words.First();     //Get the first word of the line being processed (IE: The command)


                if (command.StartsWith("@"))
                {
                    if (command.Equals("@echo", StringComparison.InvariantCultureIgnoreCase))
                    {
                        System.Console.WriteLine("This is a work in progress, but,\"ECHO\", by default, is disabled.\n\nIf \"ECHO\" is on... too bad. I haven't fixed that yet.");
                    }
                    else
                    {
                        System.Console.WriteLine(command + "\t\tI have no idea what this is...\n\nGoing to error...");
                        goto Error;
                    }
                    goto Success;
                }


                if (command.StartsWith(":"))     //If line is a goto marker
                {
                    command.TrimStart(':');     //Take away the ':'
                    fileContents.Add(command + ":;"); //Then convert to a C# marker (With the appropriate name)
                    goto Success;
                }

                if (command.Equals("goto", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (words.Length.Equals(2))
                    {
                        fileContents.Add("goto" + string.Join(" ", words.Skip(1)) + ":;");
                    }
                    else
                    {
                        goto Error;
                    }
                    goto Success;
                }


                if (command.Equals("echo", StringComparison.InvariantCultureIgnoreCase))    //If the command is echo, regardless of case (lower or upper), continue with program
                {
                    string echoArg = string.Join(" ", words.Skip(1));       //Subtract the command from the arguments (IE: What is being said)
                    string echoText = "System.Console.WriteLine(\"" + echoArg + "\");"; //Set up line for the file
                    System.Console.WriteLine("\tConverts to: \t" + echoText);           //Tell the user what the line converts to
                    fileContents.Add(echoText);
                    goto Success;   //Skip error catch
                }


                if (command.Equals("title", StringComparison.InvariantCultureIgnoreCase))   //Same as ECHO but for the "title" command
                {
                    string titleArg = string.Join(" ", words.Skip(1));
                    string titleText = "System.Console.Title(\"" + titleArg + "\");";
                    System.Console.WriteLine("\tConverts to: \t" + titleText);
                    fileContents.Add(titleText);    //Add title command to file
                    goto Success;   //Skip error catch
                }


                if (command.Equals("pause", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (command.Contains('>'))
                    {
                        string arg = command.TrimStart('>'); //Everything after the '>' is the arguments
                        if (arg.Equals("NUL", StringComparison.OrdinalIgnoreCase))
                        {
                            continue; //Exits the loop (skips over the writeline)
                        }
                        else
                        {
                            goto Error;
                        }

                    }
                    else
                    {
                        fileContents.Add("System.Console.WriteLine(\"Press any key to continue\");");
                        fileContents.Add("System.Console.Readkey();");
                    }
                    goto Success;
                }

                if (command.Equals("CLS", StringComparison.InvariantCultureIgnoreCase))
                {
                    fileContents.Add("System.Console.Clear");
                    goto Success;
                }

                if (command.Equals("IF", StringComparison.InvariantCultureIgnoreCase)) // "/I" would add "StringComparison.InvariantCultureIgnoreCase"
                {
                    //Oh noes. An error. This would be tricky to do, so, I will work on it later. For now, I'll just do the basics
                    goto Error;
                }

                if (command.Equals("SET", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (words[2].Contains('/'))
                    {
                        if (words.GetValue(2).Equals("/P"))
                        {
                            fileContents.Add("var " + words[words.Length] + " = System.Console.ReadLine();");   //CHECK THE MIDDLE ARGUMENT LATER
                        }
                        System.Console.WriteLine("Command not implimented. Leaving placeholder.");
                        fileContents.Add("///////////SET COMMAND PLACEHOLDER");
                    }
                    goto Error;
                }


                if (line.Equals("") || command.Equals("REM", StringComparison.InvariantCultureIgnoreCase)) //Comments and empty lines don't give errors
                {
                    goto Success;
                }



                //If command does not match one of the above
                goto Error; //Go to end of program and complain about what went wrong.


            Success: ;   //Go to marker for a successful command
                lineNumber++;   //Increase line to edit by 1 (again, for troubleshooting purposes)
            }

            //All lines have been looked over
            foreach (string line in fileContents)   //Look for printouts
            {
                if (line.Contains("System.Console.Write"))
                {

                }
            }

            //If all goes well, and the programmer wants to, write code to file
            bool writeToFile = true;
            string outputPath = "C:\\Users\\TRUTECH\\Desktop\\Output.txt";
            if (writeToFile)
            {
                System.Console.WriteLine("=====WRITING TO FILE=====");
                System.Console.WriteLine("Writing to file: " + outputPath);
                if (System.IO.File.Exists(outputPath))
                {
                    System.Console.WriteLine("Existing file contents deleted");
                    System.IO.File.WriteAllText(outputPath, ""); //Deletes all text in file. Possibly find a better method of doing so later
                }
                System.Console.WriteLine("\n\n");
                foreach (string line in fileContents)
                {
                    System.Console.WriteLine(line);
                    System.IO.File.AppendAllText(@outputPath, line + Environment.NewLine);
                }
                System.Console.WriteLine("\n\nWrite Completed");
            }








            goto positiveEnd;
        Error:
            System.Console.WriteLine("==========There was an error m80.==========\n\t\nLine number \t" + (lineNumber + 1));
        positiveEnd: ;
            // Keep the console window open in debug mode.
            Console.WriteLine("\n\n\n\nPress any key to exit.");
            System.Console.ReadKey();
        }
    }
}