//////////////////////////////////////////////////////////////////////////////
// TestHarness.cs - Demonstrate TestHarness operations                      //
// ver 1.0                                                                  // 
// Author: Shiqi Zhang, szhang65@syr.edu                                    //
// Application: CSE681 Project 4-CsMessagePassingComm                       //
// Environment: C# console                                                  //
//////////////////////////////////////////////////////////////////////////////
/* 
* Package Operations: 
* =================== 
* 
* This package execute dlls and demonstrates the TestResults.
* 
* 
* Public Interface 
* ---------------- 
* - RequestInfo                      -
* - parseRequestInfoFormCommMessage  -
* 
* Required Files: 
* --------------- 
* IMessagePassingCommService.cs 
* 
* Maintenance History: 
* -------------------- 
* ver 1.0 : 06 Dec 2017 
* - first release 
*  
*/
using MessagePassingComm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestHarness
{

    // manage a request 
    public class RequestInfo
    {

        public string author { get; set; }
        //public string projectName { get; set; }

        public string requestStr;

        public CommMessage msg;
        //public Dictionary<string , string> requestAuguments;

        public RequestInfo ( CommMessage msg)
        {
            this.msg = msg;

            parseRequestInfoFormCommMessage ( msg );
        }


        public bool parseRequestInfoFormCommMessage( CommMessage msg  )
        {
            author = msg.author;
            //projectName = msg.

            return true;
        }
            

    }

    class TestHarness
    {

        
    }

    class TestMain
    {
        static void Main ( string[ ] args )
        {

        }
    }


    
}
