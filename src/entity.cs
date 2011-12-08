
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

namespace HamRadio.FccDB {

public enum LicenseClass { Novice, Technician, TechnicianPlus, General, Advanced, 
        Extra, None };

public class Entity {
    public string call { get; set; }
    public string frnOrSn { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string middleInitial { get; set; }
    public string suffix { get; set; }
    public string daytimePhone { get; set; }
    public string faxPhone { get; set; }
    public string email { get; set; }
    public string street { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string zip { get; set; }
    public string poBox { get; set; }
    public LicenseClass licenseClass { get; set; }

    public static char LicenseClassToChar( LicenseClass c )
    {
        char result;

        switch (c)
        {
            case LicenseClass.Novice:
                result = 'N';
                break;
            case LicenseClass.Technician:
                result = 'T';
                break;
            case LicenseClass.TechnicianPlus:
                result = 'P';
                break;
            case LicenseClass.General:
                result = 'G';
                break;
            case LicenseClass.Advanced:
                result = 'A';
                break;
            case LicenseClass.Extra:
                result = 'E';
                break;
            default:
                result = ' ';
                break;
        }
        return result;
    }

    public static LicenseClass CharToLicenseClass( char c )
    {
        LicenseClass licenseClass;

        switch (c)
        {
            case 'N':
                licenseClass = LicenseClass.Novice;
                break;
            case 'T':
                licenseClass = LicenseClass.Technician;
                break;
            case 'P':
                licenseClass = LicenseClass.TechnicianPlus;
                break;
            case 'G':
                licenseClass = LicenseClass.General;
                break;
            case 'A':
                licenseClass = LicenseClass.Advanced;
                break;
            case 'E':
                licenseClass = LicenseClass.Extra;
                break;
            default:
                licenseClass = LicenseClass.None;
                break;
        }
        return licenseClass;
    }
} 


}
