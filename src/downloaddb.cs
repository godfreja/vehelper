
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
using System.Net;
using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace HamRadio.FccDB {

public class DBDownloader {
    public string dbAddress { get; set;  }
    public string filename { get; set; } 
    protected string dir;
    protected string targetDB;
    public WebClient client { get; set; }
    
    public DBDownloader ( string target ) {
        dbAddress = "http://wireless.fcc.gov/uls/data/complete/l_amat.zip";
        dir = Path.GetTempPath() + Path.GetRandomFileName();
        Directory.CreateDirectory( dir ); // XXX Handle exception
        filename = dir +  Path.DirectorySeparatorChar + "l_amat.zip";
        client = new WebClient();
        targetDB = target;
    }

    public void StartDownload( ) {
        client.DownloadFileAsync( new Uri(dbAddress), filename);
    }

    public void ProcessDownload( ) {
        string line;
        StreamReader file; 
        LocalFccDB saveDB = new LocalFccDB( dir + 
                   Path.DirectorySeparatorChar + Path.GetRandomFileName() ); 
        Dictionary<string,Entity> table = new Dictionary<string,Entity>();

        // Extract the downloaded file
        ExtractZipFile( filename, null, dir );


        file = new StreamReader( dir + Path.DirectorySeparatorChar 
                    + "EN.dat" );
        while((line = file.ReadLine()) != null)
        {
            string [] tokens = line.Split('|');
            Entity entry = new Entity();
            entry.call = tokens[4];
            entry.firstName = tokens[8];
            entry.lastName = tokens[10];
            entry.middleInitial = tokens[9];
            entry.suffix = tokens[11];
            entry.daytimePhone = tokens[12];
            entry.faxPhone = tokens[13];
            entry.email = tokens[14];
            entry.street = tokens[15];
            entry.city = tokens[16];
            entry.state = tokens[17];
            entry.zip = tokens[18];
            entry.poBox = tokens[19];
            entry.frnOrSn = tokens[22];
            table.Add( tokens[1], entry );
        }
        file.Close();

        // Read the AM.dat file and cache the information
        file = new StreamReader( dir + Path.DirectorySeparatorChar 
                    + "AM.dat" );
        while((line = file.ReadLine()) != null)
        {
            string [] tokens = line.Split('|');
            Entity entry =  table[tokens[1]];
            entry.call = tokens[4];
            if(tokens[5].Length == 1 )
                entry.licenseClass = Entity.CharToLicenseClass( tokens[5][0] );
            else
                entry.licenseClass = LicenseClass.None;
        }
        file.Close();

        foreach (var pair in table)
            saveDB.WriteRecord( pair.Value );

        saveDB.CloseFiles();
        try
        {
            File.Delete( targetDB ); // Delete it if it is there
        }
        catch ( IOException ) {}

        File.Move( saveDB.Filename, targetDB );

        File.Delete( dir + Path.DirectorySeparatorChar + "AM.dat" );
        File.Delete( dir + Path.DirectorySeparatorChar + "CO.dat" );
        File.Delete( dir + Path.DirectorySeparatorChar + "EN.dat" );
        File.Delete( dir + Path.DirectorySeparatorChar + "HD.dat" );
        File.Delete( dir + Path.DirectorySeparatorChar + "HS.dat" );
        File.Delete( dir + Path.DirectorySeparatorChar + "LA.dat" );
        File.Delete( dir + Path.DirectorySeparatorChar + "SC.dat" );
        File.Delete( dir + Path.DirectorySeparatorChar + "SF.dat" );
        File.Delete( dir + Path.DirectorySeparatorChar + "counts" );
        File.Delete( dir + Path.DirectorySeparatorChar + "l_amat.zip" );
        Directory.Delete( dir );
    }

    // Use example code from SharpZipLib
    public void ExtractZipFile(string archiveFilenameIn, string password, string outFolder) {
        ZipFile zf = null;
        try {
            FileStream fs = File.OpenRead(archiveFilenameIn);
            zf = new ZipFile(fs);
            if (!String.IsNullOrEmpty(password)) {
                zf.Password = password;     // AES encrypted entries are handled automatically
            }
            foreach (ZipEntry zipEntry in zf) {
                if (!zipEntry.IsFile) {
                    continue;           // Ignore directories
                }
                String entryFileName = zipEntry.Name;
                // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.

                byte[] buffer = new byte[4096];     // 4K is optimum
                Stream zipStream = zf.GetInputStream(zipEntry);

                // Manipulate the output filename here as desired.
                String fullZipToPath = Path.Combine(outFolder, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                using (FileStream streamWriter = File.Create(fullZipToPath)) {
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
                }
            }
        } finally {
            if (zf != null) {
                zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                zf.Close(); // Ensure we release resources
            }
        }
    }

} 
}
