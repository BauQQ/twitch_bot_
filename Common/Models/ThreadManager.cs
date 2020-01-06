using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chatbot.Common.Objects;
using Chatbot.Common;
using System.Threading;
using System.Windows.Forms;

namespace Chatbot.Common.Models
{
    public static class ThreadManager
    {
        public static Dictionary<string, AutoResetEvent> _resetEvents { get; set; } = new Dictionary<string, AutoResetEvent>();
        public static Dictionary<string, Thread> _threads { get; } = new Dictionary<string, Thread>();

        //private static Logger _logger { get; } = new Logger("ThreadLog", true);
        public static List<string> _markedForAbort { get; } = new List<string>();

        public static void ShowThreads(string name)
        {
            string lname = name.ToLower();
            if (_threads.ContainsKey(lname))
            {
                Console.WriteLine("Found it?");
            }            
        }

        public static void Build(Action method, string name, bool background = true, bool illegal = false)
        {
            Thread _thread;
            string lname = name.ToLower();
            if (!_threads.ContainsKey(lname))
            {
                try
                {

                    if (illegal)
                    {
                        _thread = new Thread(() =>
                        {
                            Control.CheckForIllegalCrossThreadCalls = false;
                            method();
                            Control.CheckForIllegalCrossThreadCalls = true;
                        });
                    }
                    else
                    {
                        _thread = new Thread(() => method());
                    }
                    _thread.Name = lname;
                    _threads.Add(lname, _thread);
                    _threads[lname].IsBackground = background;
                    _threads[lname].Start();
                }
                catch (Exception ex)
                {
                    //_logger.Push(ex);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }           
            else
            {
                //_logger.Push("I could not build the thread: "+ _threads[lname].Name);
            }
        }

        public static void Store(Thread thread)
        {
            if (!_threads.ContainsKey(thread.Name))
            {
                try
                {
                    _threads.Add(thread.Name ?? throw new InvalidOperationException(), thread);
                }
                catch (InvalidOperationException ex)
                {
                    //_logger.Push(ex);
                    throw new InvalidOperationException();
                }
                finally
                {
                    //_logger.Push("Thread " + thread.Name + " was stored.");
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public static void Bind(string name)
        {
            string lname = name.ToLower();
            if (!_resetEvents.ContainsKey(lname))
            {
                try
                {
                    AutoResetEvent zEvent = new AutoResetEvent(false);
                    _resetEvents.Add(lname ?? throw new InvalidOperationException(), zEvent);
                }
                catch (InvalidOperationException ex)
                {
                    //_logger.Push(ex);
                    throw new InvalidOperationException();
                }
                finally
                {
                    //_logger.Push("Zombie Thread Created: " + lname);
                }
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void Bind(Thread thread)
        {
            Thread _thread = thread;
            try
            {
                AutoResetEvent zEvent = new AutoResetEvent(false);
                _resetEvents.Add(_thread.Name ?? throw new InvalidOperationException(), zEvent);
            }
            catch (InvalidOperationException ex)
            {
                //_logger.Push(ex);
                throw new InvalidOperationException();
            }
            finally
            {
                //_logger.Push("Zombie Thread Created: " + _thread.Name);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void Unbind(string name)
        {
            string lname = name.ToLower();
            if (_resetEvents.ContainsKey(lname))
            {
                _resetEvents.Remove(lname);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void Sleep(string name)
        {
            string lname = name.ToLower();
            if (_resetEvents.ContainsKey(lname))
            {
                _resetEvents[lname].Reset();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void WakeUp(string name)
        {
            string lname = name.ToLower();
            if (_resetEvents.ContainsKey(lname))
            {
                _resetEvents[lname].Set();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void TimedWakeUp(string name, int ms)
        {
            string lname = name.ToLower();
            if (_resetEvents.ContainsKey(lname))
            {
                Thread.Sleep(ms);
                _resetEvents[lname].Set();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void InternalAborting()
        {
            while (true)
            {
                
               List<string> _mFAIN = new List<string>(_markedForAbort);
               if (_mFAIN.Any())
                {
                        foreach (string name in _mFAIN)
                    {
                        if (_threads.ContainsKey(name))
                        {
                            _threads[name].Abort();
                            _threads.Remove(name);
                            _markedForAbort.Remove(name);
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }

        public static void CloseThread(string name)
        {
            string lname = name.ToLower();
            try
            {
                if (_threads.ContainsKey(lname))
                {
                    if (!_markedForAbort.Contains(lname))
                    {
                        _markedForAbort.Add(lname);
                        //_logger.Push("Thread: " + lname + " is marked for purge");
                    }
                }

                if (!_resetEvents.ContainsKey(lname)) return;
                _resetEvents[lname].Close();
                _resetEvents.Remove(lname);
            }
            catch (Exception ex)
            {
                //_logger.Push(ex.ToString());
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void closeAll()
        {
            foreach (KeyValuePair<string, Thread> entry in _threads)
            {
                try
                {
                    Thread.Sleep(1);
                    entry.Value.Abort();
                }
                catch (ThreadAbortException ex)
                {
                    //_logger.Push(ex.ToString());
                }
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
