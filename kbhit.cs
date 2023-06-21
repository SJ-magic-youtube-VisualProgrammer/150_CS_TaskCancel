/************************************************************
************************************************************/
// #define NG_CASE

/************************************************************
************************************************************/
// using System;
// using System.Diagnostics;
// using System.Threading;


/************************************************************
************************************************************/
namespace VisuapProgrammer_sj
{
	internal class AsyncTest
	{
		/******************************
		******************************/
		static void Main(string[] args)
		{
#if !NG_CASE
			Console.WriteLine("OK case");
#else
			Console.WriteLine("NG case");
#endif

			Console.WriteLine("input any key.");
			Console.WriteLine("hit 'q' to quit");
			
			bool b_Run = true;
			while(b_Run){
#if !NG_CASE
				/********************
				OK case
				********************/
				ConsoleKey key;
				if(IsKeyPressed(out key)){
					switch(key){
						case ConsoleKey.Spacebar:
							Console.WriteLine("spacebar");
							break;
							
						case ConsoleKey.Q:
							Console.WriteLine("Quit");
							b_Run = false;
							break;
					}
				}
				
#else
				/********************
				NG case
				********************/
				if(IsKeyPressed(ConsoleKey.Q)){
					Console.WriteLine("Quit");
					b_Run = false;
				}else if(IsKeyPressed(ConsoleKey.Spacebar)){
					Console.WriteLine("spacebar");
				}
				
#endif
				/********************
				********************/
				DoAnyWork();
			}
		}
		
		/******************************
		OK case
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
		
		/******************************
		NG case
		******************************/
		static bool IsKeyPressed(ConsoleKey key){
			bool ret = false;
			
			if (Console.KeyAvailable == true){
				ConsoleKeyInfo cki = new ConsoleKeyInfo();
				cki = Console.ReadKey(true); // 引数 : 押されたキーをコンソール ウィンドウに表示するかどうかを決定します。 押されたキーを表示しない場合は true。それ以外の場合は false。
				
				if(cki.Key == key){
					ret = true;
				}
			}
			
			return ret;
		}

		/******************************
		******************************/
		static void DoAnyWork(){
			Thread.Sleep(200);
			
			// print to check if looping.
			string str_now = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss fff");
			Console.WriteLine($"{str_now}");
		}
	}
}

