using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSVrammed
{
    //Text visible to user and not created by visual studio designer (menus, forms, dialogs, etc.).
    class Strings
    {
        //Old "AssemblyInfo.cs" style. NET core stores this in the .csproj file so edit this manually in there?
        //public const string ProductVersionMinor = "1";
        //public const string ProductVersionMajor = "1";
        //public const string ProductVersion = "v" + ProductVersionMajor + "." + ProductVersionMinor;
        //public const string ProductVersionAssembly = ProductVersionMajor + "." + ProductVersionMinor + ".0.0";
        //public const string CompanyName = "mechaskrom";
        //public const string Copyright = "Copyright © 2014";
        public const string ProductName = "PSVrammed";

        public const string AboutDescription = "PlayStation 1 emulator state file graphics viewer and simple editor. Written in C# and uses WinForms.";
        public const string AboutCreator = "Created by mechaskrom.";
        public const string AboutContact = "PayPal and contact: ";
        public const string AboutEMail = "mechaskrom@gmail.com";
        public const string AboutDonation = "If you like this program please consider donating to my PayPal account. Donations are highly appreciated and helpful.";

        public const string FormTitleNoFile = "(no file)";

        //State files.
        public const string OpenStateErrorCaption = "Error opening state";
        public const string OpenStateUnrecognized = "Unrecognized file! Tried formats and reason: \n";
        public const string OpenStateException = "Couldn't open state. Reason: ";
        public const string SaveStateErrorCaption = "Error saving state";
        public const string SaveStateException = "Couldn't save state. Reason: ";
        public const string SaveChangesWarningCaption = "Save changes?";
        public const string SaveChangesWarning = "State file has been changed. Do you want to save changes?";

        //Texture image png-files.
        public const string SaveTextureErrorCaption = "Error saving texture";
        public const string SaveTextureException = "Couldn't save texture. Reason: ";

        //Edit files.
        public const string SaveEditErrorCaption = "Error saving edit sequence";
        public const string SaveEditException = "Couldn't save edit sequence. Reason: ";
        public const string OpenEditErrorCaption = "Error opening edit sequence";
        public const string OpenEditException = "Couldn't open edit sequence. Reason: ";
        public const string OpenEditSignatureErrorMsg = "Signature not found!";
        public const string OpenEditVersionErrorMsg = "Version not found or not supported!";
        public const string OpenEditReadErrorMsg = "Could not read edit command on line {0} because {1}!"; //Line nr + reason.
        public const string OpenEditReadErrorMsgUnknown = "unknown save id";
        public const string OpenEditReadErrorMsgMissing = "too few fields";

        //State info txt-files.
        public const string SaveInfoErrorCaption = "Error saving state info";
        public const string SaveInfoException = "Couldn't save state info. Reason: ";

        //Mode.
        public const string ModeNoFile = "(no file)";
        public const string Mode4BitView = "4 bit indexed view";
        public const string Mode4BitComp = "4 bit indexed compare";
        public const string Mode4BitPalE = "4 bit indexed palette editor";
        public const string Mode8BitView = "8 bit indexed view";
        public const string Mode8BitComp = "8 bit indexed compare";
        public const string Mode8BitPalE = "8 bit indexed palette editor";
        public const string Mode16BitView = "16 bit rgb view";
        public const string Mode24BitView = "24 bit rgb view";
        public const string ModeUnknown = "unknown";
    }
}
