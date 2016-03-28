/////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Name                     :Isaac Styles
// Department Name : Computer and Information Sciences 
// File Name                :CreateBinaryFile.cs
// Purpose                  :Read a binary file from disk, write a sorted binary file to
//                             disk, and merge temp files
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4
{
    public struct fileName
    {
        public int iNum;
        public string strName;

        public fileName(int iNum, string strFileName)
        {
            this.iNum = iNum;
            this.strName = strFileName;
        }
    }
    class CreateBinaryFile
    {
        private long fileLength;
        private MinHeap min;
        public string sortedText = "";

        /// <summary>
        /// Reads the binary source file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="directory">The folder path.</param>
        /// <param name="heapSize">Size of the heap.</param>
        /// <param name="mergeFiles">Number files to merge.</param>

        public void readSourceFile(String fileName, String directory, int heapSize)
        {
            try
            {
                using (BinaryReader b = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    long position = 0;                                  //byte pointer into file
                    int i = 1;                                          //index for filename
                    fileLength = (int)b.BaseStream.Length;         //get the length of the file

                    BinaryWriter w = new BinaryWriter(File.Create(directory + "\\Temp" + i + ".bin"));

                    List<int> tempList = new List<int>();               //holds the values

                    while (position < fileLength)                       //so long as EOF hasn't been reached
                    {
                        if (tempList.Count() == heapSize)               //if heap is full
                        {
                            tempList.Sort();
                            for (int j = 0; j < heapSize; j++)          //write sorted file to disk                        
                                w.Write(tempList[j]);
                            w.Close();
                            i++;                                        //inc fileName index
                            w = new BinaryWriter(File.Create(directory + "\\Temp" + i + ".bin"));//create next temp file
                            tempList = new List<int>();                 //clear the old list
                        }
                        tempList.Add(b.ReadInt32());                    //read an int and place in list
                        position += 4;                                  //increment to next int in file
                    }

                    if (tempList.Count() > 0)               //if odd number of items remaining in the file, write final file
                    {
                        tempList.Sort();
                        for (int j = 0; j < tempList.Count(); j++)
                            w.Write(tempList[j]);
                        w.Close();
                    }
                    b.Close();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read: ");
                Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Creates the sort file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="directory">The folder path.</param>
        /// <param name="sortedFile">The sorted file.</param>
        /// <param name="heapSize">Size of the heap.</param>
        /// <param name="mergeFiles">The merge files.</param>

        public void createSortFile(String fileName, String FolderPath, String sortedFile, int heapSize, int mergeFiles)
        {

            int size = (int)fileLength / 4;                 //size = number of ints in source file
            int[] numbers = new int[size];                  //array holds all values from the source file
            int filesToTraverse = size / heapSize;          //number of temp files
            int k = heapSize * mergeFiles;                  //k = size of heap 
            min = new MinHeap(k);
            String filename = "";                           //filename of temp binary file
            bool EOF = false;                               //flag for end of files
            int fNums = 0;
            int position = 0;
            int i = 1;                                      //first pointer to temp files
            int track = 0;                                  //offset from first pointer to files
            int trackFiles = mergeFiles + track;            //how many files to merge

            fileName[] fName = new fileName[mergeFiles * heapSize]; //make struct][], set filname to total size

            for (; i <= trackFiles; )
            {
                int currentRead = i;                        //pointer to current readfile
                int currentWrite = trackFiles;              //pointer to current writefile
                for (; currentRead <= currentWrite; currentRead++) //read in files to merge
                {
                    filename = FolderPath + "\\Temp" + currentRead + ".bin";
                    try                                     //place each file in struct and repeat til end of file
                    {
                        using (BinaryReader b = new BinaryReader(File.Open(filename, FileMode.Open)))
                        {
                            while (position < (int)b.BaseStream.Length)
                            {
                                fName[fNums] = new fileName() { iNum = b.ReadInt32() };
                                fNums++;                    //inc file ptr to merging files
                                position += 4;              //inc byte ptr by sizeof int
                                b.BaseStream.Seek(position, SeekOrigin.Begin);
                            }
                            b.Close();
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("The file could not be read: ");
                        Console.WriteLine(e.Message);
                    }
                    position = 0;                           //reset the position
                }
                for (int j = 0; j < k; j++)                 //place ints from merge files in heap
                {
                    min.Insert(fName[j].iNum);
                }

                try                                         //write the merged heaps to file
                {
                    using (BinaryWriter b = new BinaryWriter(File.Create(FolderPath + "\\Temp" + currentWrite + ".bin")))
                    {
                        numbers = min.heapSort();

                        for (int h = 1; h < numbers.Length; h++)
                        {
                            b.Write(numbers[h]);
                        } b.Close();
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("The file could not be read: ");
                    Console.WriteLine(e.Message);
                }


                if (EOF == true)
                {
                    break;
                }
                i += (mergeFiles - 1);                      //increment current file to beyond merged files
                track += (mergeFiles - 1);                  //place pointer to merge files at current file
                trackFiles = mergeFiles + track;            //set end of merge files
                if (trackFiles > (size / heapSize))         //if merging file files
                {
                    trackFiles -= (trackFiles - (size / heapSize)); //set end of merge files to end of files
                    bool isInt = ((double)size / heapSize) == Math.Truncate((double)size / heapSize);   //if no items are left over then EOF is reached
                    if (isInt == false)
                    {
                        trackFiles++;
                    }

                    EOF = true;
                }

                k = 0;                                      //reset heap size

                for (int i2 = i, t2 = trackFiles; i2 <= t2; i2++)  //read next files to continue the process
                {
                    filename = FolderPath + "\\Temp" + i2 + ".bin";

                    using (BinaryReader b = new BinaryReader(File.Open(filename, FileMode.Open)))
                    {
                        k += (int)b.BaseStream.Length / 4;
                        b.Close();
                    }
                }
                fName = new fileName[k];
                min = new MinHeap(k);
                position = 0;
                fNums = 0;
            }
            //if the sorted file exists, delete the previous one
            if (File.Exists(sortedFile + "\\" + size + "sorted.bin"))
            {
                File.Delete(sortedFile + "\\" + size + "sorted.bin");
            }                               //rename to proper format
            File.Copy(FolderPath + "\\Temp" + trackFiles + ".bin", sortedFile + "\\" + size + "sorted.bin");
            sortedText = (sortedFile + "\\" + size + "sorted.bin").ToString();
        }
        public override string ToString()
        {
            return min.ToString();
        }
    }
}
