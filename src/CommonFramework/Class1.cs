using System.Reflection;

namespace CommonFramework;

/// <summary>
/// 共通フレームワークのメインクラス
/// deps.jsonでの依存関係管理を理解するためのサンプル
/// </summary>
public class FrameworkCore
{
    /// <summary>
    /// フレームワークの情報を取得
    /// </summary>
    public FrameworkInfo GetFrameworkInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();
        return new FrameworkInfo
        {
            Name = "CommonFramework",
            Version = assembly.GetName().Version?.ToString() ?? "1.0.0",
            Location = assembly.Location,
            FullName = assembly.FullName ?? "Unknown"
        };
    }

    /// <summary>
    /// 依存関係のある処理をシミュレート
    /// </summary>
    public string ProcessData(string input)
    {
        // System.Text.Jsonを使用してJSONシリアライゼーション
        return System.Text.Json.JsonSerializer.Serialize(new { 
            Input = input, 
            ProcessedAt = DateTime.UtcNow,
            Framework = GetFrameworkInfo()
        });
    }
}

/// <summary>
/// フレームワーク情報を格納するクラス
/// </summary>
public class FrameworkInfo
{
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
