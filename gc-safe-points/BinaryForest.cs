using System;
using System.Threading;
using System.Collections.Generic;

using Mono.GetOptions;

public class RandomIntegerArray : System.IComparable<RandomIntegerArray>
{
	public static int LowerSizeBound = 1, UpperSizeBound = 10;
	
	protected static Random random = new Random ();
	
	private int [] internalArray;
	
	public RandomIntegerArray ()
	{
		internalArray = new int [RandomIntegerArray.random.Next (LowerSizeBound, UpperSizeBound)];
	}
	
	public int CompareTo (RandomIntegerArray other)
	{
		return this.internalArray [0].CompareTo (other.internalArray [0]);
	}
	
}

public class BinaryTree<T> where T : System.IComparable<T>
{

	protected class BinaryTreeNode
	{
		public BinaryTreeNode left, right, parent;
		public T info;
	}
	
	protected BinaryTreeNode root = null;
	protected int nodes;
	
	public int Count {
		get {
			return nodes;
		}
	}
	
	public void InsertElement (T info)
	{
		BinaryTreeNode node = new BinaryTreeNode ();
		node.info = info;
		if (root == null) {
			root = node;
			nodes++;
			return;
		}	
		BinaryTreeNode site = root, parent = null;
		
		while (site != null) {
		
			/* Don't insert duplicate nodes. */
			if (site.info.CompareTo (info) == 0)
				return;
	
			parent = site;
			if (site.info.CompareTo (info) > 0)
				site = site.left;
			else				
				site = site.right;
		}
		if (parent.info.CompareTo (info) > 0)
			parent.left = node;
		else
			parent.right = node;
		node.parent = parent;
		nodes++;
	}
	
	protected void InorderTraversal (List<T> list, BinaryTreeNode parent)
	{
		if (parent.left != null)
			InorderTraversal (list, parent.left);
		list.Add(parent.info);
		if (parent.right != null)
			InorderTraversal (list, parent.right);
	}
	
	public List<T> GetSorted ()
	{
		List<T> list = new List<T> ();
		if (root != null)
			InorderTraversal (list, root);
		return list;
	}
	
	public bool AssertCorrectness ()
	{
		List<T> list = GetSorted ();
		for (int i = 0; i < (list.Count - 1); i++) {
			if (list [i].CompareTo (list [i + 1]) > 0)
				return false;
		}
		return true;
	}
	
	protected BinaryTreeNode FindMinimum (BinaryTreeNode root)
	{
		while (root.left != null)
			root = root.left;
		return root;
	}
	
	protected void Delete (BinaryTreeNode node)
	{
		if (node == root) {
			/* Just to stuff */
			return;
		}
		if (node.left == null && node.right == null) {
			/* No children */
			if (node == node.parent.left)
				node.parent.left = null;
			else
				node.parent.right = null;
			nodes--;
		} else if (node.left == null ^ node.right == null) {
			/* One child */
			nodes--;
			BinaryTreeNode newNode = null;
			if (node.left != null)
				newNode = node.left;
			else
				newNode = node.right;
			if (node == node.parent.left)
				node.parent.left = newNode;
			else
				node.parent.right = newNode;
		} else {
			/* Two children */
			BinaryTreeNode replacement = FindMinimum (node.right);
			node.info = replacement.info;
			Delete (replacement);
		}
	}
	
	protected int NthNodeCount = 0;
	protected BinaryTreeNode SelectNthNode (BinaryTreeNode root, int n)
	{
		if (NthNodeCount == n) {
			return root;
		}
		if (root == null)
			return null;
		NthNodeCount++;
		BinaryTreeNode node = SelectNthNode (root.left, n);
		if (node == null)
			node = SelectNthNode (root.right, n);
		return node;
	}
	
	protected Random random = new Random ();
	protected BinaryTreeNode GetRandomNode ()
	{
		if (nodes == 0)
			return null;
		int nodeCount = random.Next (0, nodes);
		NthNodeCount = 0;
		return SelectNthNode (root, nodeCount);
	}
	
	public void DeleteRandomNode ()
	{
		Delete (GetRandomNode ());
	}
	
}

public class StressThread
{
	
	public static long TotalInsertions, TotalDeletions;
	protected static object StatisticsUpdateLock = new object ();
	public static Semaphore FinishNotifier = null;

	protected List<BinaryTree <int>> trees;
	protected int numInsertions, numDeletions, repetitions;
	protected double variance;
	protected Random random;
	protected bool printThreadSummary;
	
	protected StressThread (int numTrees, int numInsertions, int numDeletions, double variance, int repetitions,
	                        bool printThreadSummary)
	{
		trees = new List<BinaryTree<int>> ();
		for (int i = 0; i < numTrees; i++) {
			trees.Add (new BinaryTree <int> ());
		}
		this.numDeletions = numDeletions;
		this.numInsertions = numInsertions;
		this.variance = variance;
		this.repetitions = repetitions;
		this.printThreadSummary = printThreadSummary;
		this.random = new Random ();
	}
	
	protected void Execute ()
	{
		int localInsertions = 0, localDeletions = 0;
		for (int r = 0; r < repetitions; r++) {
			foreach (BinaryTree<int> tree in trees) {
				int thisInsertions, thisDeletions;
				if (random.NextDouble () < 0.5)
					thisInsertions = (int) (numInsertions * (1.0 - variance * random.NextDouble ()));
				else
					thisInsertions = (int) (numInsertions * (1.0 + variance * random.NextDouble ()));
				if (random.NextDouble () < 0.5)
					thisDeletions = (int) (numDeletions * (1.0 - variance * random.NextDouble ()));
				else
					thisDeletions = (int) (numDeletions * (1.0 + variance * random.NextDouble ()));
				if (thisDeletions >= thisInsertions)
					thisDeletions = thisInsertions - 1;
				for (int i = 0; i < thisInsertions; i++)
					tree.InsertElement (random.Next ());
				for (int i = 0; i < thisInsertions; i++)
					tree.DeleteRandomNode ();
				if (!tree.AssertCorrectness ()) {
					throw new Exception ("Inconsistency in binary tree.");
				}
				lock (StressThread.StatisticsUpdateLock) {
					StressThread.TotalDeletions += thisDeletions;
					StressThread.TotalInsertions += thisInsertions;
				}
				localDeletions += thisDeletions;
				localInsertions += thisInsertions;
				Console.Write (".");
			}
		}
		if (printThreadSummary)
			System.Console.WriteLine ("Total local insertions = {0}, local deletions = {1}\n", localInsertions, localDeletions);
		StressThread.FinishNotifier.Release();
	}

	public static Thread GetStressThread (int numTrees, int numInsertions, int numDeletions, double variance, int repeat,
	                                      bool printThreadSummary)
	{
		StressThread thread = new StressThread (numTrees, numInsertions, numDeletions, variance, repeat,
		                                        printThreadSummary);
		return new Thread (new ThreadStart (thread.Execute));
	}
}

public class MainClass
{

	protected class CommandLineOptions : Options
	{
		[Option ("The total number of threads to run this benchmark with.", 't')]
		public int threads;
		
		[Option ("The number of trees in each thread.", 'n')]
		public int trees;
		
		[Option ("The number of insertions per tree per thread.", 'i')]
		public int insertions;
		
		[Option ("The number of deletions per tree per thread.", 'd')]
		public int deletions;
		
		[Option ("The number of times the insertions / deletions cycle is to be repeated per tree per thread.", 'r')]
		public int repetitions;
		
		[Option ("The amount the actual number of insertions / deletions is allowed to vary.", 'v')]
		public double variance;
		
		[Option ("True if each thread is to print its own statistics along with the final score.", 's')]
		public bool individual_thread_summary;
		
		[Option ("Prompt before beginning the test.", 'p')]
		public bool prompt;
		
		public CommandLineOptions ()
		{
			threads = 50;
			trees = 12;
			insertions = 666;
			deletions = 222;
			repetitions = 10;
			variance = 0.3;
			individual_thread_summary = false;
			prompt = false;
		}
		
	}
	
	public static string Sanitize (long num)
	{
		string basic = num.ToString ();
		int len = basic.Length;
		for (int i = 0; i < ((3 - len % 3) % 3); i++)
			basic = " " + basic;
		string result = "";
		for (int i = 0; i < basic.Length / 3; i++) {
			result += basic.Substring (i * 3, 3);
			if (i != (basic.Length / 3 - 1))
				result += ",";
		}
		return result.Trim ();
	}

	public static void Main (string []argv)
	{
		CommandLineOptions options = new CommandLineOptions ();
		options.ProcessArgs (argv);
		
		DateTime previous = DateTime.Now;
		StressThread.FinishNotifier = new Semaphore (0, options.threads);
		
		if (options.prompt) {
			Console.Write ("Press a key to start the benchmarking ... ");
			Console.ReadKey ();
			Console.WriteLine ();
		}
		
		for (int i = 0; i < options.threads; i++) {
			Thread t = StressThread.GetStressThread (options.trees, options.insertions, options.deletions,
			                                         options.variance, options.repetitions,
			                                         options.individual_thread_summary);
 			t.Start ();
		}
		
		for (int i = 0; i < options.threads; i++) {
			StressThread.FinishNotifier.WaitOne ();
			Console.Write ("*");
		}
		
		Console.WriteLine ("\nDone waiting for sub-threads.");
		
		TimeSpan interval = DateTime.Now - previous;
		
		Console.WriteLine ("Benchmarking Complete!");
		Console.WriteLine ("Total Insertions = {0}, Total Deletions = {1}", Sanitize (StressThread.TotalInsertions),
		                   Sanitize (StressThread.TotalDeletions));
		double seconds = (interval.Seconds * 1000.0 + interval.Milliseconds) / 1000.0;
		int insertionsPerSecond = (int) (StressThread.TotalInsertions / seconds);
		int deletionsPerSecond = (int) (StressThread.TotalDeletions / seconds);
		Console.WriteLine ("{0} insertions per second, {1} deletions per second.", Sanitize (insertionsPerSecond),
		                   Sanitize (deletionsPerSecond));
		
	}
	
}

