// See https://aka.ms/new-console-template for more information
using FluentFTP;
using FTPBatchCleaner;

Console.WriteLine("Hello, World!");

var client = new FtpClient(Setting.Instance.HostName, Setting.Instance.UserName, Setting.Instance.Pwd);
client.AutoConnect();
List<Task> tasks = new List<Task>();
//foreach (var dir in Setting.Instance.Dirs)
//{
//    tasks.Add(Task.Run(() =>
//    {
//        var list = client.GetListing(dir).ToDictionary(i => i.Name, i => i);
//        if (list.Count > 30)
//        {
//            var slist = new System.Collections.SortedList(list);
//            var enumer = slist.GetEnumerator();
//            for (int i = 0; i < list.Count - 30; i++)
//            {
//                enumer.MoveNext();
//                var item = enumer.Value as FtpListItem;
//                if (item.Type == FtpObjectType.File)
//                {
//                    client.DeleteFile(item.FullName);
//                }
//                else if (item.Type == FtpObjectType.Directory)
//                {
//                    client.DeleteDirectory(item.FullName);
//                }
//                Console.WriteLine($"{enumer.Value} was removed");
//            }
//        }
//    }));
//}

//Task.WhenAll(tasks.ToArray()).ContinueWith(t =>
//{
//    if (t.IsFaulted)
//    {
//        foreach (var ex in t.Exception.InnerExceptions)
//        {
//            Console.Error.WriteLine(ex.ToString());
//        }
//    }
//});


foreach (var dir in Setting.Instance.Dirs)
{
    try
    {

        var list = client.GetListing(dir).ToDictionary(i => i.Name, i => i);
        if (list.Count > 30)
        {
            var slist = new System.Collections.SortedList(list);
            var enumer = slist.GetEnumerator();
            for (int i = 0; i < list.Count - 30; i++)
            {
                enumer.MoveNext();
                var item = enumer.Value as FtpListItem;
                if (item.Type == FtpObjectType.File)
                {
                    client.DeleteFile(item.FullName);
                }
                else if (item.Type == FtpObjectType.Directory)
                {
                    client.DeleteDirectory(item.FullName);
                }
                Console.WriteLine($"{enumer.Value} was removed");
            }
        }

    }
    catch (Exception ex)
    {

        Console.Error.WriteLine(ex.ToString());

    }
}