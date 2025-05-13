using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyCustomStuff
{
    //Keyboard keys can be done with a giant switch, but it is hard to change and manage.
    //And menu shortcut keys needs to be handled also. It is a lot of code to handle and
    //makes changes hard to do. This is my try to gather all keyboard functionality to one place.
    //It also makes it easy to print out a table with all keys used for documentation.

    //Also does command to menu items, mostly to avoid the problem with PerformClick() and
    //disabled menu items (see below for more info). Maybe also a bit nicer than all simple
    //click events needed in form main/designer?

    //Using a menu click to action map has a few advantages compared to calling PerformClick().
    //addCmd(description, keys, allowRepeat, menuItem, menuItem.PerformClick);
    //PerformClick has a bit more overhead, but a bigger problem is that the menu item must be enabled.
    //If it is disabled (e.g. in a drop down opening event) it must be reenabled
    //again (e.g. in a drop down closed event) before the key handler will work.
    //foreach (ToolStripItem item in ((ToolStripMenuItem)sender).DropDownItems / ((ContextMenuStrip)sender).Items)
    //{
    //    item.Enabled = true;
    //}

    //For mapping application commands to keyboard keys and menu items.
    public class CmdHandler
    {
        private class KeyAction
        {
            private readonly bool mCanRepeat;
            private readonly Action mAction;

            public KeyAction(bool canRepeat, Action action)
            {
                mCanRepeat = canRepeat;
                mAction = action;
            }

            public void call(bool isRepeat)
            {
                if (!isRepeat || mCanRepeat)
                {
                    //Log.timeBegin();
                    mAction();
                    //Log.timeEnd("key action()");
                }
            }
        }

        private readonly Dictionary<Keys, KeyAction> mKeyMap;
        private readonly Dictionary<ToolStripMenuItem, Action> mMenuMap;
        private readonly StringBuilder mKeyTableBuilder;

        public CmdHandler()
            : this(null)
        {
        }

        public CmdHandler(StringBuilder keyTableBuilder)
        {
            mKeyMap = new Dictionary<Keys, KeyAction>();
            mMenuMap = new Dictionary<ToolStripMenuItem, Action>();

            //Provide a StringBuilder to enable key table printing.
            mKeyTableBuilder = keyTableBuilder;
        }

        public bool HasKeyTableBuilder
        {
            get { return mKeyTableBuilder != null; }
        }

        public void addCmd(string description, Action action, params Keys[] keys)
        {
            addCmd(description, action, null, false, keys);
        }

        public void addCmd(string description, Action action, bool canRepeat, params Keys[] keys)
        {
            addCmd(description, action, null, canRepeat, keys);
        }

        public void addCmd(string description, Action action, ToolStripMenuItem menuItem, params Keys[] keys)
        {
            addCmd(description, action, menuItem, false, keys);
        }

        public void addCmd(string description, Action action, ToolStripMenuItem menuItem, bool canRepeat, params Keys[] keys)
        {
            if (action == null)
            {
                throw new ArgumentException("Command need an action!");
            }
            if (keys.Length == 0 && menuItem == null)
            {
                throw new ArgumentException("Command need a shortcut key or a menu item!");
            }

            //Log.timeBegin();

            if (keys.Length > 0) //Command has a shortcut key?
            {
                KeyAction keyAction = new KeyAction(canRepeat, action);
                foreach (Keys k in keys)
                {
                    mKeyMap.Add(k, keyAction);
                }

                if (mKeyTableBuilder != null)
                {
                    appendKeyTableBuild(description, keys);
                }
            }

            if (menuItem != null) //Command has a menu item associated with it?
            {
                if (keys.Length > 0)
                {
                    menuItem.ShortcutKeyDisplayString = keyToString(keys[0]);
                }

                //Perform action if item is clicked.
                mMenuMap.Add(menuItem, action);
                menuItem.Click += item_Click;
            }

            //Log.timeEnd("addCmd()");
        }

        private void item_Click(object sender, EventArgs e)
        {
            //Log.add("form main item_Click() " + sender);
            mMenuMap[(ToolStripMenuItem)sender].Invoke();
        }

        private void appendKeyTableBuild(string description, params Keys[] keys)
        {
            HashSet<string> keyStrings = new HashSet<string>();
            foreach (Keys k in keys)
            {
                string keyString = keyToString(k);
                if (keyStrings.Add(keyString)) //Append only unique key strings.
                {
                    if (keyStrings.Count == 1) //First key?
                    {
                        mKeyTableBuilder.Append(description.PadRight(60));
                    }
                    else
                    {
                        mKeyTableBuilder.Append(" or ");
                    }
                    mKeyTableBuilder.Append(keyString);
                }
            }
            mKeyTableBuilder.AppendLine();
        }

        public bool call(Keys keys, Message msg) //Return true if key was mapped to an action.
        {
            bool isRepeat = (msg.LParam.ToInt32() & 0x40000000) != 0;
            return call(keys, isRepeat);
        }

        private bool call(Keys keys, bool isRepeat)
        {
            KeyAction keyAction;
            if (mKeyMap.TryGetValue(keys, out keyAction))
            {
                keyAction.call(isRepeat);
                return true;
            }
            return false;
        }

        private string keyToString(Keys key)
        {
            //System.Windows.Forms.KeysConverter is nicer but slower.
            string mod = "";
            if ((key & Keys.Control) != 0) mod += "Ctrl+";
            if ((key & Keys.Shift) != 0) mod += "Shift+";
            if ((key & Keys.Alt) != 0) mod += "Alt+";

            string code = "";
            switch (key & Keys.KeyCode)
            {
                case Keys.F1: code = "F1"; break;
                case Keys.F2: code = "F2"; break;
                case Keys.F3: code = "F3"; break;
                case Keys.F4: code = "F4"; break;
                case Keys.F5: code = "F5"; break;
                case Keys.F6: code = "F6"; break;
                case Keys.F7: code = "F7"; break;
                case Keys.F8: code = "F8"; break;
                case Keys.F9: code = "F9"; break;
                case Keys.F10: code = "F10"; break;
                case Keys.F11: code = "F11"; break;
                case Keys.F12: code = "F12"; break;
                case Keys.Delete: code = "Delete"; break;
                case Keys.Up: code = "Up"; break;
                case Keys.Down: code = "Down"; break;
                case Keys.Left: code = "Left"; break;
                case Keys.Right: code = "Right"; break;
                case Keys.Add: code = "+"; break;
                case Keys.Subtract: code = "-"; break;
                case Keys.Oemplus: code = "+"; break;
                case Keys.OemMinus: code = "-"; break;
                case Keys.Home: code = "Home"; break;
                case Keys.PageUp: code = "PageUp"; break;
                case Keys.PageDown: code = "PageDown"; break;
                case Keys.Space: code = "Space"; break;
                case Keys.Enter: code = "Enter"; break;
                default: code = ((char)key).ToString(); break;
            }
            return mod + code;
        }

        public void printKeyTable()
        {
            if (HasKeyTableBuilder)
            {
                string path = Misc.GetExeBasePath();
                File.WriteAllText(path + "\\key table.txt", mKeyTableBuilder.ToString(), Encoding.UTF8);
            }
            else
            {
                throw new InvalidOperationException(
                    "Cannot print key table because no string builder was provided at construction.");
            }
        }
    }
}
