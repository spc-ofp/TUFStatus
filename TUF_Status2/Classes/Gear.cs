using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUFStatus
{
    public class Gear
    {
        private string _GearCode;

        public void New(string strGearCode)
        {
            _GearCode = strGearCode;
        }

        public void New(char cGearCode)
        {
            _GearCode = GearCode(cGearCode);
        }

        public string GearCode()
        {
            return _GearCode;
        }

        public string GearCode(char cGearCode)
        {
            string strCode;

            strCode = "";

            switch (cGearCode)
            {
                case 'B':
                    strCode = "BU";
                    break;
                case 'C':
                    strCode = "FC";
                    break;
                case 'G':
                    strCode = "GN";
                    break;
                case 'H':
                    strCode = "HL";
                    break;
                case 'L':
                    strCode = "LL";
                    break;
                case 'N':
                    strCode = "RN";
                    break;
                case 'P':
                    strCode = "PL";
                    break;
                case 'R':
                    strCode = "RS";
                    break;
                case 'S':
                    strCode = "PS";
                    break;
                case 'T':
                    strCode = "TR";
                    break;
                default:
                    strCode = "OT";
                    break;
            }
            return strCode;
        }



    }
}
