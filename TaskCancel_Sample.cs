// using System;
using System.Diagnostics;

namespace VisuapProgrammer_sj
{
	internal class AsyncTest
	{
		static CancellationTokenSource tokenSource = new ();
		
		static void Main(string[] args)
		{
			var token = tokenSource.Token;
			
			Console.WriteLine($"start : thread id = {Thread.CurrentThread.ManagedThreadId}");
			
			Console.WriteLine($"call   > RunAsync : thread id = {Thread.CurrentThread.ManagedThreadId}");
			
			Task<TimeSpan>  t = RunAsync(token);
			
			Console.WriteLine($"return < RunAsync : thread id = {Thread.CurrentThread.ManagedThreadId}");
			Console.WriteLine($"t.Id got @ Main = {t.Id}");
			
			try{
				while(!t.IsCompleted){
					ConsoleKey key;
					if(IsKeyPressed(out key)){
						if(key == ConsoleKey.Spacebar)	tokenSource.Cancel();
					}
					
					if(t.Wait(200)) break;
					// Thread.Sleep(200);
					Console.Write(".");
				}
			}catch (AggregateException ex){
				Console.WriteLine($"> {ex.GetType()}");
				foreach (var inner in ex.Flatten().InnerExceptions){
					// Console.WriteLine("Type : {0}", inner.GetType());
					Console.WriteLine($"---{inner.GetType()}");
					Console.WriteLine($"---{inner.Message}");
					
					// TaskCanceledExceptionはOperationCanceledExceptionの派生
					if(inner.GetType() == typeof(OperationCanceledException)){
						Console.WriteLine("sj:OperationCanceledException.");
					}else if(inner.GetType() == typeof(TaskCanceledException)){
						Console.WriteLine("sj:TaskCanceledException.");
					}
					
				}
			}
			
			Console.WriteLine();
			
			
			Console.WriteLine($"Status of Task @ Main = {t.Status}");
			if(t.Status == TaskStatus.Canceled){
				Console.WriteLine("write process when canceled.");
			}
			
			try{
				Console.WriteLine($"t.Result at main = {t.Result}");
			}catch (AggregateException ex){
				Console.WriteLine($"> {ex.GetType()}");
				foreach (var inner in ex.Flatten().InnerExceptions){
					// Console.WriteLine("Type : {0}", inner.GetType());
					Console.WriteLine($"----{inner.GetType()}");
					Console.WriteLine($"----{inner.Message}");
					
					// TaskCanceledExceptionはOperationCanceledExceptionの派生
					if(inner.GetType() == typeof(OperationCanceledException)){
						Console.WriteLine("sj:OperationCanceledException.");
					}else if(inner.GetType() == typeof(TaskCanceledException)){
						Console.WriteLine("sj:TaskCanceledException.");
					}
					
				}
			}
			
			Console.WriteLine($"\nfinish : thread id = {Thread.CurrentThread.ManagedThreadId}");
		}
		
		static bool IsKeyPressed(out ConsoleKey key){
			key = ConsoleKey.A; // temp;
			
			bool ret = false;
			
			if (Console.KeyAvailable == true){
				ConsoleKeyInfo cki = new ConsoleKeyInfo();
				cki = Console.ReadKey(true);
				
				key = cki.Key;
				ret = true;
			}
			
			return ret;
		}
		
		static async Task<TimeSpan> RunAsync(CancellationToken token){
			Console.WriteLine($"> RunAsync : thread id = {Thread.CurrentThread.ManagedThreadId}");
			
			var watch = Stopwatch.StartNew();
			
			Task t =  Task.Run( () => Heavy(token), token );
			Console.WriteLine($"t.Id got @ RunAsync = {t.Id}");
			
			try{
				await t;
			}catch (OperationCanceledException ex){
			// }catch (Exception ex){
				Console.WriteLine($"--{ex.GetType()}");
				Console.WriteLine($"--{ex.Message}");
				throw;
			}
			Console.WriteLine($"Status of Task @ RunAsync = {t.Status}");
			
			
			watch.Stop();
			
			Console.WriteLine($"< RunAsync : thread id = {Thread.CurrentThread.ManagedThreadId}");
			
			// Thread.Sleep(1000);
			
			return watch.Elapsed;
		}
		
		static void Heavy(CancellationToken token){
			Console.WriteLine($"> Heavy : thread id = {Thread.CurrentThread.ManagedThreadId}");
			
			// Thread.Sleep(5000);
			
			try{
				for(int i = 0; i < 50; i++){
					Thread.Sleep(100);
					
					// if(token.IsCancellationRequested) throw new OperationCanceledException("sj test");
					token.ThrowIfCancellationRequested();
				}
			}catch (Exception ex){
				Console.WriteLine($"-{ex.GetType()}");
				Console.WriteLine($"-{ex.Message}");
				throw;
			}
			
			Console.WriteLine($"< Heavy : thread id = {Thread.CurrentThread.ManagedThreadId}");
			
			
		}
	}
}
