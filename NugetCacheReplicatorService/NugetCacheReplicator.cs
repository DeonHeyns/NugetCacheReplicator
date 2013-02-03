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

using System;
using System.IO;
using System.Threading.Tasks;

namespace NugetCacheReplicatorService
{
    internal sealed class NugetCacheReplicator
    {
        private readonly string _targetNugetRepositoryDirectory;
        private readonly string _nugetCacheDirectory;
        private FileSystemWatcher _fileWatcher;

        internal NugetCacheReplicator(string targetNugetRepositoryDirectory, string nugetCacheDirectory)
        {
            if (string.IsNullOrWhiteSpace(targetNugetRepositoryDirectory))
                throw new ArgumentNullException("targetNugetRepositoryDirectory");
            if (string.IsNullOrWhiteSpace(nugetCacheDirectory))
                throw new ArgumentNullException("nugetCacheDirectory");

            _targetNugetRepositoryDirectory = targetNugetRepositoryDirectory;
            _nugetCacheDirectory = nugetCacheDirectory;
        }

        internal void Start()
        {
            _fileWatcher = new FileSystemWatcher(_nugetCacheDirectory)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName,
                Filter = "*.nupkg",
                EnableRaisingEvents = true,
            };

            _fileWatcher.Created += (s, e) =>
                {
                        var action = new Action(() =>
                        {
                            var pathWithFileName = e.FullPath.Replace(_nugetCacheDirectory, _targetNugetRepositoryDirectory);
                            var pathWithoutFileName = Path.GetDirectoryName(pathWithFileName);

                            Directory.CreateDirectory(pathWithoutFileName);
                            File.Copy(e.FullPath, pathWithFileName, true);
                        });
                    
                    Task.Factory.StartNew(() =>
                        {
                            var done = false;
                            var delay = 60000; // milliseconds - 1 minute
                            while (!done)
                            {
                                try
                                {
                                    action.Invoke();
                                    done = true;
                                }
                                catch (IOException)
                                {
                                    Task.Delay(delay);
                                    delay += 60000;
                                }
                                catch (Exception)
                                {
                                    done = true;
                                }
                            }
                        });
                };

        }

        internal void Stop()
        {
            if(_fileWatcher != null)
                _fileWatcher.Dispose();
        }
    }
}
