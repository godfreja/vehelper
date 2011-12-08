
/* 
VEHelper - print forms for ARRL VE Sessions
Copyright (C) 2011  Jason A. Godfrey

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

using System;
using System.IO;
using System.Net;
using System.ComponentModel;

namespace HamRadio.FccDB {

class TestDb {

    static DBDownloader db;
    static int lastPercentage = 0;
    static void Main(string[] args)
    {
        db = new DBDownloader( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
            Path.DirectorySeparatorChar + "fccdb.dat" );
        db.client.DownloadProgressChanged += new DownloadProgressChangedEventHandler( DownloadProgressCallback );
        db.client.DownloadFileCompleted += new AsyncCompletedEventHandler( DownloadFileCompletedCallback );
        System.Console.WriteLine( "Filename is " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "fccdb.dat" );
        db.StartDownload();
    }

    private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
    {
        if (e.ProgressPercentage == lastPercentage)
        {
            return;
        }

        lastPercentage = e.ProgressPercentage;
        // Displays the operation identifier, and the transfer progress.
        System.Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...", 
            (string)e.UserState, 
            e.BytesReceived, 
            e.TotalBytesToReceive,
            e.ProgressPercentage);
    }

    private static void DownloadFileCompletedCallback(object sender, AsyncCompletedEventArgs e) 
    {
        db.ProcessDownload();
    }

}
}
