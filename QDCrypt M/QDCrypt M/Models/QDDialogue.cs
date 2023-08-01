using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDCrypt.Models
{
    public static class QDDialogue
    {
        public static (string, string, string) HashingError()
        {
            return ("Error!", "Invalid password character(s).", "OK");
        }

        public static (string, string, string) EncryptionSuccess()
        {
            return ("Notice!", "Encryption complete.", "OK");
        }

        public static (string, string, string) DecryptionSuccess()
        {
            return ("Notice!", "Decryption complete.", "OK");
        }

        public static (string, string, string) EncryptionError()
        {
            return ("Encryption Failed!", "Please make sure to encrypt at least 1 character.", "OK");
        }

        public static (string, string, string) DecryptionError()
        {
            return ("Decryption Failed!", "Incorrect password or cipher.", "OK");
        }

        public static (string, string, string) CopyError()
        {
            return ("Error!", "Failed to copy text(s).", "OK");
        }

        public static (string, string, string) MaxPasswordNotice()
        {
            return ("Notice!", "You reached the 250 character limit on password.", "OK");
        }

        public static (string, string, string, string) CipherEditWarning()
        {
            return ("Warning!", "Are you sure you want edit the encryption?", "OK", "Cancel");
        }

        public static (string, string, string, string) ClearWarning()
        {
            return ("Warning!", "Are you sure you want to clear all texts?", "OK", "Cancel");
        }
    }
}
