using Dapper;
using Dapper.Contrib.Extensions;
using FluentFTP;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace NugetPackageUploader
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var rcH = Topshelf.HostFactory.New(x =>
            {
                x.Service<Uploader>(s =>
                {
                    s.ConstructUsing(n =>
                    {
                        var host = new Uploader();


                        return host;
                    });
                    s.WhenStarted((t, c) =>
                    {
                        var result = t.Start(c);

                        return true;
                    });

                    s.WhenStopped((t, c) =>
                    {
                        return t.Stop(c);
                    });
                });
                x.EnableServiceRecovery(src =>
                {
                    src.RestartService(TimeSpan.FromSeconds(2));
                    src.OnCrashOnly();
                });
                x.RunAsLocalSystem();
                //x.StartAutomaticallyDelayed();
                x.SetStartTimeout(TimeSpan.FromSeconds(60));
                x.SetStopTimeout(TimeSpan.FromSeconds(60));
                x.OnException(ex =>
                {
                    Console.Error.WriteLine(ex.Message);
                    Vinci.Logging.LoggerFactory.Logger.LogError(ex, "HostFactory");
                });

                x.SetDescription("Nuget Package Uploader");
                x.SetDisplayName("NugetPackageUploader");
                x.SetServiceName("NugetPackageUploader");
            });

            var rc = rcH.Run();

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;


        }
    }

    class Uploader : ServiceControl
    {
        public const string ProfileName = "profile.json";
        private System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource();
        private Task CurrentT;
        DateTime svcLastHandleDate = DateTime.MinValue;
        public bool Start(HostControl hostControl)
        {
            var token = tokenSource.Token;
            CurrentT = Task.Run(() =>
            {
                Vinci.Logging.LoggerFactory.Logger.LogInformation("Hello, NugetPackageUploader!");
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        if (DateTime.UtcNow.Subtract(svcLastHandleDate).Days < 1)
                        {
                            continue;
                        }
                        var assembly = System.Reflection.Assembly.GetAssembly(typeof(Setting));
                        var dir = System.IO.Path.GetDirectoryName(assembly.Location);
                        // using Dapper;
                        var dbfile = System.IO.Path.Combine(Setting.Instance.BagetDir, "baget.db");
                        string connectionString = $"Data Source={dbfile};";
                        var start = DateTime.UtcNow;
                        Vinci.Logging.LoggerFactory.Logger.LogInformation($"FTP:{Setting.Instance.HostName},{Setting.Instance.UserName}, {Setting.Instance.Pwd},{start.ToString()}");
                        var client = new FtpClient(Setting.Instance.HostName, Setting.Instance.UserName, Setting.Instance.Pwd);
                        client.AutoConnect();
                        Vinci.Logging.LoggerFactory.Logger.LogInformation("FTP connect ok");
                        var localProfileFile = System.IO.Path.Combine(dir, ProfileName);
                        var remoteFile = System.IO.Path.Combine(Setting.Instance.RemoteDir, ProfileName);
                        //client.DownloadFile(localFile, remoteFile);
                        //Vinci.Logging.LoggerFactory.Logger.LogInformation("Profile download ok");

                        var profile = Newtonsoft.Json.JsonConvert.DeserializeObject<Profile>(System.IO.File.ReadAllText(localProfileFile));
                        var lastDate = profile.Updated;
                        // Connect to the database
                        using (var connection = new SqliteConnection(connectionString))
                        {

                            // Create a query that retrieves all authors"    
                            var sql = $"SELECT Key,Id,Version FROM Packages Where Published >= '{lastDate}';";
                            // Use the Query method to execute the query and return the first author
                            var packages = connection.Query<Package>(sql);
                            foreach (var item in packages)
                            {
                                var local = System.IO.Path.Combine(Setting.Instance.LocalDir, item.Id, item.Version);
                                var remote = Path.Combine(Setting.Instance.RemoteDir, item.Id, item.Version);
                                client.UploadDirectory(local, remote);
                                Vinci.Logging.LoggerFactory.Logger.LogInformation($"{local} --> {remote}");
                            }
                        }
                        svcLastHandleDate = start;
                        profile.Updated = start.ToString("yyyy-MM-dd");

                        System.IO.File.WriteAllText(localProfileFile, Newtonsoft.Json.JsonConvert.SerializeObject(profile));

                        //client.UploadFile(localFile, remoteFile);
                        Vinci.Logging.LoggerFactory.Logger.LogInformation($"Profile upload ok:{svcLastHandleDate}");

                    }
                    catch (Exception ex)
                    {
                        Vinci.Logging.LoggerFactory.Logger.LogError(ex, "NugetPackageUploader");
                    }
                    finally
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromHours(1));
                    }
                }
                Vinci.Logging.LoggerFactory.Logger.LogInformation("Task stopped");

            }, token);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            tokenSource.Cancel();
            if (CurrentT != null && CurrentT.IsCompleted)
            {
                if (!System.Threading.SpinWait.SpinUntil(() => CurrentT.IsCompleted, 60000))
                {
                    Vinci.Logging.LoggerFactory.Logger.LogError("Task stop timeout");
                }
            }
            return true;
        }
    }

    class Package
    {
        [Key]
        public int Key { get; set; }
        public string Id { get; set; }
        //public DateTime Published { get; set; }
        public string Version { get; set; }

    }
}