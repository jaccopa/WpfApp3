using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfAppdump
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //try
            //{
            App app = new App();
            app.DispatcherUnhandledException -= App_DispatcherUnhandledException;
            app.DispatcherUnhandledException += App_DispatcherUnhandledException;
            app.Run(new MainWindow());
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}
        }



        [DllImport("dbghelp.dll")]
        public static extern bool MiniDumpWriteDump(IntPtr hProcess,
                                                    Int32 ProcessId,
                                                    IntPtr hFile,
                                                    int DumpType,
                                                    IntPtr ExceptionParam,
                                                    IntPtr UserStreamParam,
                                                    IntPtr CallackParam);


        public static class MINIDUMP_TYPE
        {
            public const int MiniDumpNormal = 0x00000000;
            public const int MiniDumpWithDataSegs = 0x00000001;
            public const int MiniDumpWithFullMemory = 0x00000002;
            public const int MiniDumpWithHandleData = 0x00000004;
            public const int MiniDumpFilterMemory = 0x00000008;
            public const int MiniDumpScanMemory = 0x00000010;
            public const int MiniDumpWithUnloadedModules = 0x00000020;
            public const int MiniDumpWithIndirectlyReferencedMemory = 0x00000040;
            public const int MiniDumpFilterModulePaths = 0x00000080;
            public const int MiniDumpWithProcessThreadData = 0x00000100;
            public const int MiniDumpWithPrivateReadWriteMemory = 0x00000200;
            public const int MiniDumpWithoutOptionalData = 0x00000400;
            public const int MiniDumpWithFullMemoryInfo = 0x00000800;
            public const int MiniDumpWithThreadInfo = 0x00001000;
            public const int MiniDumpWithCodeSegs = 0x00002000;
        }

        private static void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            CreateMiniDump();

            //Thread.Sleep(2000);
            //Process proc = new Process();
            //proc.StartInfo.FileName = @"WpfAppdump.exe";
            //proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(@"WpfAppdump.exe");
            //proc.Start();

        }

        //private static void CreateMiniDump()
        //{

        //    //MiniDump.TryDump("error.dmp");

        //    //using (FileStream fs = new FileStream("UnhandledDump.dmp", FileMode.Create))
        //    {
        //        //using (System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess())
        //        {
        //            //MiniDump.TryDump("error.dmp");
        //        }
        //    }
        //}

        private static void CreateMiniDump()
        {
            using (FileStream fs = new FileStream("UnhandledDump.dmp", FileMode.Create))
            {
                using (System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess())
                {
                    MiniDumpWriteDump(process.Handle,
                                                     process.Id,
                                                     fs.SafeFileHandle.DangerousGetHandle(),
                                                     MINIDUMP_TYPE.MiniDumpNormal,
                                                     IntPtr.Zero,
                                                     IntPtr.Zero,
                                                     IntPtr.Zero);
                }
            }
        }

        public static class MiniDump
        {
            // Taken almost verbatim from http://blog.kalmbach-software.de/2008/12/13/writing-minidumps-in-c/
            [Flags]
            public enum Option : uint
            {
                // From dbghelp.h:
                Normal = 0x00000000,
                WithDataSegs = 0x00000001,
                WithFullMemory = 0x00000002,
                WithHandleData = 0x00000004,
                FilterMemory = 0x00000008,
                ScanMemory = 0x00000010,
                WithUnloadedModules = 0x00000020,
                WithIndirectlyReferencedMemory = 0x00000040,
                FilterModulePaths = 0x00000080,
                WithProcessThreadData = 0x00000100,
                WithPrivateReadWriteMemory = 0x00000200,
                WithoutOptionalData = 0x00000400,
                WithFullMemoryInfo = 0x00000800,
                WithThreadInfo = 0x00001000,
                WithCodeSegs = 0x00002000,
                WithoutAuxiliaryState = 0x00004000,
                WithFullAuxiliaryState = 0x00008000,
                WithPrivateWriteCopyMemory = 0x00010000,
                IgnoreInaccessibleMemory = 0x00020000,
                ValidTypeFlags = 0x0003ffff,
            }

            enum ExceptionInfo
            {
                None,
                Present
            }

            //typedef struct _MINIDUMP_EXCEPTION_INFORMATION {
            //    DWORD ThreadId;
            //    PEXCEPTION_POINTERS ExceptionPointers;
            //    BOOL ClientPointers;
            //} MINIDUMP_EXCEPTION_INFORMATION, *PMINIDUMP_EXCEPTION_INFORMATION;
            [StructLayout(LayoutKind.Sequential, Pack = 4)]  // Pack=4 is important! So it works also for x64!
            struct MiniDumpExceptionInformation
            {
                public uint ThreadId;
                public IntPtr ExceptionPointers;
                [MarshalAs(UnmanagedType.Bool)]
                public bool ClientPointers;
            }

            //BOOL
            //WINAPI
            //MiniDumpWriteDump(
            //    __in HANDLE hProcess,
            //    __in DWORD ProcessId,
            //    __in HANDLE hFile,
            //    __in MINIDUMP_TYPE DumpType,
            //    __in_opt PMINIDUMP_EXCEPTION_INFORMATION ExceptionParam,
            //    __in_opt PMINIDUMP_USER_STREAM_INFORMATION UserStreamParam,
            //    __in_opt PMINIDUMP_CALLBACK_INFORMATION CallbackParam
            //    );
            // Overload requiring MiniDumpExceptionInformation
            [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]

            static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, ref MiniDumpExceptionInformation expParam, IntPtr userStreamParam, IntPtr callbackParam);

            // Overload supporting MiniDumpExceptionInformation == NULL
            [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
            static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);

            [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
            static extern uint GetCurrentThreadId();

            static bool Write(SafeHandle fileHandle, Option options, ExceptionInfo exceptionInfo)
            {
                Process currentProcess = Process.GetCurrentProcess();
                IntPtr currentProcessHandle = currentProcess.Handle;
                uint currentProcessId = (uint)currentProcess.Id;
                MiniDumpExceptionInformation exp;
                exp.ThreadId = GetCurrentThreadId();
                exp.ClientPointers = false;
                exp.ExceptionPointers = IntPtr.Zero;
                if (exceptionInfo == ExceptionInfo.Present)
                {
                    exp.ExceptionPointers = Marshal.GetExceptionPointers();
                }
                return exp.ExceptionPointers == IntPtr.Zero ? MiniDumpWriteDump(currentProcessHandle, currentProcessId, fileHandle, (uint)options, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) : MiniDumpWriteDump(currentProcessHandle, currentProcessId, fileHandle, (uint)options, ref exp, IntPtr.Zero, IntPtr.Zero);
            }

            static bool Write(SafeHandle fileHandle, Option dumpType)
            {
                return Write(fileHandle, dumpType, ExceptionInfo.None);
            }

            public static Boolean TryDump(String dmpPath, Option dmpType = Option.Normal)
            {
                var path = Path.Combine(Environment.CurrentDirectory, dmpPath);
                var dir = Path.GetDirectoryName(path);
                if (dir != null && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    return Write(fs.SafeFileHandle, dmpType);
                }
            }
        }
    }
}
