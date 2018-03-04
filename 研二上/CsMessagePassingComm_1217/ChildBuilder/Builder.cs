//////////////////////////////////////////////////////////////////////////////
// Builder.cs - Demonstrate Build operations                                //
// ver 1.0                                                                  // 
// Author: Shiqi Zhang, szhang65@syr.edu, Ammar Salman                      //
// Application: CSE681 Project 4-CsMessagePassingComm                       //
// Environment: C# console                                                  //
//////////////////////////////////////////////////////////////////////////////
/* 
* Package Operations: 
* =================== 
* Building dlls using the provided source code.
* 
* Public Interface 
* ---------------- 
* - DllBuilder    - initialize the builder 
* - Build         - build the code and add it to logger 
*  
* Maintenance History: 
* -------------------- 
* ver 1.0 : 06 Dec 2017 
* - first release 
*  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Building
{
    public class DllBuilder
    {
        string[] filenames;
        string dllname;

        StringBuilder logger;
        public string Log
        {
            get
            {
                return logger.ToString();
            }
        }


        public DllBuilder(List<string> filenames, string dllname)
        {
            logger = new StringBuilder();
            this.filenames = filenames.ToArray();
            this.dllname = dllname;
        }

        public bool Build()
        {
            try
            {
                CSharpCodeProvider codeProvider = new CSharpCodeProvider ( );
                ICodeCompiler icc = codeProvider.CreateCompiler ( );

                System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters ( );
                parameters.GenerateExecutable = false;
                parameters.OutputAssembly = dllname;
                CompilerResults results = icc.CompileAssemblyFromSourceBatch ( parameters , filenames );

                foreach ( var result in results.Output )
                {
                    logger.Append ( result.ToString ( ) + "\n" );
                }

                return true;
            }
            catch(Exception ex)
            {
                Console.Write ( "\n  {0}" , ex.Message );
                return false;

            }


        }
    }
}
