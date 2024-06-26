﻿using Cake.Core.Annotations;
using Cake.Core;
using Cake.Common.Build;
using Cake.Common.Build.GoCD;
using System;
using Cake.Common;
using Newtonsoft.Json;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("UnitProfile")]
namespace Cake.Profile
{
    public class ProfileItem
    {
        public string Subversion { get; set; }
        public string FromSubverion { get; set; }
        // group name
        public string ProductName { get; set; }
        public string PreVersion { get; set; }
        [JsonIgnore]
        public string Dir { get; set; }
        [JsonIgnore]
        public string BinOutputDir { get => System.IO.Path.Combine(Dir ?? "", "bin"); }
        public string SVN { get; set; }
        /// <summary>
        /// 上传的FTP地址
        /// </summary>
        public string FTPDir { get; set; }
        public string DownloadFilePath { get; set; }

        [JsonIgnore]
        public string Version
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Subversion))
                {
                    return PreVersion;
                }
                else
                {
                    return $"{PreVersion}.{Subversion}";
                }
            }
        }

        [JsonIgnore]
        public string SVNMsg { get; set; }

        [JsonIgnore]
        public string ProductNameWithVersion { get => $"{ProductName} {Version}"; }
    }
    [CakeAliasCategory("Profile")]
    public static class Profile
    {
        const string FILENAME = "profile.json";
        internal static ProfileItem _ProfileItem;
        static object locker = new();
        static string TargetDir = ".";

        [CakePropertyAlias]
        public static ProfileItem TheProfileItem(this ICakeContext context)
        {
            if (_ProfileItem is null)
            {
                TargetDir = context.Argument("dir", "");
                lock (locker)
                {
                    var path = System.IO.Path.Combine(TargetDir, FILENAME);
                    if (System.IO.File.Exists(path))
                    {
                        _ProfileItem = GetProfileFromLocalSystem(context, TargetDir);
                    }
                    else
                    {
                        _ProfileItem = GetProfileFromBuildSystem(context);
                        SaveProfile(context, TargetDir, _ProfileItem);
                    }
                }
                _ProfileItem.Dir = TargetDir;
            }
            return _ProfileItem;
        }
        [CakeMethodAlias]
        public static void RemoveProfile(this ICakeContext context, string targetDir)
        {
            _ProfileItem = null;
            var path = System.IO.Path.Combine(targetDir, FILENAME);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            //var dir = System.IO.Path.Combine(targetDir, "bin");
            //if (System.IO.Directory.Exists(dir))
            //{
            //    System.IO.Directory.Delete(dir, true);
            //}
        }


        [CakeMethodAlias]
        public static void SetTargetDir(this ICakeContext context, string targetDir)
        {
            TargetDir = targetDir;
        }

        [CakeMethodAlias]
        public static ProfileItem GetProfileFromBuildSystem(this ICakeContext context)
        {
            var profile = new ProfileItem();
            profile.ProductName = context.Environment.GetEnvironmentVariable("prod_name");
            profile.Subversion = context.Environment.GetEnvironmentVariable("GO_REVISION_SVN");
            profile.FromSubverion = context.Environment.GetEnvironmentVariable("GO_FROM_REVISION_SVN");
            profile.PreVersion = context.Environment.GetEnvironmentVariable("VERSION");
            profile.SVN = context.Environment.GetEnvironmentVariable("GO_MATERIAL_URL_SVN");
            profile.FTPDir = context.Environment.GetEnvironmentVariable("ftpdir");
            profile.DownloadFilePath = context.Environment.GetEnvironmentVariable("dfp");
            //profile.Dir = context.Environment.WorkingDirectory.FullPath;
            return profile;
        }

        [CakeMethodAlias]
        public static void SaveProfile(this ICakeContext context, string dir, ProfileItem profileItem)
        {
            System.IO.File.WriteAllText(System.IO.Path.Combine(dir, FILENAME), Newtonsoft.Json.JsonConvert.SerializeObject(profileItem));
        }

        [CakeMethodAlias]
        public static ProfileItem GetProfileFromLocalSystem(this ICakeContext context, string dir)
        {
            var p = Newtonsoft.Json.JsonConvert.DeserializeObject<ProfileItem>(System.IO.File.ReadAllText(System.IO.Path.Combine(dir, "profile.json")));
            _ProfileItem = p;
            return p;
        }

    }
}
