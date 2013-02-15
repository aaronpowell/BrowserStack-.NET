using System;
namespace BrowserStack
{
  public sealed class ApiStatus
  {
    public string Message { get; set; }
    public TimeSpan UsedTime{ get; set; }
    public TimeSpan AvailableTime { get; set; }
    public int RunningWindowsSessions{ get; set; }
    public int WindowsSessionsLimit { get; set; }
    public int RunningMacSessions { get; set; }
    public int MacSessionsLimit { get; set; }
  }
}