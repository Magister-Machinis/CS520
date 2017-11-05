/*
 * https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/ because i forgot the syntax again
 * https://stackoverflow.com/questions/11463734/split-a-list-into-smaller-lists-of-n-size found an algorithm for splitting lists that kicks the shit of what I was thinking of
 * https://stackoverflow.com/questions/2632753/rounding-values-up-or-down-in-c-sharp for rounding values
 * https://stackoverflow.com/questions/20044730/convert-character-to-its-alphabet-integer-position funky little trick for playing with characters
 * John Fromholtz
 * CS520 Fall 2017
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace SimulationCore
{

    class Memblock //creating linked tree to keep track of memory allocations
    {
        public Memblock Parent;
        public List<Memblock> Children;
        public bool Container; //whether this is holding for smaller items (true) or holding a value itself
        public int Start;
        public int End;
        public int Size;
        

        

        public  Memblock( int start, int end, Memblock parent = null)
        {
            Container = false;
            Parent = parent;
            Start = start;
            End = end;
            Size = end - start;
            Children = new List<Memblock>();
        }

        public void Split()
        {
            Container = true;
            Children.Add(new Memblock(Start, Convert.ToInt32(Math.Floor(Convert.ToDouble(Size / 2))), this));
            Children.Add(new Memblock(Convert.ToInt32(Math.Floor(Convert.ToDouble(Size / 2)))+1, End, this));
        }

        public void Merge()
        {
            Container = false;
            this.Children[0] = null;
            this.Children[1] = null;
            Children.Clear();
        }

    }
    class Program
    {

        static void Main(string[] args)
        {
            
            Stopwatch bigtimer = new Stopwatch(); //timer for runtime statistics
            bigtimer.Start();

            

            Console.WriteLine("Input size of memory Block (in KiloBytes): ");
            int memsize = Convert.ToInt32(Console.ReadLine());
            char[] Memarray = new char[memsize];
            for (int count = 0; count < Memarray.Length; count++)
            {
                Memarray[count] = 'u';
                
            }

            Memblock Root = new Memblock(0, Memarray.Length);
            int[] MemInput = new int[] { 20,  35,  90,  40,  240 };
            char[] InputName = new char[] { 'a', 'b', 'c', 'd', 'e' };
            int[] MemOutput = new int[] {  40,  20,  90,  35,  240 };
            char[] OutputName = new char[] { 'd', 'a', 'c', 'b', 'e' };
            string filepath = @".\results.txt";
            filepath = Path.GetFullPath(filepath);
            


            for (int count = 0; count < MemInput.Length; count++) //input loop
            {

            WritetoFile(filepath, Memarray);
            Memarray = Insert(Root, MemInput[count], InputName[count], Memarray);

            }

            for (int count = 0; count < MemOutput.Length; count++) //output loop
            {

                WritetoFile(filepath, Memarray);
                Memarray = Extract(Root, OutputName[count], Memarray);

            }
            WritetoFile(filepath, Memarray);
            bigtimer.Stop();
            TimeSpan rundurationraw = bigtimer.Elapsed;

            Console.WriteLine("Runtime " + rundurationraw);
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        public static char[] Extract(Memblock Root, char name, char[] Memarray)
        {
            //find the target block to release
            Memblock target = Finder(Root, name, Memarray);
            for(int count = target.Start; count < target.End; count++) //marking the space as available
            {
                Memarray[count] = 'u';
            }
            Memarray = Consolidate(Root, Memarray); //merging sibling unallocated blocks
            return Memarray;
        }

        public static char[] Consolidate(Memblock subject, char[] Memarray) //recursively checks for sibling pairs who are both unallocated and merges them
        {
            if (subject.Container == true)
            {
                try
                {
                    if (subject.Children[0] != null)
                    {
                        Memarray = Consolidate(subject.Children[0], Memarray);
                    }
                }
                catch { }

                try
                {
                    if (subject.Children[1] != null)
                    {
                        Memarray = Consolidate(subject.Children[1], Memarray);
                    }
                }
                catch { }
                return Memarray;
            }
            else
            {
                Memblock targetp = subject.Parent;
                try
                {

                    if (Memarray[targetp.Children[0].Start] == 'u' & Memarray[targetp.Children[1].Start] == 'u')
                    {
                        targetp.Merge();
                    }
                }
                catch { }
                return Memarray;
            }
            
        }

        public static Memblock Finder(Memblock subject, char name, char[] Memarray)
        {
            Memblock suba;
            Memblock subb;
            if (subject.Container == true)
            {
                suba= Finder(subject.Children[0], name, Memarray);
                subb= Finder(subject.Children[1], name, Memarray);
                if (Memarray[suba.Start] == name)
                {
                    return suba;
                }
                else
                {
                    return subb;
                }
                
            }
            else
            {
                return subject;
            }

            
        }

        public static void WritetoFile(string filepath, char[] Memarray)
        {
            if(File.Exists(filepath) == false)
            {
                using (StreamWriter output = File.CreateText(filepath))
                {
                    output.WriteLine("test results");
                }
            }
            using (StreamWriter output = File.AppendText(filepath))
            {
                output.WriteLine("Snapshot of state taken at: " + DateTime.Now);
                for (int count = 0; count < Memarray.Length; count ++)
                {
                    string linetowrite = " ";
                    if(Memarray[count] != 'u')
                    {
                        int charnum = (int)(Memarray[count] % 32); //adjusts offset of each named block for readability, i wish i had thought of this myself
                        for (int innercount = 0; innercount <  charnum; innercount++)
                        {
                            linetowrite += " ";
                        }
                    }
                    linetowrite += Memarray[count];
                    output.WriteLine(linetowrite);
                }
                
            }
        }

        public static char[]  Insert(Memblock Root, int size, char name, char[] Memarray)
        {
            //find smallest unallocated block into which this thing can fit
            
            Memblock targeta = Findhome(Root, size);

            // split smallest unallocated blocked to smallest piece that can still fit the block

            Memblock targetb = Sizehome(targeta, size);

            for(int count = targetb.Start; count < targetb.End; count++) //marking the allocation space as used
            {
                Memarray[count] = name;
            }
            return Memarray;
        }

        public static Memblock Sizehome(Memblock subject, int size) //recursively splits selected block to smallest fragments that still fit 'size'
        {
            if (subject.Size < size)
            {
                Memblock targetp = subject.Parent;
                targetp.Merge();
                return targetp;

            }
            else
            {
                subject.Split();
                return Sizehome(subject.Children[0], size);
            }
        }
        public static Memblock Findhome(Memblock subject, int size) // recursively finds the smallest chunk into which 'size' will fit
        {
            Memblock suba;
            Memblock subb;
            if (subject.Container == true)
            {
                suba = Findhome(subject.Children[0], size);
                subb = Findhome(subject.Children[1], size);
            }
            else
            {
                return subject;
            }
            if(subb.Size < size)
            {
                return suba;
            }
            else if(suba.Size < size)
            {
                return subb;
            }
            else if (suba.Size <= subb.Size)
            {
                return suba;
            }
            else
            {
                return subb;
            }
        }

    }
    
}
 