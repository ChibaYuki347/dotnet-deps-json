using System.CommandLine;
using System.Text.Json;

namespace DependencyAnalyzer;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var fileOption = new Option<FileInfo?>(
            name: "--file",
            description: "解析するdeps.jsonファイルのパス")
        {
            IsRequired = true
        };

        var verboseOption = new Option<bool>(
            name: "--verbose",
            description: "詳細な情報を表示");

        var rootCommand = new RootCommand("deps.jsonファイルを解析して依存関係情報を表示します");
        rootCommand.AddOption(fileOption);
        rootCommand.AddOption(verboseOption);

        rootCommand.SetHandler(async (file, verbose) =>
        {
            await AnalyzeDepsFile(file!, verbose);
        }, fileOption, verboseOption);

        return await rootCommand.InvokeAsync(args);
    }

    static async Task AnalyzeDepsFile(FileInfo file, bool verbose)
    {
        if (!file.Exists)
        {
            Console.WriteLine($"ファイルが見つかりません: {file.FullName}");
            return;
        }

        Console.WriteLine($"=== deps.json解析結果: {file.Name} ===");
        Console.WriteLine();

        try
        {
            var content = await File.ReadAllTextAsync(file.FullName);
            var jsonDoc = JsonDocument.Parse(content);
            var root = jsonDoc.RootElement;

            // 基本情報
            DisplayBasicInfo(root);

            // ランタイム情報
            DisplayRuntimeInfo(root);

            // ライブラリ情報
            DisplayLibrariesInfo(root, verbose);

            // ランタイム依存関係
            DisplayRuntimeDependencies(root, verbose);

            // 統計情報
            DisplayStatistics(root);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSONの解析エラー: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"予期しないエラー: {ex.Message}");
        }
    }

    static void DisplayBasicInfo(JsonElement root)
    {
        Console.WriteLine("--- 基本情報 ---");

        if (root.TryGetProperty("runtimeTarget", out var runtimeTarget))
        {
            if (runtimeTarget.TryGetProperty("name", out var name))
            {
                Console.WriteLine($"ランタイムターゲット: {name.GetString()}");
            }
            if (runtimeTarget.TryGetProperty("signature", out var signature))
            {
                Console.WriteLine($"署名: {signature.GetString()}");
            }
        }

        if (root.TryGetProperty("compilationOptions", out var compilationOptions))
        {
            Console.WriteLine("コンパイルオプション:");
            foreach (var option in compilationOptions.EnumerateObject())
            {
                Console.WriteLine($"  {option.Name}: {option.Value}");
            }
        }

        Console.WriteLine();
    }

    static void DisplayRuntimeInfo(JsonElement root)
    {
        Console.WriteLine("--- ランタイム情報 ---");

        if (root.TryGetProperty("targets", out var targets))
        {
            foreach (var target in targets.EnumerateObject())
            {
                Console.WriteLine($"ターゲット: {target.Name}");
                var libraryCount = target.Value.EnumerateObject().Count();
                Console.WriteLine($"  ライブラリ数: {libraryCount}");
            }
        }

        Console.WriteLine();
    }

    static void DisplayLibrariesInfo(JsonElement root, bool verbose)
    {
        Console.WriteLine("--- ライブラリ情報 ---");

        if (root.TryGetProperty("libraries", out var libraries))
        {
            var libraryList = libraries.EnumerateObject().ToList();
            Console.WriteLine($"総ライブラリ数: {libraryList.Count}");

            var packageLibraries = libraryList.Where(lib => 
                lib.Value.TryGetProperty("type", out var type) && 
                type.GetString() == "package").ToList();

            var projectLibraries = libraryList.Where(lib => 
                lib.Value.TryGetProperty("type", out var type) && 
                type.GetString() == "project").ToList();

            Console.WriteLine($"NuGetパッケージ: {packageLibraries.Count}");
            Console.WriteLine($"プロジェクト参照: {projectLibraries.Count}");

            if (verbose)
            {
                Console.WriteLine("\nNuGetパッケージ一覧:");
                foreach (var package in packageLibraries)
                {
                    Console.WriteLine($"  - {package.Name}");
                    if (package.Value.TryGetProperty("sha512", out var sha))
                    {
                        Console.WriteLine($"    SHA512: {sha.GetString()?[..20]}...");
                    }
                    if (package.Value.TryGetProperty("path", out var path))
                    {
                        Console.WriteLine($"    パス: {path.GetString()}");
                    }
                }

                Console.WriteLine("\nプロジェクト参照一覧:");
                foreach (var project in projectLibraries)
                {
                    Console.WriteLine($"  - {project.Name}");
                    if (project.Value.TryGetProperty("path", out var path))
                    {
                        Console.WriteLine($"    パス: {path.GetString()}");
                    }
                }
            }
        }

        Console.WriteLine();
    }

    static void DisplayRuntimeDependencies(JsonElement root, bool verbose)
    {
        Console.WriteLine("--- ランタイム依存関係 ---");

        if (root.TryGetProperty("targets", out var targets))
        {
            foreach (var target in targets.EnumerateObject())
            {
                Console.WriteLine($"ターゲット: {target.Name}");

                foreach (var library in target.Value.EnumerateObject())
                {
                    if (library.Value.TryGetProperty("dependencies", out var dependencies))
                    {
                        if (dependencies.ValueKind == JsonValueKind.Object && dependencies.EnumerateObject().Any())
                        {
                            Console.WriteLine($"  {library.Name}:");
                            foreach (var dep in dependencies.EnumerateObject())
                            {
                                Console.WriteLine($"    → {dep.Name}: {dep.Value.GetString()}");
                            }
                        }
                    }

                    if (verbose && library.Value.TryGetProperty("runtime", out var runtime))
                    {
                        Console.WriteLine($"  {library.Name} ランタイムファイル:");
                        foreach (var file in runtime.EnumerateObject())
                        {
                            Console.WriteLine($"    - {file.Name}");
                        }
                    }
                }
            }
        }

        Console.WriteLine();
    }

    static void DisplayStatistics(JsonElement root)
    {
        Console.WriteLine("--- 統計情報 ---");

        var runtimeFiles = 0;

        if (root.TryGetProperty("targets", out var targets))
        {
            foreach (var target in targets.EnumerateObject())
            {
                foreach (var library in target.Value.EnumerateObject())
                {
                    if (library.Value.TryGetProperty("runtime", out var runtime))
                    {
                        runtimeFiles += runtime.EnumerateObject().Count();
                    }
                }
            }
        }

        Console.WriteLine($"ランタイムファイル数: {runtimeFiles}");

        Console.WriteLine();
    }
}
