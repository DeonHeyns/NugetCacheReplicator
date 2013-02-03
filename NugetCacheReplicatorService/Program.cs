﻿﻿//    Copyright 2012 Deon Heyns
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using Topshelf;

namespace NugetCacheReplicatorService
{
    class Program
    {
        static void Main(string[] args)
        {
            var targetNugetRepositoryDirectory =
                Properties.Settings.Default.TargetNugetRepositoryDirectory;

            var nugetCacheDirectory = 
                Properties.Settings.Default.NugetCacheDirectory;

            HostFactory.Run(x =>
            {
                x.Service<NugetCacheReplicator>(s =>
                {
                    s.ConstructUsing(replicator => new NugetCacheReplicator(targetNugetRepositoryDirectory, nugetCacheDirectory));
                    s.WhenStarted(replicator => replicator.Start());
                    s.WhenStopped(replicator => replicator.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("NuGet Cache Replicator Host");
                x.SetDisplayName("NuGet Cache Replicator");
                x.SetServiceName("NuGetCacheReplicator");
            });
        }
    }
}
