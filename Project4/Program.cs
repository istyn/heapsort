/////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Name                     :Isaac Styles
// Department Name : Computer and Information Sciences 
// File Name                :Program.cs
// Purpose                  :input a binary file with length at top,
//                            perform on disk minheapsort, and display in sorted order.
//							
// Author			        : Isaac Styles, styles@goldmail.etsu.edu
// Create Date	            : Nov 14, 2015
//
//-----------------------------------------------------------------------------------------------------------
//
// Modified Date	: Dec 10, 2015
// Modified By		: Isaac Styles
//
/////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4
{
    class Program
    {
        static void Main(string[] args)
        {
            string address, strHeap, strMerge, dir;
            int heapSize;                               //size of heap
            int mergeFiles;                             //number of files to merge
            Console.WriteLine("Enter the address of the file: ");
            address = Console.ReadLine();               //get source file address
            Console.WriteLine("   Enter the size of the heap: ");
            strHeap = Console.ReadLine();               //get size of heap
            Int32.TryParse(strHeap, out heapSize);      //parse size of heap
            Console.WriteLine("Enter the number of files to merge: ");
            strMerge = Console.ReadLine();              //get files to merge
            Int32.TryParse(strMerge, out mergeFiles);   //parse files to merge
            dir = address.Substring(0, address.LastIndexOf('\\'));  //dir is working directory

            Stopwatch sw = Stopwatch.StartNew();

            if (!Directory.Exists(dir + "\\Temp"))      //delete temp dir if it exists
            {
                Directory.CreateDirectory(dir + "\\Temp");
            }
            String sortedFileName = dir;                //place resultant file in working directory
            dir = dir + "\\Temp";                       //dir is working temp directory

            CreateBinaryFile unsorted = new CreateBinaryFile(); //create class

            unsorted.readSourceFile(address, dir, heapSize);    //read in the source file
            unsorted.createSortFile(address, dir, sortedFileName, heapSize, mergeFiles);    //perform heapsort
            Directory.Delete(dir, true);                //delete the temp dir
            sw.Stop();
            Console.WriteLine("Time Used: " + sw.Elapsed.TotalMilliseconds / 1000.0 + " seconds");
            Console.WriteLine("\tPrint results? (Y or N then ENTER): ");
            int print = Console.Read();
            if (print == 121 || print == 89)       //if user says 'y' or 'Y'
            {
                Console.Write(unsorted.ToString());
            }
        }
    }
}
