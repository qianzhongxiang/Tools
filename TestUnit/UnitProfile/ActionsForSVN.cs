using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Profile;
using System;
using Xunit;

namespace Unit_SVN
{

    public class context : ICakeContext
    {
        public IFileSystem FileSystem => throw new NotImplementedException();

        public ICakeEnvironment Environment => throw new NotImplementedException();

        public IGlobber Globber => throw new NotImplementedException();

        public ICakeLog Log => throw new NotImplementedException();

        public ICakeArguments Arguments => throw new NotImplementedException();

        public IProcessRunner ProcessRunner => throw new NotImplementedException();

        public IRegistry Registry => throw new NotImplementedException();

        public IToolLocator Tools => throw new NotImplementedException();

        public ICakeDataResolver Data => throw new NotImplementedException();

        public ICakeConfiguration Configuration => throw new NotImplementedException();
    }

    public class ActionsForSVN
    {
      

        [Fact]
        public void GetChanges()
        {
            Profile._ProfileItem = new ProfileItem
            {
                SVN = "https://rd01/svn/VP_SVN/SEMI_Business/Tornado2000S/source/trunk/Tornado_2.0",
                Subversion = "2532",
                FromSubverion = "2499",
            };
            var changes = SVN_Resolver.SVNLogs(new context());
            Assert.NotEmpty(changes);
        }
    }
}
