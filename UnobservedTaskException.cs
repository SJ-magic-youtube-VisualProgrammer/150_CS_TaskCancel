// using System;
// using System.Diagnostics;

// using System.Threading;
// using System.Threading.Tasks;


namespace VisuapProgrammer_sj
{
	internal class AsyncTest
	{
		/******************************
		******************************/
		static void Main()
		// static async Task Main()
        {
			/********************
			TaskScheduler.UnobservedTaskExceptionのイベントが発火されるタイミング = ガベージコレクション（GC）によってTaskクラスが破棄されるとき
			********************/
			// TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
			
			/********************
			********************/
			// Task t = Task.Run( () => { throw new InvalidOperationException("-- sj : Task1 --");	} );
			Task.Run( () => { throw new InvalidOperationException("-- sj : Task1 --");	} );
			Task.Run( () => { throw new InvalidCastException("-- sj : Task2 --");			} );
			// Task.Run( () => { throw new OperationCanceledException("-- sj : Task3 --");	} );
			
			Thread.Sleep(300); //--- タスクの完了を待機
			
			/********************
			********************/
			Console.WriteLine("space : call GC.");
			Console.WriteLine("q     : quit.");
			
			/* // 例外にaccess
			try{
				// t.Wait();
				await t;
			}catch(Exception e){
				Console.WriteLine($"{e.GetType()}");
				Console.WriteLine($"{e.Message}");
			}
			
			// Console.WriteLine($"Task.Exception = {t.Exception}");
			*/
			
			/*
			bool b_Run = true;
			while(b_Run){
				ConsoleKey key;
				if(IsKeyPressed(out key)){
					switch(key){
						case ConsoleKey.Spacebar:
							// TaskScheduler.UnobservedTaskExceptionのイベントが発火されるタイミング = ガベージコレクション（GC）によってTaskクラスが破棄されるとき
							Console.WriteLine("> call GC.");
							
							GC.Collect();					//--- タスクインスタンスを回収
							GC.WaitForPendingFinalizers();	//--- Finalizeを強制的に呼び出す
							break;
							
						case ConsoleKey.Q:
							Console.WriteLine("> Quit");
							b_Run = false;
							break;
					}
				}
			}
			*/
			
			/********************
			********************/
            Console.WriteLine("End");
        }
		
		/******************************
		******************************/
        static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
			foreach (var inner in e.Exception.Flatten().InnerExceptions)
            {
                Console.WriteLine(inner.Message);
                Console.WriteLine("Type : {0}", inner.GetType());
            }
			// e.SetObserved(); //--- 処理済みとしてマークする
			
			/********************
			イベントを購読しても、何もしないと、例外は無視され、そのままアプリケーションは継続される。
			********************/
			// Environment.Exit(1);
        }
		
		/******************************
		******************************/
		static bool IsKeyPressed(out ConsoleKey key){
			key = ConsoleKey.A; // temp;
			
			bool ret = false;
			
			if (Console.KeyAvailable == true){
				ConsoleKeyInfo cki = new ConsoleKeyInfo();
				cki = Console.ReadKey(true); // 引数 : 押されたキーをコンソール ウィンドウに表示するかどうかを決定します。 押されたキーを表示しない場合は true。それ以外の場合は false。
				key = cki.Key;
				ret = true;
			}
			
			return ret;
		}
	}
}

