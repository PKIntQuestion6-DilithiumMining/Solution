using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilithium
{
    class Program
    {
        // create linked list that will contain dilithium quantities of strip
        static LinkedList<int> dilithium_raw_data = new LinkedList<int>();
        static int max_dilithium = 0; // max dilithium found
        // Linked list containing the optimum traverse path
        static LinkedList<DilithiumOutputData> max_dilithium_output_data_list;

        static int Main(string[] args)
        {
            // Check if input arguments were supplied
            int numItems = args.Length;
            if (numItems == 0)
            {
                System.Console.WriteLine("Enter the dilithium data for the strip in sequential order, example:  Dilithium 206, 140, 300, 52, 107");
                return 1;
            }

            // Parse the arguments
            int i;
            for(i = 0; i < numItems; i++)
            {
                int dilithium_val;
                if (!int.TryParse(args[i], out dilithium_val))
                {
                    System.Console.WriteLine(String.Format("%s is not a valid number", args[i]));
                    return 1;
                }
                // Add a new item to the linked list
                dilithium_raw_data.AddLast(dilithium_val);
            }

            // We have our dilithium data. 

            // Create linked list that contains the following items: [1] dilithium square index, [2] dilithium square amount
            LinkedList<DilithiumOutputData> dilithium_output_data_list = new LinkedList<DilithiumOutputData>();

            // Now let's crunch the data
            // Let's start with the first node
            LinkedListNode<int> node = dilithium_raw_data.First;
            int sum = node.Value; // Initialize
            int cur_index = 0; // Initialize
            // Start with the first node and recursively traverse the nodes.
            dilithium_output_data_list.AddLast(new DilithiumOutputData(cur_index, node.Value));
            TraverseNextNode(node, cur_index, sum, dilithium_output_data_list);

            // Alternate choice - start with the second node
            node = node.Next;
            if (node != null)
            {
                cur_index++;
                sum = node.Value; // Initialize
                dilithium_output_data_list.Clear();
                dilithium_output_data_list.AddLast(new DilithiumOutputData(cur_index, node.Value));
                TraverseNextNode(node, cur_index, sum, dilithium_output_data_list);
            }

            StringBuilder str = new StringBuilder();
            LinkedListNode<DilithiumOutputData> nodeOutputData = max_dilithium_output_data_list.First;
            while(nodeOutputData != null)
            {
                str.AppendFormat("[{0}, {1}]; ", nodeOutputData.Value.GetIndex(), nodeOutputData.Value.GetAmount());
                nodeOutputData = nodeOutputData.Next;
            }

            System.Console.WriteLine(String.Format("Optimum output = {0}; Traverse Path = {1}", max_dilithium, str));
            // Wait for user input before exiting
            System.Console.ReadLine();
            return 0;
        }

        // Data for each square - Note down the index of the square and the amount of dilithium in that square
        struct DilithiumOutputData
        {
            int index;
            int amount;
            public DilithiumOutputData(int _index, int _amount)
            {
                index = _index;
                amount = _amount;
            }
            public int GetIndex() { return index; }
            public int GetAmount() { return (amount); }
        }

        static void TraverseNextNode(LinkedListNode<int> node, int cur_index, int sum, LinkedList<DilithiumOutputData> dilithium_output_data_list)
        {
            node = node.Next;
            if (node == null)
            {
                ProcessEndOfList(sum, dilithium_output_data_list);
                return;
            }
            cur_index++;
            // Skip this node
            node = node.Next;
            if (node == null)
            {
                ProcessEndOfList(sum, dilithium_output_data_list);
                return;
            }
            cur_index++;
            // Create a copy for the alternate choice later.
            int sum2 = sum;
            LinkedList<DilithiumOutputData> dilithium_output_data_list2 = new LinkedList<DilithiumOutputData>(dilithium_output_data_list);


            dilithium_output_data_list.AddLast(new DilithiumOutputData(cur_index, node.Value));
            sum += node.Value;
            // Recursively traverse the next node
            TraverseNextNode(node, cur_index, sum, dilithium_output_data_list);


            // Alternate choice - go to the next node - Use fresh copy of output data for this traverse
            node = node.Next;
            if (node != null)
            {
                cur_index++;
                sum2 += node.Value;
                dilithium_output_data_list2.AddLast(new DilithiumOutputData(cur_index, node.Value));
                TraverseNextNode(node, cur_index, sum2, dilithium_output_data_list2);
            }
        }

        static void ProcessEndOfList(int sum, LinkedList<DilithiumOutputData> dilithium_output_data_list)
        {
            // Compare the current sum with the maximum obtained so far in the search.
            if (max_dilithium < sum)
            {
                max_dilithium = sum;
                max_dilithium_output_data_list = new LinkedList<DilithiumOutputData>(dilithium_output_data_list);
            }
            return;
        }

    }
}
