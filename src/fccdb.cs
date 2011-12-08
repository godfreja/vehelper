
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

namespace HamRadio.FccDB {

public class LocalFccDB {
    public string dbFilename;
    public string Filename { get {return dbFilename;} }
    protected TextWriter tw;

    public LocalFccDB( string filename ) 
    {
        dbFilename = filename;
    }

    ~LocalFccDB()
    {
        if( tw != null ) tw.Close();
    }

    public void CloseFiles ( )
    {
        if( tw != null ) tw.Close();
        tw = null;
    }

    public void WriteRecord( Entity save )
    {
        string[] tokens = new string[15];
        
        if( tw == null ) tw = new StreamWriter( dbFilename );

        tokens[0] = save.call;
        tokens[1] = save.frnOrSn;
        tokens[2] = save.firstName;
        tokens[3] = save.lastName;
        tokens[4] = save.middleInitial;
        tokens[5] = save.suffix;
        tokens[6] = save.daytimePhone;
        tokens[7] = save.faxPhone;
        tokens[8] = save.email;
        tokens[9] = save.street;
        tokens[10] = save.city;
        tokens[11] = save.state;
        tokens[12] = save.zip;
        tokens[13] = save.poBox;
        tokens[14] = Char.ToString(Entity.LicenseClassToChar( save.licenseClass ));

        tw.WriteLine( String.Join( "|", tokens ));
    }
} 


}
