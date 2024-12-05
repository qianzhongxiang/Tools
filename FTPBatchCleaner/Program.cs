// See https://aka.ms/new-console-template for more information
using FluentFTP;
using FTPBatchCleaner;
using Topshelf;

var rcH = Topshelf.HostFactory.New(x =>
{
    x.Service<Cleaner>(s =>
    {
        s.ConstructUsing(n =>
        {
            var host = new Cleaner();


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

    x.SetDescription("FTPBatchCleaner");
    x.SetDisplayName("FTPBatchCleaner");
    x.SetServiceName("FTPBatchCleaner");
});

var rc = rcH.Run();

var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
Environment.ExitCode = exitCode;


class Cleaner : ServiceControl
{
    private System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource();
    private Task CurrentT;
    DateTime svcLastHandleDate = DateTime.MinValue;
    public bool Start(HostControl hostControl)
    {
        var token = tokenSource.Token;
        CurrentT = Task.Run(() =>
        {
            Vinci.Logging.LoggerFactory.Logger.LogInformation("Hello, FTPBatchCleaner!");
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (DateTime.Now.Subtract(svcLastHandleDate).Days < 1)
                    {
                        continue;
                    }

                    var client = new FtpClient(Setting.Instance.HostName, Setting.Instance.UserName, Setting.Instance.Pwd);
                    client.AutoConnect();
                    List<FtpListItem> list;
                    foreach (var dir in Setting.Instance.Dirs)
                    {
                        list = client.GetListing(dir).ToList();
                        if (list.Count() > 30)
                        {
                            list.Sort((f1, f2) =>
                            {
                                if (f1.Modified < f2.Modified)
                                {
                                    return -1;
                                }
                                else
                                {
                                    return 1;
                                }
                            });
                            for (int i = 0; i < list.Count - 30; i++)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    throw new TaskCanceledException();
                                }
                                var item = list[i];
                                if (item.Type == FtpObjectType.File)
                                {
                                    client.DeleteFile(item.FullName);
                                }
                                else if (item.Type == FtpObjectType.Directory)
                                {
                                    client.DeleteDirectory(item.FullName);
                                }
                                Console.WriteLine($"{item.Name} was removed");
                            }
                        }
                    }
                    foreach (var fdir in Setting.Instance.DirsForOnlyFiles)
                    {
                        list = client.GetListing(fdir).Where(f => f.Type == FtpObjectType.File).ToList();
                        if (list.Count > 30)
                        {
                            list.Sort((f1, f2) =>
                            {
                                if (f1.Modified < f2.Modified)
                                {
                                    return -1;
                                }
                                else
                                {
                                    return 1;
                                }
                            });
                            for (int i = 0; i < list.Count - 30; i++)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    throw new TaskCanceledException();
                                }
                                var item = list[i];
                                client.DeleteFile(item.FullName);

                                Console.WriteLine($"{item.Name} was removed");
                            }
                        }
                    }
                    svcLastHandleDate = DateTime.Now;
                }
                catch (Exception ex)
                {
                    Vinci.Logging.LoggerFactory.Logger.LogError(ex, "FTPBatchCleaner");
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

