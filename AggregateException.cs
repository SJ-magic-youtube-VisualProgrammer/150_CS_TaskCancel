// using System;
// using System.Diagnostics;

// using System.Threading;
// using System.Threading.Tasks;


namespace VisuapProgrammer_sj
{
	public class MyException : Exception
	{
		public MyException(string msg) : base(msg) { }
	}
	
	internal class AggregateExceptionExample
	{
		public static void Run_0() 
		// public static async Task Run_0() 
		{
			var t = Task.Run(() => { throw new Exception("Error1"); });
			// var t = Task.Run(() => { throw new OperationCanceledException("Canceled"); });
			
			try{
				t.Wait();
				// await t;
				
			}catch (AggregateException exc){
				Console.WriteLine($"> {exc.GetType()}");
				
				foreach (var e in exc.InnerExceptions){
					Console.WriteLine($"-- {e.GetType()}");
					Console.WriteLine(e.Message);
					
					/*
					if(e.GetType() == typeof(TaskCanceledException)){
						Console.WriteLine("sj:TaskCanceledException.");
					}
					*/
				}
				/********************
				System.Exception
				Error1
				********************/
				
				/********************
				System.Threading.Tasks.TaskCanceledException
				A task was canceled.
				********************/
			}catch(Exception exc){
				Console.WriteLine($"-- {exc.GetType()}");
				Console.WriteLine(exc.Message);
				
				/********************
				System.Exception
				Error1
				********************/
				
				/********************
				System.OperationCanceledException
				Canceled
				********************/
			}
		}
		
		public static void Run_1()
		{
			var tasks = new List<Task>();
			
			tasks.Add(Task.Run(() => { throw new Exception("Error1"); }));
			tasks.Add(Task.Run(() => { throw new Exception("Error2"); }));
			tasks.Add(Task.Run(() => { throw new Exception("Error3"); }));
			
			try{
				Task.WaitAll(tasks.ToArray());
			}catch (AggregateException exc){
				Console.WriteLine($"> {exc.GetType()}");
				
				foreach (var e in exc.InnerExceptions){
					Console.WriteLine($"-- {e.GetType()}");
					Console.WriteLine(e.Message);
				}
				/********************
				System.Exception
				Error1
				System.Exception
				Error2
				System.Exception
				Error3
				********************/
			}
		}
		
		public static void Run_2()
		{
			var tasks = new List<Task>();
	
			tasks.Add(Task.Run(() => { throw new Exception("Error1"); }));
			tasks.Add(Task.Run(() => { throw new MyException("MyError"); }));
			tasks.Add(Task.Run(() => { throw new Exception("Error2"); }));
			
			try{
				Task.WaitAll(tasks.ToArray());
			}catch (AggregateException exc){
				Console.WriteLine($"> {exc.GetType()}");
				
				// public void Handle (Func<Exception,bool> predicate);
				exc.Handle((e) =>
				{
					/********************
					true	：例外処理した			(外に出ていかない)
					false	：例外処理していない	(外に出ていく)
					********************/
					return e.GetType() == typeof(MyException);
					// return e.Message == "Error1";
				});
			}
		}
		
		public static void Run_3()
		{
			var tasks = new List<Task>();
			
			tasks.Add(Task.Run(() => { throw new Exception("Error1"); }));
			tasks.Add(Task.Run(() => { throw new Exception("Error2"); }));
			tasks.Add(Task.Run(() => {
				var tasks2 = new List<Task>();
				
				tasks2.Add(Task.Run(() => { throw new Exception("Error3-1"); }));
				tasks2.Add(Task.Run(() => { throw new Exception("Error3-2"); }));
				tasks2.Add(Task.Run(() => { throw new Exception("Error3-3"); }));
				
				Task.WaitAll(tasks2.ToArray());
			}));
	
			try{
				Task.WaitAll(tasks.ToArray());
			}catch (AggregateException exc){
				Console.WriteLine($"> {exc.GetType()}");
				
				// foreach (var e in exc.InnerExceptions){
				foreach (var e in exc.Flatten().InnerExceptions){
					Console.WriteLine($"-- {e.GetType()}");
					Console.WriteLine(e.Message);
				}
				/********************
				-- System.Exception
				Error1
				-- System.Exception
				Error2
				-- System.Exception
				Error3-1
				-- System.Exception
				Error3-2
				-- System.Exception
				Error3-3
				********************/
			}
		}
	}
	
	internal class AsyncTest
	{
		/******************************
		******************************/
		static void Main()
        {
			AggregateExceptionExample.Run_0();
			
			/*
			try{
				AggregateExceptionExample.Run_2();
			}catch (AggregateException exc){
				Console.WriteLine($"> {exc.GetType()}");
				
				foreach (var e in exc.InnerExceptions){
					Console.WriteLine($"-- {e.GetType()}");
					Console.WriteLine(e.Message);
				}
			}
			*/
			
			Console.WriteLine("input any key to Finish.");
			Console.ReadLine();
        }
	}
}


