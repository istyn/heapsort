/////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Name                     :Isaac Styles
// Department Name : Computer and Information Sciences 
// File Name                :MinHeap.cs
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4
{
    /// <summary>
    /// Adds ints to the heap and sorts them in ascending order
    /// </summary>
    public class MinHeap
    {
        private int maxSize;            //max size of heap
        private int size;               //current size of heap
        private int[] h;                //array of ints in the heap
        int index;                      //pointer into heap for insert


        /// <summary>
        /// Initializes a new instance of the <see cref="MinHeap"/> class.
        /// </summary>
        /// <param name="maxSize">The maximum size of the heap.</param>
        public MinHeap(int maxSize)
        {
            this.size = 0;                  //no ints in heap
            this.maxSize = maxSize + 1;     //allow temp space in heap for root = 0
            this.h = new int[this.maxSize];	    //init the heap array
        }
        //Sorts a given array of items.
        /// <summary>
        /// Sorts the heap.
        /// </summary>
        /// <returns>int[]</returns>
        /// 
        public int[] heapSort()
        {
            int[] result = new int[maxSize];	    //create an empty array
            for (int i = size; i >= 1; i--)
            {
                result[i] = extractMin();	        //add strings from heap to empty array
            }
            return result;
        }


        /// <summary>
        /// Extract the min from the top of the heap.
        /// </summary>
        /// <returns>int min</returns>
        public int extractMin()
        {
            int min = h[1];                             //larger holds first int
            h[1] = h[size--];			                    //swap top item with the first item and reduce heap size by 1
            minheapify(1, size);	                    //call minheapify to fix the heap
            return min;
        }

        /// <summary>
        /// Iterates through the heap, swapping smaller children with the parent
        /// </summary>
        /// <param name="i">The index in heap.</param>
        /// <param name="size">The size of heap.</param>
        /// 
        private void minheapify(int i, int size)
        {
            int larger;                                  //larger holds the largest in the parent/child relation

            int l = left(i);
            int r = right(i);
            if ((l <= size) && (h[i]) < h[l])           //if left child bigger than parent
            {
                larger = l;
            }
            else
            {
                larger = i;
            }
            if ((r <= size) && (h[r] > h[larger]))      //if right child bigger than parent
            {
                larger = r;                             //right child is larger
            }
            //If parent is not smallest, swap heap[index] with heap[larger]
            if (larger != i)
            {
                int temp = h[i];                         //temp storage for current int
                h[i] = h[larger];                       //parent equals larger
                h[larger] = temp;                       //child equals old parent
                minheapify(larger, size);              //recurse to make sure order is intact
            }

        }
        /// <summary>
        /// Prints this heap array.
        /// </summary>
        /// 
        public override string ToString()
        {
            string str = "";
 	        for (int i = 0; i <= size; i++)
            {
                str+= h[i] + "\r\n";
            }
            return str;
        }


        /// <summary>
        /// Insert a new int into the heap
        /// </summary>
        /// <param name="item"></param>
        public void Insert(int item)
        {

            int temp;                                   //temp incase swap happens
            h[++size] = item;                           //inc size and add int
            index = size;                               //set pointer to current int


            while (Parent(index) > 0 && (h[index] > h[Parent(index)]))//If not at root and heap index greater than parent index
            {
                temp = h[Parent(index)];               //swap parent of index with heap of index
                h[Parent(index)] = h[index];
                h[index] = temp;
                index = Parent(index);	                //current index gets parent index
            }
        }

        /// <summary>
        /// Determine index of the parent of the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>int of the parent</returns>
        /// 
        private int Parent(int index)
        {
            return index >> 1;	//parent of index (/2)
        }

        /// <summary>
        /// Lefts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>int</returns>
        /// 
        private int left(int index)
        {
            return index << 1;		//left child of index (*2)
        }

        /// <summary>
        /// Rights the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>int</returns>
        private int right(int index)
        {
            return (index << 1) + 1;	//right child of index (*2 + 1)
        }
    }

}
