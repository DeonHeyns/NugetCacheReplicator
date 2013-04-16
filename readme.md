NuGet Cache Replicator
======================
#Replicate your NuGet cache to a local folder

[![Build Status](http://hookify.cloudapp.net:8080/job/NuGet%20Cache%20Replicator/badge/icon)](/)

##What
This is a Windows Service (NT Service) that will monitor a directory (NuGet Cache Directory).
When a new NuGet package is installed that doesn't exist in this cache NuGet will copy it here,
the replicator will pick it up and copy it over to your own local NuGet cache.

##Why
I live in South Africa bandwidth is scarce and ain't nobody got time to wait on NuGet to download a 
package over a slow connection.

##How
Well it runs as a Windows Service using  [Topshelf](https://github.com/phatboyg/Topshelf), it creates
a FileSystemWatcher that will watch the NuGet cached Directory. To get the application to run as a service 
you will need to do the following:

1.) Download or clone the source

2.) Change the NugetCacheDirectory(this is the directory where NuGet stores the cached packages for your system) setting in the NugetCacheReplicatorService.exe.config and also change the TargetNugetRepositoryDirectory(this is the directory where you want your local cache to be) which is also in the NugetCacheReplicatorService.exe.config

3.) XCopy the built Release files to C:\Program Files (x86)\Nuget Cache Replicator

4.) Open up CMD as an Administrator

5.) Enter the command NugetCacheReplicatorService.exe install

6.) Enter the command NugetCacheReplicatorService.exe start

7.) Add this new folder as a Package Source in Visual Studio under Tools -> Options -> Package Manager -> Package Sources.

8.) Chill as your new local NuGet cache is populated with NuGet Packages as you install them via the Package Manager

How to get hold of me
twitter: [@DeonHeyns](https://twitter.com/deonheyns)

website: [deonheyns.com](http://deonheyns.com/contact)

GitHub: [GitHub Profile](https://github.com/deonheyns)

#Open source projects used:

Topshelf: https://github.com/phatboyg/Topshelf