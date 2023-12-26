using Cake.Core;
using Cake.Core.Annotations;
using SharpSvn;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cake.Profile
{

    public class LogItem
    {
        public string Msg { get; set; }
        public DateTime Time { get; set; }
        public List<ChangeItem> ChangeItems { get; set; }
        public string Author { get; set; }
        public long Resversion { get; set; }
    }

    public class ChangeItem
    {
        public string Path { get; set; }
        public string Action { get; set; }
        public string NodeKind { get; set; }
      
    }

    public static class SVN_Resolver
    {
        [CakeMethodAlias]
        public static LogItem[] SVNLogs(this ICakeContext context)
        {
            var opt = context.TheProfileItem();
            List<LogItem> changes = new List<LogItem>();
            using (var client = new SharpSvn.SvnClient())
            {
                client.Authentication.ForceCredentials("qianzhongxiang", "qianzhongxiang");
                client.Authentication.SslServerTrustHandlers += new EventHandler<SharpSvn.Security.SvnSslServerTrustEventArgs>(Authentication_SslServerTrustHandlers);
                Collection<SvnLogEventArgs> res = null;

                if (client.GetLog(new Uri(opt.SVN), new SvnLogArgs { Start = int.Parse(opt.FromSubverion), End = int.Parse(opt.Subversion) }, out res))
                {
                    foreach (var log in res)
                    {
                        LogItem logItem= new LogItem {  Msg=log.LogMessage, Time=log.Time, Author=log.Author, Resversion= log.Revision, ChangeItems=new List<ChangeItem>()};
                        foreach (var item in log.ChangedPaths)
                        {
                            logItem.ChangeItems.Add(new ChangeItem {  Action= item.Action.ToString(), Path= item.Path, NodeKind= item.NodeKind.ToString()});
                        }
                        changes.Add(logItem);
                    }
                  
                }
            }
            return changes.ToArray();
        }
        [CakeMethodAlias]
        public static void Update(this ICakeContext context)
        {
            var opt = context.TheProfileItem();
            using (var client = new SharpSvn.SvnClient())
            {
                client.Authentication.ForceCredentials("qianzhongxiang", "qianzhongxiang");
                client.Authentication.SslServerTrustHandlers += new EventHandler<SharpSvn.Security.SvnSslServerTrustEventArgs>(Authentication_SslServerTrustHandlers);

                if (client.GetUriFromWorkingCopy(opt.Dir) is null)
                {
                    var args = new SharpSvn.SvnCheckOutArgs();
                    args.Notify += Notification;
                    client.CheckOut(new Uri(opt.SVN), opt.Dir, args);
                }
                else
                {
                    var args = new SharpSvn.SvnUpdateArgs();
                    args.Notify += Notification;
                    client.Update(opt.Dir, args);
                }
            }
        }
        static void Authentication_SslServerTrustHandlers(object sender, SharpSvn.Security.SvnSslServerTrustEventArgs e)
        {
            // Look at the rest of the arguments of E, whether you wish to accept

            // If accept:
            e.AcceptedFailures = e.Failures;
            e.Save = true; // Save acceptance to authentication store
        }
        static void Notification(object sender, SvnNotifyEventArgs e)
        {
            System.Console.Write(e.Action);
            System.Console.WriteLine(e.FullPath);
        }
    }

}
