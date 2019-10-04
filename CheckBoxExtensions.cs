using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


static class CheckBoxExtensions
{
    static public void Check(this CheckBox checkBox, bool value)
    {
        checkBox.Checked = value;
        if (value)
            checkBox.CheckState = CheckState.Checked;
        else
            checkBox.CheckState = CheckState.Unchecked;
    }
}
