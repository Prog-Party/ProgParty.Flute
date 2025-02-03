using System;
using System.Windows;
using System.IO.IsolatedStorage;
using System.IO;
using Fluit.Resources;
using Microsoft.Phone.Tasks;

namespace Fluit.Classes
{
    public class LittleWatson
    {
        const string Filename = "LittleWatson.txt";

        internal static void ReportException(Exception ex, string extra)
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    SafeDeleteFile(store);

                    using (TextWriter output = new StreamWriter(store.CreateFile(Filename)))
                    {
                        output.WriteLine(extra);
                        output.WriteLine(ex.Message);
                        output.WriteLine(ex.StackTrace);
                        //make recursive
                        output.WriteLine(ex.InnerException ?? new Exception("no inner"));
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        internal static void CheckForPreviousException(bool wasLastTime)
        {
            try
            {
                string contents = null;
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(Filename))
                    {
                        using (TextReader reader = new StreamReader(store.OpenFile(Filename, FileMode.Open, FileAccess.Read, FileShare.None)))
                        {
                            contents = reader.ReadToEnd();
                        }
                        SafeDeleteFile(store);
                    }
                }
                if (contents != null)
                {
                    String message;
                    if (wasLastTime)
                        message = AppResources.LittleWatsonSendMail;
                    else
                        message = AppResources.LittleWatsonUnexpectedError;

                    Threads.ForegroundDispatcher.BeginInvoke(() =>
                    {
                        if (MessageBox.Show(message, AppResources.UnexpectedError, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            EmailComposeTask email = new EmailComposeTask
                            {
                                To = "JensDennisBV@gmail.com",
                                Subject = "Flute Instrument auto-generated problem report",
                                Body = contents
                            };
                            email.Show();
                        }
                    });
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                SafeDeleteFile(IsolatedStorageFile.GetUserStoreForApplication());
            }
        }

        private static void SafeDeleteFile(IsolatedStorageFile store)
        {
            try
            {
                store.DeleteFile(Filename);
            }
            catch (Exception)
            {
            }
        }
    }
}
