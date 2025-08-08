using System.Reflection;
using System.Text.Json;
using CommonFramework;

namespace DepsJsonDemo;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== .NET deps.json 学習デモ ===");
        Console.WriteLine();

        // 1. アプリケーション情報の表示
        DisplayApplicationInfo();
        
        // 2. deps.jsonファイルの場所と内容の確認
        AnalyzeDepsJson();
        
        // 3. 共通フレームワークの使用
        DemoCommonFramework();
        
        // 4. 依存関係の解析
        AnalyzeDependencies();

        Console.WriteLine("\n=== デモ完了 ===");
        Console.WriteLine("deps.jsonファイルは以下の場所にあります:");
        Console.WriteLine(GetDepsJsonPath());
    }

    static void DisplayApplicationInfo()
    {
        Console.WriteLine("--- アプリケーション情報 ---");
        var assembly = Assembly.GetExecutingAssembly();
        
        Console.WriteLine($"Assembly名: {assembly.GetName().Name}");
        Console.WriteLine($"バージョン: {assembly.GetName().Version}");
        Console.WriteLine($"場所: {assembly.Location}");
        Console.WriteLine($"実行時フレームワーク: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
        Console.WriteLine();
    }

    static void AnalyzeDepsJson()
    {
        Console.WriteLine("--- deps.json 解析 ---");
        
        var depsJsonPath = GetDepsJsonPath();
        
        if (File.Exists(depsJsonPath))
        {
            Console.WriteLine($"deps.jsonファイルが見つかりました: {depsJsonPath}");
            
            try
            {
                var content = File.ReadAllText(depsJsonPath);
                var jsonDoc = JsonDocument.Parse(content);
                
                Console.WriteLine("deps.json の主要セクション:");
                
                foreach (var property in jsonDoc.RootElement.EnumerateObject())
                {
                    Console.WriteLine($"  - {property.Name}");
                }
                
                // runtimeTarget の情報を表示
                if (jsonDoc.RootElement.TryGetProperty("runtimeTarget", out var runtimeTarget))
                {
                    Console.WriteLine($"\nランタイムターゲット:");
                    if (runtimeTarget.TryGetProperty("name", out var name))
                    {
                        Console.WriteLine($"  名前: {name.GetString()}");
                    }
                }
                
                // libraries の数を表示
                if (jsonDoc.RootElement.TryGetProperty("libraries", out var libraries))
                {
                    var libraryCount = libraries.EnumerateObject().Count();
                    Console.WriteLine($"\n依存ライブラリ数: {libraryCount}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"deps.jsonの解析中にエラーが発生しました: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("deps.jsonファイルが見つかりませんでした。");
            Console.WriteLine("アプリケーションをpublishすると生成されます。");
        }
        
        Console.WriteLine();
    }

    static void DemoCommonFramework()
    {
        Console.WriteLine("--- 共通フレームワークのデモ ---");
        
        try
        {
            var framework = new FrameworkCore();
            var info = framework.GetFrameworkInfo();
            
            Console.WriteLine("共通フレームワーク情報:");
            Console.WriteLine($"  名前: {info.Name}");
            Console.WriteLine($"  バージョン: {info.Version}");
            Console.WriteLine($"  場所: {info.Location}");
            
            var result = framework.ProcessData("サンプルデータ");
            Console.WriteLine($"\n処理結果: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"共通フレームワークの実行中にエラーが発生しました: {ex.Message}");
        }
        
        Console.WriteLine();
    }

    static void AnalyzeDependencies()
    {
        Console.WriteLine("--- 依存関係の解析 ---");
        
        var assembly = Assembly.GetExecutingAssembly();
        var referencedAssemblies = assembly.GetReferencedAssemblies();
        
        Console.WriteLine("参照されているアセンブリ:");
        foreach (var refAssembly in referencedAssemblies)
        {
            Console.WriteLine($"  - {refAssembly.Name} (v{refAssembly.Version})");
        }
        
        Console.WriteLine("\n現在ロードされているアセンブリ:");
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        foreach (var loadedAssembly in loadedAssemblies.OrderBy(a => a.GetName().Name))
        {
            var name = loadedAssembly.GetName();
            Console.WriteLine($"  - {name.Name} (v{name.Version}) - {loadedAssembly.Location}");
        }
        
        Console.WriteLine();
    }

    static string GetDepsJsonPath()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyPath = assembly.Location;
        var directory = Path.GetDirectoryName(assemblyPath);
        var assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
        
        return Path.Combine(directory ?? "", $"{assemblyName}.deps.json");
    }
}
